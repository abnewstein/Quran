namespace QuranServer

open QuranLib
open WebSharper

module Server =

    let QuranData = FileParser.AvailableQuranData()

    [<Rpc>]
    let GetQuranDataAsync () =
        async {
            return QuranData
        }