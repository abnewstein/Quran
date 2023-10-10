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

module FileParser =

    let assembly = Assembly.GetExecutingAssembly()

    let readEmbeddedResource (resourceName: string) =
        use stream = assembly.GetManifestResourceStream(resourceName)
        use reader = new StreamReader(stream)
        reader.ReadToEnd()

    let getResourceNames () =
        assembly.GetManifestResourceNames()
        |> Array.map Path.GetFileNameWithoutExtension
        |> Set.ofArray

    let parseFileName (fileName: string) : Translation option =
        fileName.Split('_')
        |> Array.toList
        |> function
            | language :: author :: [] -> Translation.Of (Author author) (Language language) |> Option.Some
            | _ -> None

    let parseResourceNames (resourceNames: string Set) : Translation Set =
        resourceNames
        |> Set.map (fun s -> s.Split('.') |> Array.last)
        |> Set.map parseFileName
        |> Set.filter Option.isSome
        |> Set.map Option.get

    let getChaptersFiles (a: string Set) : string Set =
        a |> Set.filter (fun s -> s.StartsWith("Quran.data.chapters"))

    let getVersesFiles (a: string Set) : string Set =
        a |> Set.filter (fun s -> s.StartsWith("Quran.data.verses"))

    let getNotesFiles (a: string Set) : string Set =
        a |> Set.filter (fun s -> s.StartsWith("Quran.data.notes"))

    let getAvailableTranslations () =
        getResourceNames ()
        |> fun a -> (getChaptersFiles a, getVersesFiles a)
        |> fun (a, b) -> parseResourceNames a, parseResourceNames b
        |> fun (a, b) -> Set.intersect a b
