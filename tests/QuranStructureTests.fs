module QuranStructureTests

open Xunit
open QuranLib
open Quran

let QuranData: array<Quran> = Service.AvailableQuranData ()

let assertContainsTranslation expectedTranslation qurans =
    Assert.Contains(qurans, fun q -> q.Translation = expectedTranslation)

let assertNotContainsTranslation expectedTranslation qurans =
    Assert.DoesNotContain(qurans, fun q -> q.Translation = expectedTranslation)

let assertChapterCount expectedCount qurans =
    qurans |> Array.iter (fun q -> Assert.Equal(expectedCount, q.Chapters.Length))

[<Fact>]
let ``Quran consists of the correct number of chapters`` () =
    Assert.Equal(Constants.VERSE_COUNT_BY_CHAPTER.Length, Constants.TOTAL_CHAPTERS)

[<Fact>]
let ``Quran comprises the expected total number of verses`` () =
    Assert.Equal(Constants.VERSE_COUNT_BY_CHAPTER |> Array.sum, Constants.TOTAL_VERSES)

[<Theory>]
[<InlineData("original", "ar")>]
[<InlineData("sam-gerrans", "en")>]
let ``Available translations include expected authors and languages`` (author: string, language: string) =
    let expectedTranslation = Translation.Of (Author author) (Language language)
    assertContainsTranslation expectedTranslation QuranData

[<Theory>]
[<InlineData("transliteration", "en")>]
let ``Unavailable translations are excluded from the dataset`` (author: string, language: string) =
    let expectedTranslation = Translation.Of (Author author) (Language language)
    assertNotContainsTranslation expectedTranslation QuranData

[<Fact>]
let ``Each Quran translation contains the canonical number of chapters`` () =
    assertChapterCount Constants.TOTAL_CHAPTERS QuranData

[<Fact>]
let ``Each chapter in every Quran translation has the expected number of verses`` () =
    QuranData
    |> Array.iter (fun quran ->
        quran.Chapters
        |> Array.iteri (fun i chapter ->
            let expectedVerseCount = Constants.VERSE_COUNT_BY_CHAPTER.[i]
            Assert.Equal(expectedVerseCount, chapter.Verses.Length)))

[<Fact>]
let ``Total verse count in each Quran translation matches the canonical count`` () =
    QuranData
    |> Array.iter (fun quran ->
        let totalVerseCount = quran.Chapters |> Array.sumBy (fun ch -> ch.Verses.Length)
        Assert.Equal(Constants.TOTAL_VERSES, totalVerseCount))
