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

    let quranData = State.QuranDataVar

    module Reader =
        type ChapterNameList = array<(string * string)>
        let ChapterListDoc =
            quranData
            |> View.Map (fun quranData ->
                match quranData with
                | quranData when quranData.Length > 0 ->
                    let ChapterNames1 = QuranOps.ChapterNames1 quranData
                    let ChapterNames2 = QuranOps.ChapterNames2 quranData
                    let ChapterNames: ChapterNameList =
                        match ChapterNames1, ChapterNames2 with
                        | Some c1, Some c2 ->
                            Array.zip c1 c2                            
                        | _ -> [||]
                    let chapterList =
                        ChapterNames
                        |> Array.map (fun (num, name) ->
                            li [] [Doc.Link name [] (fun _ -> State.SetRouterVar (Chapter num))]
                        )
                        |> Doc.Concat
                    
                    ul [] [chapterList]
                | _ -> p [] [text "No quran data"]
            )
            |> Doc.EmbedView
            