namespace QuranServer

open QuranLib
open WebSharper

module Server =

    let quranData = FileParser.AvailableQuranData()

    [<Rpc>]
    let GetQuranDataAsync () =
        async {
            return quranData
        }