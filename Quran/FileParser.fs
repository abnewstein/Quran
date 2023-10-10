namespace Quran

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
    open System
    open System.IO
    open FSharpPlus
    open Constants
    open Utilities.Functions
    open Thoth.Json.Net

    let PATH = "./data"
    let CHAPTERS_JSON_PATH = $"{PATH}/chapters"
    let VERSES_JSON_PATH = $"{PATH}/verses"
    let NOTES_JSON_PATH = $"{PATH}/notes"

    type QuranFile = { path: string; content: DataFileType }

    let getFileNames (path: string) : string array =
        Directory.GetFiles path |> Array.map Path.GetFileNameWithoutExtension

    let parseFileName (fileName: string) : Translation option =
        fileName.Split('_')
        |> Array.toList
        |> function
            | language :: author :: [] -> Translation.Of (Author author) (Language language) |> Option.Some
            | _ -> None

    let readAllText (filePath: string) =
        File.ReadAllText filePath |> Option.ofObj
