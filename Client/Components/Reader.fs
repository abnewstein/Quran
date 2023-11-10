namespace QuranClient

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open QuranLibrary
open Utilities

[<JavaScript>]
module Reader =
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

        State.QuranData
        |> View.Map (function
            | q when q.Length >= 2 ->
                let primary = QuranArray.PrimaryQuran q
                let secondary = QuranArray.SecondaryQuran q
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
        
        State.QuranData
        |> View.Map (function
            | [| primary; secondary |] ->
                let primaryVerses = Quran.GetVersesByChapter primary chapterNumber
                let secondaryVerses = Quran.GetVersesByChapter secondary chapterNumber
                let verses = Array.zip primaryVerses secondaryVerses
                ul [] [renderVerses verses]
                
            | _ -> p [] [text "Loading..."]
        )
        |> Doc.EmbedView
