module Tests

open Xunit
open Quran

let assertContainsTranslation (expectedTranslation: Translation) (qurans: Quran list) =
    Assert.True(List.exists (fun q -> q.Translation = expectedTranslation) qurans)

let assertNotContainsTranslation (expectedTranslation: Translation) (qurans: Quran list) =
    Assert.False(List.exists (fun q -> q.Translation = expectedTranslation) qurans)

let assertChapterCount (expectedCount: int) (qurans: Quran list) =
    qurans |> List.iter (fun q -> Assert.Equal(expectedCount, q.Chapters.Length))


[<Fact>]
let ``True Story`` () = Assert.True(true)

[<Fact>]
let ``Quran has 114 chapters`` () =
    Assert.Equal(114, Constants.VERSE_COUNT_BY_CHAPTER.Length)

[<Fact>]
let ``Quran has 6236 verses`` () =
    Assert.Equal(6236, Constants.VERSE_COUNT_BY_CHAPTER |> Array.sum)

[<Fact>]
let ``Should retrieve all available translations`` () =
    let availableQuranData = FileParser.getAvailableQuranData ()
    let expectedTranslation1 = Translation.Of (Author "original") (Language "ar")
    let expectedTranslation2 = Translation.Of (Author "sam-gerrans") (Language "en")

    assertContainsTranslation expectedTranslation1 availableQuranData
    assertContainsTranslation expectedTranslation2 availableQuranData

[<Fact>]
let ``Should not retrieve unavailable translations`` () =
    let availableQuranData = FileParser.getAvailableQuranData ()

    let expectedTranslation = Translation.Of (Author "transliteration") (Language "en")

    assertNotContainsTranslation expectedTranslation availableQuranData

[<Fact>]
let ``All Quran translations should have 114 chapters`` () =
    let availableQuranData = FileParser.getAvailableQuranData ()
    assertChapterCount Constants.ChapterCount availableQuranData

[<Fact>]
let ``Each Quran should have the expected number of verses per chapter`` () =
    let availableQuranData = FileParser.getAvailableQuranData ()

    availableQuranData
    |> List.iter (fun quran ->
        quran.Chapters
        |> Array.iteri (fun i chapter ->
            let expectedVerseCount = Constants.VerseCountBy(i + 1)
            Assert.Equal(expectedVerseCount, chapter.Verses.Length)))

[<Fact>]
let ``Each Quran should have the expected total number of verses`` () =
    let availableQuranData = FileParser.getAvailableQuranData ()

    availableQuranData
    |> List.iter (fun quran ->
        let totalVerseCount = quran.Chapters |> Array.sumBy (fun ch -> ch.Verses.Length)
        Assert.Equal(Constants.VerseCount, totalVerseCount))
