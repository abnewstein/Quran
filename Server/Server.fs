namespace QuranServer

open QuranLibrary
open WebSharper

module ServerFunctions =

    let QuranData = FileParser.AvailableQuranData()

    [<Rpc>]
    let GetQuranDataAsync () =
        async {
            return QuranData
        }