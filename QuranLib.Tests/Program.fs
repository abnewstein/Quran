module Program =
    open QuranLib
    let quranData: array<Quran> = Service.getAvailableQuranData ()
    
    ()