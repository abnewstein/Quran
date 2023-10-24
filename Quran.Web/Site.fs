namespace Quran.Web

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI
open WebSharper.UI.Server

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/about">] About

module Templating =
    open WebSharper.UI.Html

    let Main ctx action (title: string) (body: Doc list) =
        Content.Page(
            Templates.MainTemplate()
                .Title(title)
                .Body(body)
                .Doc()
        )

module Site =
    open WebSharper.UI.Html
    open type WebSharper.UI.ClientServer
    open Quran

    let quranData = Service.getAvailableQuranData ()
    let englishQuran = quranData |> Array.find (fun q -> q.Translation.Language = Language "en")

    let firstVerse = englishQuran.Chapters.[0].Verses.[0]

    let HomePage ctx =
        Templating.Main ctx EndPoint.Home "Home" [
            h1 [] [text firstVerse.Text]
            div [] [client (Client.Main())]
        ]

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

