namespace QuranClient

open WebSharper
open WebSharper.UI
open WebSharper.UI.Notation
open QuranServer

[<JavaScript>]
module State =
    let QuranDataVar = View.ConstAsync(ServerFunctions.GetQuranDataAsync())
    let RouterVar: Var<EndPoint> = RouteMap.Install Routes.Value

    let SetRouterVar (newUrl: EndPoint) =
        RouterVar.Set newUrl
