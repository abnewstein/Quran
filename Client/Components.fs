namespace QuranClient

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open QuranLibrary

[<JavaScript>]
module QuranOps =
    let primaryOrSecondaryQuran index (quranData: array<Quran>) =
        if quranData.Length > index then Some quranData.[index] else None
    
    let chapterNamesAtIndex index (quranData: array<Quran>) =
        quranData
        |> primaryOrSecondaryQuran index
        |> Option.map Quran.GetChapterNames

[<JavaScript>]
module Components =

    let quranData = State.QuranDataVar

    module Reader =
        type ChapterNameList = array<(string * string)>
        let GoToChapter chapterNumber = fun _ -> State.SetRouterVar (Chapter $"{chapterNumber}")
        
        let ChapterListDoc =
            let renderChapterNames chapterNames =
                chapterNames
                |> Array.mapi (fun i (c1, c2) ->
                    li [] [
                        Doc.Link c1 [] (GoToChapter (i + 1))
                        text " "
                        Doc.Link c2 [] (GoToChapter (i + 1))
                    ]
                )
                |> Doc.Concat

            quranData
            |> View.Map (function
                | [| primary; secondary |] ->
                    let NamePairs = Array.zip (Quran.GetChapterNames primary) (Quran.GetChapterNames secondary)
                    ul [] [renderChapterNames NamePairs]
                | _ -> p [] [text "Loading..."]
            )
            |> Doc.EmbedView
        
        let VerseListDoc chapterNumber =
            let renderVerses (verses: array<Verse * Verse>) =
                verses
                |> Array.mapi (fun i (v1, v2) ->
                    li [] [
                        p [] [text v1.Text]
                        p [] [text v2.Text]
                    ]
                )
                |> Doc.Concat
            
            quranData
            |> View.Map (function
                | [| primary; secondary |] ->
                    let primaryVerses = Quran.GetVersesByChapter primary chapterNumber
                    let secondaryVerses = Quran.GetVersesByChapter secondary chapterNumber
                    let verses = Array.zip primaryVerses secondaryVerses
                    ul [] [renderVerses verses]
                    
                | _ -> p [] [text "Loading..."]
            )
            |> Doc.EmbedView
