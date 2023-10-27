module Program =

    open Quran
    open System

    // NoteRef.fromString "1:1:1" |> printfn "%A"
    // FileParser.getAvailableTranslations () |> printfn "%A"
    let quranData: Quran array = Service.getAvailableQuranData ()

    let searchResult = Quran.filterVersesByTextWithScore quranData[1] "not equal"

    // print only the first 10 results
    searchResult
    |> Array.sortByDescending snd
    |> Array.take 10
    |> Array.iter (fun (verse, score) -> printfn "%A %f" verse score)
