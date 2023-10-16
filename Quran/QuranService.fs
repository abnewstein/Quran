namespace Quran

open WebSharper

module Service = 
    open FileParser

    [<Rpc>]
    let getAvailableQuranData () : array<Quran> =
        getAvailableTranslations () |> Set.map constructQuranFromJson |> Set.toArray