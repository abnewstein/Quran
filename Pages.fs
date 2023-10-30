namespace QuranWeb

module Pages =

    open WebSharper
    open WebSharper.UI
    open WebSharper.UI.Html
    open WebSharper.UI.Client
    open WebSharper.UI.Notation
    open WebSharper.JavaScript
    open Routes
    open QuranLib
    open Components
    open QuranOps

    [<JavaScript>]
    type Props = (EndPoint -> unit) * View<array<Quran>>

    [<JavaScript>]
    let HomePage (props: Props) =
        let go, quranData = props
        let chapterList = 
            quranData 
            |> View.Map ChapterListDoc
            |> Doc.EmbedView

        Doc.Concat [
            h1 [] [text "Home"]
            Doc.Link "Go to About" [] (fun _ -> go About)
            chapterList
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