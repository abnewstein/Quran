namespace QuranWeb

open WebSharper
open WebSharper.UI
open WebSharper.UI.Notation
open QuranLib
open QuranServer

[<JavaScript>]
module State =

    let QuranDataVar = View.ConstAsync(Server.GetQuranDataAsync())
    let RouterVar: Var<EndPoint> = RouteMap.Install Routes.Value

    let SetRouterVar (newUrl: EndPoint) =
        RouterVar.Set newUrl
