namespace Quran

open System
open Constants

type ChapterNumber = int
type VerseNumber = int
type NoteNumber = int

type VerseRef =
    { ChapterNumber: ChapterNumber
      VerseNumber: VerseNumber }

    static member (==)(a: VerseRef, b: VerseRef) =
        a.ChapterNumber = b.ChapterNumber && a.VerseNumber = b.VerseNumber

    static member (~+)(a: VerseRef) =
        let chapterNumber = a.ChapterNumber
        let verseNumber = a.VerseNumber + 1

        if verseNumber > VERSE_COUNT_BY_CHAPTER.[chapterNumber - 1] then
            { ChapterNumber = chapterNumber + 1
              VerseNumber = 1 }
        else
            { ChapterNumber = chapterNumber
              VerseNumber = verseNumber }

type NoteRef =
    { ChapterNumber: ChapterNumber
      VerseNumber: VerseNumber
      NoteNumber: NoteNumber }

    static member (==)(a: NoteRef, b: NoteRef) =
        a.ChapterNumber = b.ChapterNumber
        && a.VerseNumber = b.VerseNumber
        && a.NoteNumber = b.NoteNumber

module VerseRef =
    let private create chapterNumber verseNumber =
        { ChapterNumber = chapterNumber
          VerseNumber = verseNumber }

    let fromString (s: string) : VerseRef option =
        TryParseVerseRef s
        |> Option.map (fun (chapterNumber, verseNumber) -> create chapterNumber verseNumber)

    let Of (chapterNumber: ChapterNumber) (verseNumber: VerseNumber) : VerseRef option =
        IsValidVerseNumber chapterNumber verseNumber
        |> function
            | true -> Some(create chapterNumber verseNumber)
            | false -> None

module NoteRef =
    let private create chapterNumber verseNumber noteNumber =
        { ChapterNumber = chapterNumber
          VerseNumber = verseNumber
          NoteNumber = noteNumber }

    let fromString (s: string) : NoteRef option =
        TryParseNoteRef s
        |> Option.map (fun (chapterNumber, verseNumber, noteNumber) -> create chapterNumber verseNumber noteNumber)

    let Of (chapterNumber: ChapterNumber) (verseNumber: VerseNumber) (noteNumber: NoteNumber) : NoteRef =
        create chapterNumber verseNumber noteNumber
