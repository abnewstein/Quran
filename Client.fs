namespace QuranWeb

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
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
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let People =
        ListModel.FromSeq [
            "John"
            "Paul"
        ]

    let quranData = Var.Create [||]

    let RunOnPageLoad () =
        async {
            let! data = Server.GetQuranData()
            quranData.Value <- data
        } |> Async.StartImmediate

    [<SPAEntryPoint>]
    let Main () =
        RunOnPageLoad()
        let newName = Var.Create ""
        Console.Log(quranData)

        IndexTemplate.Main()
            .ListContainer(
                People.View.DocSeqCached(fun (name: string) ->
                    IndexTemplate.ListItem().Name(name).Doc()
                )
            )
            .Name(newName)
            .Add(fun _ ->
                People.Add(newName.Value)
                newName.Value <- ""
            )
            .Doc()
        |> Doc.RunById "main"
