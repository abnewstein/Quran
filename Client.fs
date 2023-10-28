namespace QuranWeb

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript
open QuranLib

module Server =
    [<Rpc>]
    let GetQuranData () =
        async {
            let data = Service.getAvailableQuranData()
            return data
        }

[<JavaScript>]
module Client =
    let quranData = Var.Create [||]

    let RunOnPageLoad () =
        async {
            let! data = Server.GetQuranData()
            quranData := data
        } |> Async.StartImmediate

    type EndPoint = Home | About

    let HomePage go =
        Doc.Concat [
            h1 [] [text "Home"]
            Doc.Link "Go to About" [] (fun _ -> go About)
        ]
    
    let AboutPage go =
        Doc.Concat [
            h1 [] [text "About"]
            p [] [text "This is the about page" ]
            Doc.Link "Go to Home" [] (fun _ -> go Home)
        ]
    
    let routeMap =
        RouteMap.Create
        <| function
            | Home -> []
            | About -> ["about"]
        <| function
            | [] -> Home
            | ["about"] -> About
            | _ -> failwith "404"

    [<SPAEntryPoint>]
    let Main =
        RunOnPageLoad()
        Console.Log(quranData)

        let router = RouteMap.Install routeMap
        let renderMain v =
            View.FromVar v
            |> View.Map (fun pty ->
                let go = Var.Set v
                match pty with
                | Home -> HomePage go
                | About -> AboutPage go
            )
            |> Doc.EmbedView

        Doc.RunById "main" (renderMain router)
