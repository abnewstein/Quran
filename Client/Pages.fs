namespace QuranClient

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open QuranLibrary

[<JavaScript>]
module Pages =
    let Go = State.SetRouterVar

    let HomePage =
        let chapterList = Reader.ChapterListDoc
        Doc.Concat [
            h1 [] [text "Home"]
            Doc.Link "About" [] (fun _ -> Go About)
            chapterList
        ]
        
    let AboutPage =
        Doc.Concat [
            h1 [] [text "About"]
            p [] [text "This is the about page" ]
            Doc.Link "Home" [] (fun _ -> Go Home)
        ]

    let ChapterPage num = 
        Doc.Concat [
            h1 [] [text "Chapter"]
            p [] [text num]
            Reader.VerseListDoc (int num)
            Doc.Link "Home" [] (fun _ -> Go Home)
            Doc.Link "About" [] (fun _ -> Go About)
        ]