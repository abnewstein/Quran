module Pages

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open Routes
open QuranLib
open Quran
open QuranService

[<JavaScript>]
let HomePage props =
    let go, quranData = props
    // show a list of chapter names
    let primaryQuran = quranData |> Array.filter (fun q -> q.Translation.Language = Language "ar") |> Array.head
    let secondaryQuran = quranData |> Array.filter (fun q -> q.Translation = Translation.Of (Author "sam-gerrans") (Language "en")) |> Array.head
    let chapterNamesAr = primaryQuran.Chapters |> Array.map (fun c -> c.Name)
    let chapterNamesEn = secondaryQuran.Chapters |> Array.map (fun c -> c.Name)
    let chapterNames = Array.zip chapterNamesAr chapterNamesEn
    let chapterLinks = chapterNames |> Array.map (fun (ar, en) -> Doc.Link ar [] (fun _ -> go (Chapter en)))
    Doc.Concat [
        h1 [] [text "Home"]
        Doc.Concat chapterLinks
        Doc.Link "Go to About" [] (fun _ -> go About)
    ]

[<JavaScript>]
let AboutPage props =
    let go, _ = props
    Doc.Concat [
        h1 [] [text "About"]
        p [] [text "This is the about page" ]
        Doc.Link "Go to Home" [] (fun _ -> go Home)
    ]

[<JavaScript>]
let ChapterPage props num =
    let go, quranData = props
    Doc.Concat [
        h1 [] [text "Chapter"]
        p [] [text num]
        Doc.Link "Go to Home" [] (fun _ -> go Home)
        Doc.Link "Go to About" [] (fun _ -> go About)
    ]