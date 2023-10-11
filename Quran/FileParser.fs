namespace Quran

open System.IO
open System.Reflection
open Thoth.Json.Net

type ChaptersJson = (int * string) list
type VersesJson = (int * int * string) list
type NotesJson = (string * int * string) list

type DataFileType =
    | ChaptersJson of ChaptersJson
    | VersesJson of VersesJson
    | NotesJson of NotesJson

type DecoderType =
    | Chapters of Decoder<ChaptersJson>
    | Verses of Decoder<VersesJson>
    | Notes of Decoder<NotesJson>

module Decoder =

    let decodeList = Decode.list
    let decodeTuple2 = Decode.tuple2
    let decodeTuple3 = Decode.tuple3
    let decodeInt = Decode.int
    let decodeString = Decode.string

    let ChaptersDecoder: DecoderType =
        (decodeTuple2 decodeInt decodeString) |> decodeList |> Chapters

    let VersesDecoder: DecoderType =
        (decodeTuple3 decodeInt decodeInt decodeString) |> decodeList |> Verses

    let NotesDecoder: DecoderType =
        (decodeTuple3 decodeString decodeInt decodeString) |> decodeList |> Notes

    let decode (decoder: DecoderType) (json: string) =
        match decoder with
        | Chapters decoder -> Decode.fromString decoder json |> Result.map ChaptersJson
        | Verses decoder -> Decode.fromString decoder json |> Result.map VersesJson
        | Notes decoder -> Decode.fromString decoder json |> Result.map NotesJson

    let decodeVersesJson (a: Result<DataFileType, string>) : Verse list =
        match a with
        | Ok(VersesJson versesJson) ->
            versesJson
            |> List.map (fun (chapterNumber, verseNumber, text) ->
                { Ref =
                    { ChapterNumber = chapterNumber
                      VerseNumber = verseNumber }
                  Text = text
                  Notes = [||] })
        | _ -> []

    let decodeChaptersJson (a: Result<DataFileType, string>) : Chapter list =
        match a with
        | Ok(ChaptersJson chaptersJson) ->
            chaptersJson
            |> List.map (fun (number, name) ->
                { Number = number
                  Name = name
                  Verses = [||] })
        | _ -> []

module FileParser =

    let assembly = Assembly.GetExecutingAssembly()

    let readEmbeddedResource resourceName =
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
        sprintf "Quran.data.%s.%A_%A.json" kind translation.Language translation.Author
        |> readEmbeddedResource

    let getChaptersJson = getJsonResource "chapters"
    let getVersesJson = getJsonResource "verses"
    let getNotesJson = getJsonResource "notes"

// Construct Quran from verses Json
