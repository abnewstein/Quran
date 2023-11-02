namespace QuranWeb

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open QuranLib

[<JavaScript>]
module QuranOps =
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

    type NameList = string array
    type NameListView = View<NameList option>
    let quranData = State.quranDataVar
    let ChapterListDoc =
        let names1: NameListView = quranData |> View.Map QuranOps.ChapterNames1
        let names2: NameListView = quranData |> View.Map QuranOps.ChapterNames2
        
        Doc.Empty