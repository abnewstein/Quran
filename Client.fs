namespace QuranWeb

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open QuranLib
open QuranService

module Server =
    [<Rpc>]
    let GetQuranData () =
        async {
            let data = AvailableQuranData()
            return data
        }

[<JavaScript>]
module Client =
    open Routes
    open Pages
    let quranDataVar = Var.Create [||]

    let RunOnPageLoad () =
        async {
            let! data = Server.GetQuranData()
            quranDataVar := data
        } |> Async.StartImmediate

    [<SPAEntryPoint>]
    let Main =
        RunOnPageLoad()
        Console.Log(quranDataVar)

        let router = RouteMap.Install RouteMap.value
        let renderMain v =
            View.FromVar v
            |> View.Map (fun endPoint ->
                let go = Var.Set v
                let quranData: array<Quran> = Var.Get quranDataVar
                let props = go, quranData
                match endPoint with
                | Home -> HomePage props
                | Chapter num -> ChapterPage props num
                | About -> AboutPage props
            )
            |> Doc.EmbedView

        Doc.RunById "main" (renderMain router)
