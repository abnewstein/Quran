namespace QuranWeb

open WebSharper
open WebSharper.UI
open WebSharper.UI.Notation
open QuranLib
open QuranServer

[<JavaScript>]
module State =

    let quranDataVar = View.ConstAsync(Server.GetQuranDataAsync())
    let routerVar: Var<EndPoint> = RouteMap.Install Routes.value

    let SetClientUrlVar (newUrl: EndPoint) =
        routerVar.Set newUrl