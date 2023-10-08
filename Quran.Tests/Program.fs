module Program =

    open Quran
    open System


    VerseRef.Of 100 100 |> printfn "%A"

    let favVerse = VerseRef.fromString "1:1"

    favVerse.Value == (VerseRef.fromString "1:1").Value |> printfn "%b"
