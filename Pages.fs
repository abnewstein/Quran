module Pages

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open Routes
open QuranLib
open QuranService


type Props = (EndPoint -> unit) * array<Quran>

[<JavaScript>]
let HomePage (props: Props) =
    let go, quranData = props
    // show a list of chapters
    if quranData.Length > 0 then
        let quran = quranData.[0]
        let chapters = quran.Chapters
        let chapterLinks = chapters |> Array.map (fun c ->
            let chapterNumber = c.Number
            let chapterName = c.Name
            let chapterLink = Doc.Link chapterName [] (fun _ -> go (Chapter (string chapterNumber)))
            li [] [chapterLink]
        )
        Doc.Concat [
            h1 [] [text "Home"]
            p [] [text "This is the home page" ]
            ul [] chapterLinks
            Doc.Link "Go to About" [] (fun _ -> go About)
        ]
    else
        Doc.Concat [
            h1 [] [text "Home"]
            p [] [text "This is the home page" ]
            Doc.Link "Go to About" [] (fun _ -> go About)
        ]

[<JavaScript>]
let AboutPage (props: Props) =
    let go, _ = props
    Doc.Concat [
        h1 [] [text "About"]
        p [] [text "This is the about page" ]
        Doc.Link "Go to Home" [] (fun _ -> go Home)
    ]

[<JavaScript>]
let ChapterPage (props: Props) num =
    let go, quranData = props
    Doc.Concat [
        h1 [] [text "Chapter"]
        p [] [text num]
        Doc.Link "Go to Home" [] (fun _ -> go Home)
        Doc.Link "Go to About" [] (fun _ -> go About)
    ]