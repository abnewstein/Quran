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

    // Get all directories in the data directory, those are chapters, verses, and notes
    let getDirectories () = Directory.GetDirectories PATH

    // Get all files in a directory
    let getFiles (directory: string) = Directory.GetFiles directory

// Get available translations based on the files in the data directory
// if chapters and verses are available, then translations are available.   notes are optional
