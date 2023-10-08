module Tests

open System
open Xunit
open Quran

[<Fact>]
let ``True Story`` () = Assert.True(true)

[<Fact>]
let ``Quran has 114 chapters`` () =
    Assert.Equal(114, Constants.VERSE_COUNT_BY_CHAPTER.Length)

[<Fact>]
let ``Quran has 6236 verses`` () =
    Assert.Equal(6236, Constants.VERSE_COUNT_BY_CHAPTER |> Array.sum)
