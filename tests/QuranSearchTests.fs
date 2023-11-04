module QuranSearchTests

open Xunit
open QuranLib
open Quran

let QuranData: array<Quran> = Service.AvailableQuranData ()

[<Fact>]
let ``Searching verses by text yields non-zero scores for relevant matches`` () =
    let query = "Not Equal"
    let result = Quran.filterVersesByTextWithScore QuranData[1] query

    Assert.NotEmpty(result)
    Assert.All(result, (fun (_, score) -> Assert.True(score > 0.0)))

[<Fact>]
let ``Searching verses by text returns no results for queries with no matches`` () =
    let query = "nonexistentword"
    let result = Quran.filterVersesByTextWithScore QuranData[1] query

    Assert.Empty(result)
