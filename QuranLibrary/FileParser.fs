namespace QuranLibrary

open System.IO
open System.Reflection
open Thoth.Json.Net

type ChaptersJson = list<int * string>
type VersesJson = list<int * int * string>
type NotesJson = list<string * int * string>

module Decoder =
    let decodeList = Decode.list
    let decodeTuple2 = Decode.tuple2
    let decodeTuple3 = Decode.tuple3
    let decodeInt = Decode.int
    let decodeString = Decode.string

    let chaptersDecoder: Decoder<ChaptersJson> =
        (decodeTuple2 decodeInt decodeString) |> decodeList

    let versesDecoder: Decoder<VersesJson> =
        (decodeTuple3 decodeInt decodeInt decodeString) |> decodeList

    let notesDecoder: Decoder<NotesJson> =
        (decodeTuple3 decodeString decodeInt decodeString) |> decodeList

    let constructVerses (versesJson: VersesJson) : array<Verse> =
        versesJson
        |> List.map (fun (chapterNumber, verseNumber, text) ->
            match VerseRef.Of(chapterNumber, verseNumber) with
            | Some ref -> Verse.Of ref text [||]
            | None -> None)
        |> List.choose id // Remove None and unpack Some
        |> Array.ofList

    let constructChapters (chaptersJson: ChaptersJson) (verses: array<Verse>) : array<Chapter> =
        chaptersJson
        |> List.map (fun (chapterNumber, name) ->
            match verses |> Array.filter (fun v -> v.Ref.ChapterNumber = chapterNumber) with
            | verses when verses.Length > 0 -> Chapter.Of chapterNumber name verses
            | _ -> None)
        |> List.choose id // Remove None and unpack Some
        |> Array.ofList

    let constructQuran (chaptersJsonStr: string) (versesJsonStr: string) (translation: Translation) : Quran =
        let chapters = chaptersJsonStr |> Decode.fromString chaptersDecoder
        let verses = versesJsonStr |> Decode.fromString versesDecoder

        match chapters, verses with
        | Ok chapters, Ok verses ->
            constructVerses verses
            |> constructChapters chapters
            |> Quran.Of translation
        | _ -> failwith "Failed to parse chapters or verses, check the json files"

module FileParser =
    let RESOURCE_PATH = "Quran.data"
    let assembly = Assembly.GetExecutingAssembly()

    let ReadEmbeddedResource resourceName =
        printfn "Reading embedded resource: %s" resourceName
        use stream = assembly.GetManifestResourceStream(resourceName)
        use reader = new StreamReader(stream)
        reader.ReadToEnd()

    let GetResourceNames () =
        assembly.GetManifestResourceNames()
        |> Array.map Path.GetFileNameWithoutExtension
        |> Set.ofArray

    let ParseFileName (fileName: string) =
        fileName.Split('_')
        |> Array.toList
        |> function
            | language :: author :: [] -> Some(Translation.Of (Author author) (Language language))
            | _ -> None

    let ParseResourceNames (resourceNames: string Set) =
        resourceNames
        |> Set.map (fun s -> s.Split('.') |> Array.last)
        |> Set.map ParseFileName
        |> Set.filter Option.isSome
        |> Set.map Option.get

    let GetFilesStartingWith (prefix: string) =
        GetResourceNames () |> Set.filter (fun s -> s.StartsWith(prefix))

    let GetAvailableTranslations () =
        let chaptersFiles = GetFilesStartingWith $"{RESOURCE_PATH}.chapters"
        let versesFiles = GetFilesStartingWith $"{RESOURCE_PATH}.verses"

        let chTrans, vsTrans =
            ParseResourceNames chaptersFiles, ParseResourceNames versesFiles

        Set.intersect chTrans vsTrans

    let GetJsonResource (kind: string) (translation: Translation) =
        ReadEmbeddedResource $"{RESOURCE_PATH}.{kind}.{translation}.json"

    let GetChaptersJson = GetJsonResource "chapters"
    let GetVersesJson = GetJsonResource "verses"
    let GetNotesJson = GetJsonResource "notes"
    let ConstructQuranFromJson (translation: Translation) : Quran =
        let chaptersJsonStr = GetChaptersJson translation
        let versesJsonStr = GetVersesJson translation

        printfn "Constructing Quran for %s" (string translation)
        Decoder.constructQuran chaptersJsonStr versesJsonStr translation
    
    let AvailableQuranData () : array<Quran> =
        GetAvailableTranslations () |> Set.map ConstructQuranFromJson |> Set.toArray