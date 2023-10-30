namespace QuranWeb

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open QuranLib

[<JavaScript>]
module ViewOps =
    let PrimaryQuran (quranData: array<Quran>) =
        match quranData.Length with
        | 0 -> None
        | _ -> Some quranData[0]
    let SecondaryQuran (quranData: array<Quran>) =
        match quranData.Length with
        | 0 -> None
        | _ -> Some quranData[1]
    let ChapterNames1 (quranData: array<Quran>) =
        quranData 
        |> PrimaryQuran 
        |> Option.map Quran.GetChapterNames
    let ChapterNames2 (quranData: array<Quran>) =
        quranData 
        |> SecondaryQuran 
        |> Option.map Quran.GetChapterNames

[<JavaScript>]
module Components =
    let ChapterListDoc (quranData: array<Quran>) =
        let names1 = ViewOps.ChapterNames1 quranData
        let names2 = ViewOps.ChapterNames2 quranData
        let pairedNames = 
            match names1, names2 with
            | Some names1, Some names2 -> Array.zip names1 names2
            | _ -> [||]
        div [] [
            ul [] [
                pairedNames 
                |> Array.map (fun (name1, name2) ->
                    li [] [text $"{name1} - {name2}"])
                    |> Doc.Concat
            ]
        ]
