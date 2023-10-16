module Tests

open Xunit
open Quran

let assertContainsTranslation (expectedTranslation: Translation) (qurans: Quran array) =
    Assert.True(Array.exists (fun q -> q.Translation = expectedTranslation) qurans)

let assertNotContainsTranslation (expectedTranslation: Translation) (qurans: Quran array) =
    Assert.False(Array.exists (fun q -> q.Translation = expectedTranslation) qurans)

let assertChapterCount (expectedCount: int) (qurans: Quran array) =
    qurans |> Array.iter (fun q -> Assert.Equal(expectedCount, q.Chapters.Length))

[<Theory>]
[<InlineData(114)>]
let ``Quran has expected chapters`` (expectedChapters: int) =
    Assert.Equal(expectedChapters, Constants.VERSE_COUNT_BY_CHAPTER.Length)

[<Theory>]
[<InlineData(6236)>]
let ``Quran has expected verses`` (expectedVerses: int) =
    Assert.Equal(expectedVerses, Constants.VERSE_COUNT_BY_CHAPTER |> Array.sum)

[<Theory>]
[<InlineData("original", "ar")>]
[<InlineData("sam-gerrans", "en")>]
let ``Should retrieve all available translations`` (author: string, language: string) =
    let availableQuranData = Service.getAvailableQuranData ()
    let expectedTranslation = Translation.Of (Author author) (Language language)

    assertContainsTranslation expectedTranslation availableQuranData

[<Theory>]
[<InlineData("transliteration", "en")>]
let ``Should not retrieve unavailable translations`` (author: string, language: string) =
    let availableQuranData = Service.getAvailableQuranData ()
    let expectedTranslation = Translation.Of (Author author) (Language language)

    assertNotContainsTranslation expectedTranslation availableQuranData

[<Fact>]
let ``All Quran translations should have 114 chapters`` () =
    let availableQuranData = Service.getAvailableQuranData ()
    assertChapterCount Constants.ChapterCount availableQuranData

[<Fact>]
let ``Each Quran object should have the expected number of verses per chapter`` () =
    let availableQuranData = Service.getAvailableQuranData ()

    availableQuranData
    |> Array.iter (fun quran ->
        quran.Chapters
        |> Array.iteri (fun i chapter ->
            let expectedVerseCount = Constants.VerseCountBy(i + 1)
            Assert.Equal(expectedVerseCount, chapter.Verses.Length)))

[<Fact>]
let ``Each Quran object should have the expected total number of verses`` () =
    let availableQuranData = Service.getAvailableQuranData ()

    availableQuranData
    |> Array.iter (fun quran ->
        let totalVerseCount = quran.Chapters |> Array.sumBy (fun ch -> ch.Verses.Length)
        Assert.Equal(Constants.VerseCount, totalVerseCount))
