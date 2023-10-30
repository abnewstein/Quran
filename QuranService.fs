namespace QuranLib

open WebSharper
open FileParser

module QuranService = 

    let AvailableQuranData () : array<Quran> =
        getAvailableTranslations () |> Set.map constructQuranFromJson |> Set.toArray
    
    