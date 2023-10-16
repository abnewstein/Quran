module Tests2

open Xunit
open Quran

let quranData: Quran array = Service.getAvailableQuranData ()

[<Fact>]
let ``Test - filterVersesByTextWithScore should return verses with non-zero scores`` () =
    let query = "Not Equal"
    let result = Quran.filterVersesByTextWithScore quranData[1] query

    printfn "%A" result

    Assert.NotEmpty(result)
    Assert.All(result, (fun (_, score) -> Assert.True(score > 0.0)))

[<Fact>]
let ``Test - filterVersesByTextWithScore should return no verses for non-matching query`` () =
    let query = "nonexistentword"
    let result = Quran.filterVersesByTextWithScore quranData[1] query

    Assert.Empty(result)
