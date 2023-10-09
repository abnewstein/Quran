namespace Quran

type DataFileType =
    | Chapter
    | Verse
    | Note

module FileParser =
    open System
    open System.IO
    open FSharpPlus
    open Constants
    open Utilities.Functions


    let PATH = "./data"

    let getDirectories () = Directory.GetDirectories PATH

    let getFiles (directory: string) = Directory.GetFiles directory

// Get available translations based on the files in the data directory
// if chapters and verses are available, then translations are available.   notes are optional
