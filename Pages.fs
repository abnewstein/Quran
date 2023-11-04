namespace QuranWeb

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open QuranWeb
open QuranLib

[<JavaScript>]
module Pages =
    let Go = State.SetRouterVar

    let HomePage =
        let chapterList = Components.Reader.ChapterListDoc
        Doc.Concat [
            h1 [] [text "Home"]
            p [] [text "This is the home page" ]
            Doc.Link "Go to About" [] (fun _ -> Go About)
            chapterList
        ]
        
    let AboutPage =
        Doc.Concat [
            h1 [] [text "About"]
            p [] [text "This is the about page" ]
            Doc.Link "Go to Home" [] (fun _ -> Go Home)
        ]

    let ChapterPage num =
        Doc.Concat [
            h1 [] [text "Chapter"]
            p [] [text num]
            Components.Reader.VerseListDoc (int num)
            Doc.Link "Go to Home" [] (fun _ -> Go Home)
            Doc.Link "Go to About" [] (fun _ -> Go About)
        ]