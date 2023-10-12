module Program =

    open Quran
    open System

    // NoteRef.fromString "1:1:1" |> printfn "%A"
    // FileParser.getAvailableTranslations () |> printfn "%A"
    let quranData: Quran list = FileParser.getAvailableQuranData ()

    quranData.Tail.Head.Chapters[0].Verses |> printfn "%A"
