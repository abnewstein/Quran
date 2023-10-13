namespace Quran

open System.IO
open System.Reflection
open Thoth.Json.Net

type ChaptersJson = (int * string) list
type VersesJson = (int * int * string) list
type NotesJson = (string * int * string) list

module Decoder =

    let decodeList = Decode.list
    let decodeTuple2 = Decode.tuple2
    let decodeTuple3 = Decode.tuple3
    let decodeInt = Decode.int
    let decodeString = Decode.string

    let decodeChapters: Decoder<ChaptersJson> =
        (decodeTuple2 decodeInt decodeString) |> decodeList

    let decodeVerses: Decoder<VersesJson> =
        (decodeTuple3 decodeInt decodeInt decodeString) |> decodeList

    let decodeNotes: Decoder<NotesJson> =
        (decodeTuple3 decodeString decodeInt decodeString) |> decodeList

    let constructVerses (versesJson: VersesJson) : Verse array =
        versesJson
        |> List.map (fun (chapterNumber, verseNumber, text) ->
            match VerseRef.Of chapterNumber verseNumber with
            | Some ref -> Verse.Of ref text [||]
            | None -> None)
        |> List.choose id // Remove None and unpack Some
        |> Array.ofList

    let constructChapters (chaptersJson: ChaptersJson) (verses: Verse array) : Chapter array =
        chaptersJson
        |> List.map (fun (chapterNumber, name) ->
            match verses |> Array.filter (fun v -> v.Ref.ChapterNumber = chapterNumber) with
            | verses when verses.Length > 0 -> Chapter.Of chapterNumber name verses
            | _ -> None)
        |> List.choose id // Remove None and unpack Some
        |> Array.ofList

    let constructQuran (chapters: Chapter array) (translation: Translation) : Quran = Quran.Of translation chapters

module FileParser =

    let assembly = Assembly.GetExecutingAssembly()

    let readEmbeddedResource resourceName =
        printfn "Reading embedded resource: %s" resourceName
        use stream = assembly.GetManifestResourceStream(resourceName)
        use reader = new StreamReader(stream)
        reader.ReadToEnd()

    let getResourceNames () =
        assembly.GetManifestResourceNames()
        |> Array.map Path.GetFileNameWithoutExtension
        |> Set.ofArray

    let parseFileName (fileName: string) =
        fileName.Split('_')
        |> Array.toList
        |> function
            | language :: author :: [] -> Some(Translation.Of (Author author) (Language language))
            | _ -> None

    let parseResourceNames (resourceNames: string Set) =
        resourceNames
        |> Set.map (fun s -> s.Split('.') |> Array.last)
        |> Set.map parseFileName
        |> Set.filter Option.isSome
        |> Set.map Option.get

    let getFilesStartingWith (prefix: string) =
        getResourceNames () |> Set.filter (fun s -> s.StartsWith(prefix))

    let getAvailableTranslations () =
        let chaptersFiles = getFilesStartingWith "Quran.data.chapters"
        let versesFiles = getFilesStartingWith "Quran.data.verses"

        let chTrans, vsTrans =
            parseResourceNames chaptersFiles, parseResourceNames versesFiles

        Set.intersect chTrans vsTrans

    let getJsonResource (kind: string) (translation: Translation) =
        readEmbeddedResource $"Quran.data.{kind}.{translation}.json"

    let getChaptersJson = getJsonResource "chapters"
    let getVersesJson = getJsonResource "verses"
    let getNotesJson = getJsonResource "notes"

    // Construct Quran from verses Json
    let constructQuranObj (chaptersJsonStr: string) (versesJsonStr: string) (translation: Translation) : Quran =
        let chapters = chaptersJsonStr |> Decode.fromString Decoder.decodeChapters
        let verses = versesJsonStr |> Decode.fromString Decoder.decodeVerses

        match chapters, verses with
        | Ok chapters, Ok verses ->
            Decoder.constructVerses verses
            |> Decoder.constructChapters chapters
            |> Decoder.constructQuran
            <| translation
        | _ -> failwith "Failed to parse chapters or verses, check the json files"

    let constructQuranFromJson (translation: Translation) : Quran =
        (getChaptersJson translation, getVersesJson translation) ||> constructQuranObj
        <| translation

    let getAvailableQuranData () : Quran array =
        getAvailableTranslations () |> Set.map constructQuranFromJson |> Set.toArray
