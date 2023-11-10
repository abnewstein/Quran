namespace QuranClient

open WebSharper
open WebSharper.UI
open WebSharper.UI.Notation
open QuranLibrary
open QuranServer

[<JavaScript>]
module State =
    let QuranData: View<array<Quran>> = View.ConstAsync(ServerFunctions.GetQuranDataAsync())
    let RouterVar: Var<EndPoint> = RouteMap.Install Routes.Value

    let SetRouterVar (newUrl: EndPoint) =
        RouterVar.Set newUrl
