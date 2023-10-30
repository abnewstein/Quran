namespace QuranWeb

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open QuranLib

module Server =

    let quranData = FileParser.AvailableQuranData()

    [<Rpc>]
    let GetQuranDataAsync () =
        async {
            return quranData
        }

[<JavaScript>]
module Client =
    open Routes
    open Pages

    [<SPAEntryPoint>]
    let Main () =

        let QuranData = View.ConstAsync(Server.GetQuranDataAsync())

        let router = RouteMap.Install RouteMap.value

        let renderMain endPointVar =
            endPointVar
            |> View.FromVar
            |> View.Map (fun endPoint ->
                let navigate = Var.Set endPointVar

                let props = navigate, QuranData
                match endPoint with
                | Home -> HomePage props
                | Chapter num -> ChapterPage props num
                | About -> AboutPage props
            )
            |> Doc.EmbedView

        
        renderMain router
        |> Doc.RunById "main"
