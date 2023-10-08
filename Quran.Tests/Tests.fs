module Tests

open System
open Xunit
open Quran.Constants

[<Theory>]
[<InlineData(1, 1, 7)>]
[<InlineData(2, 1, 286)>]
[<InlineData(3, 1, 200)>]
let ``Verse count for chapter should be correct`` (chapter: int, _, expected: int) =
    let actual = ChapterVerseCountArray.[chapter - 1]
    Assert.Equal(expected, actual)

[<Fact>]
let ``Chapter count should be 114`` () =
    let actual = ChapterVerseCountArray.Length
    let expected = 114
    Assert.Equal(expected, actual)

[<Fact>]
let ``Verse count for chapter 1 should be 7`` () =
    let actual = ChapterVerseCountArray.[0]
    let expected = 7
    Assert.Equal(expected, actual)
