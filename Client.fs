namespace QuranWeb

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open QuranLib
open QuranServer

[<JavaScript>]
module Client =
    open Pages

    [<SPAEntryPoint>]
    let Main () =

        let renderMain endPointVar =
            endPointVar
            |> View.FromVar
            |> View.Map (fun endPoint ->
                
                match endPoint with
                | Home -> HomePage
                | Chapter num -> ChapterPage num
                | About -> AboutPage
            )
            |> Doc.EmbedView

        renderMain State.RouterVar
        |> Doc.RunById "main"
