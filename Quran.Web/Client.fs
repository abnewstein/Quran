namespace Quran.Web

open WebSharper
open WebSharper.UI
open WebSharper.UI.Templating
open WebSharper.UI.Notation
open Quran

[<JavaScript>]
module Templates =

    type MainTemplate = Templating.Template<"components/Main.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>

[<JavaScript>]
module Client =
    open WebSharper.UI.Html    
    
    let Main () =        
        Templates.MainTemplate()
            .Body([text "Hello World!"])
            .Doc()
