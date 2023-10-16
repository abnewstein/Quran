namespace Quran.Web

open WebSharper
open WebSharper.UI
open WebSharper.UI.Templating
open WebSharper.UI.Notation
open Quran

[<JavaScript>]
module Templates =

    type MainTemplate = Templating.Template<"Main.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>

[<JavaScript>]
module Client =

    let Main () =
        let quran: Var<array<Quran>> = Var.Create [||]

        printfn "%A" quran

        Templates.MainTemplate.MainForm()
            .OnSend(fun e ->
                async {
                    let! res = Server.getQuranData ()
                    printfn "%A" res
                    quran := res
                }
                |> Async.StartImmediate
            )            
            .Doc()
