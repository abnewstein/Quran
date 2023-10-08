namespace Quran

open System

module Constants =
    let ChapterVerseCountArray =
        [| 7
           286
           200
           176
           120
           165
           206
           75
           129
           109
           123
           111
           43
           52
           99
           128
           111
           110
           98
           135
           112
           78
           118
           64
           77
           227
           93
           88
           69
           60
           34
           30
           73
           54
           45
           83
           182
           88
           75
           85
           54
           53
           89
           59
           37
           35
           38
           29
           18
           45
           60
           49
           62
           55
           78
           96
           29
           22
           24
           13
           14
           11
           11
           18
           12
           12
           30
           52
           52
           44
           28
           28
           20
           56
           40
           31
           50
           40
           46
           42
           29
           19
           36
           25
           22
           17
           19
           26
           30
           20
           15
           21
           11
           8
           8
           19
           5
           8
           8
           11
           11
           8
           3
           9
           5
           4
           7
           3
           6
           3
           5
           4
           5
           6 |]

    let VerseCountBy chapterNumber : int =
        ChapterVerseCountArray.[chapterNumber - 1]

    let VerseCount: int = ChapterVerseCountArray |> Array.sum

    let ChapterCount: int = ChapterVerseCountArray |> Array.length

    let isValidChapterNumber (chapterNumber: int) =
        chapterNumber > 0 && chapterNumber <= ChapterCount

    let isValidVerseNumber (chapterNumber: int) (verseNumber: int) =
        isValidChapterNumber chapterNumber
        && verseNumber > 0
        && verseNumber <= VerseCountBy chapterNumber

    let isValidVerseRef (verseRefString: string) =
        match verseRefString.Split(':') with
        | [| chapterNumberString; verseNumberString |] ->
            let chapterNumber = Int32.Parse(chapterNumberString)
            let verseNumber = Int32.Parse(verseNumberString)
            isValidVerseNumber chapterNumber verseNumber
        | _ -> false
