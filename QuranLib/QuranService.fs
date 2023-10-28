namespace QuranLib

open WebSharper

module Service = 
    open FileParser

    let getAvailableQuranData () : array<Quran> =
        getAvailableTranslations () |> Set.map constructQuranFromJson |> Set.toArray
        