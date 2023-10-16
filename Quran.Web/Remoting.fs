namespace Quran.Web

open WebSharper
open Quran

module Server =

    [<Rpc>]
    let getQuranData () =
        let quran = Service.getAvailableQuranData()
        async {
            return quran
        }
