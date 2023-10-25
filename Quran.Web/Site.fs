namespace Quran.Web

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI
open WebSharper.UI.Server

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/about">] About

module Templating =
    open Quran
    open WebSharper.UI.Html

    let quranData: Var<array<Quran>> = Var.Create [||]
    let RunOnPageLoad () =
        async {
            let! response = Server.getQuranData()
            quranData.Set response
            printfn "Quran data loaded"
        }
        |> Async.Start

    let fetchEnglishQuran = Array.find (fun q -> q.Translation.Language = Language "en")

    let Main ctx action (title: string) (body: Doc list) =
        RunOnPageLoad()

        Content.Page(
            Templates.MainTemplate()
                .Title(title)
                .Body(body)
                .Doc()
        )

module Site =
    open WebSharper.UI.Html
    open type WebSharper.UI.ClientServer


    let HomePage ctx =
        Templating.Main ctx EndPoint.Home "Home" []

    let AboutPage ctx =
        Templating.Main ctx EndPoint.About "About" [
            h1 [] [text "About"]
            p [] [text "This is a template WebSharper client-server application."]
        ]

    [<Website>]
    let Main =
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home -> HomePage ctx
            | EndPoint.About -> AboutPage ctx
        )

