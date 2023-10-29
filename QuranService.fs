namespace QuranLib

open WebSharper
open FileParser

module QuranService = 

    [<JavaScript>]
    let AvailableQuranData () : array<Quran> =
        getAvailableTranslations () |> Set.map constructQuranFromJson |> Set.toArray
    
    [<JavaScript>]
    let PrimaryQuran (quranArr : array<Quran>) =
        quranArr |> Array.filter (fun q -> q.Translation.Language = Language "ar")
        |> Array.head
    
    [<JavaScript>]
    let SecondaryQuran (quran : array<Quran>) =
        quran |> Array.filter (fun q -> q.Translation = Translation.Of (Author "sam-gerrans") (Language "en"))
        |> Array.head
    
    [<JavaScript>]
    let getChapter (quran : Quran) (chapterNumber: ChapterNumber) : Chapter =
        Quran.getChapter quran chapterNumber
    
    [<JavaScript>]
    let getVersesByChapter (quran : Quran) (chapterNumber: ChapterNumber) : array<Verse> =
        Quran.getVersesByChapter quran chapterNumber
    