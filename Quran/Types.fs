namespace Quran

open System
open Constants
open Utilities.Functions

type ChapterNumber = int
type VerseNumber = int
type NoteNumber = int

type VerseRef =
    { ChapterNumber: ChapterNumber
      VerseNumber: VerseNumber }

    static member (==)(a: VerseRef, b: VerseRef) =
        a.ChapterNumber = b.ChapterNumber && a.VerseNumber = b.VerseNumber

type NoteRef =
    { VerseRef: VerseRef
      NoteNumber: NoteNumber }

    static member (==)(a: NoteRef, b: NoteRef) =
        a.VerseRef == b.VerseRef && a.NoteNumber = b.NoteNumber

module VerseRef =
    let private create (chapterNumber, verseNumber) =
        { ChapterNumber = chapterNumber
          VerseNumber = verseNumber }

    let fromString (s: string) : VerseRef option = parseTuple2 s |> Option.map create

    let Of (chapterNumber: ChapterNumber) (verseNumber: VerseNumber) : VerseRef option =
        createIfValid2 IsValidVerseNumber create (chapterNumber, verseNumber)

module NoteRef =
    let private create (chapterNumber, verseNumber, noteNumber) =
        { VerseRef =
            { ChapterNumber = chapterNumber
              VerseNumber = verseNumber }
          NoteNumber = noteNumber }

    let fromString (s: string) : NoteRef option = parseTuple3 s |> Option.map create

    let Of (chapterNumber: ChapterNumber) (verseNumber: VerseNumber) (noteNumber: NoteNumber) : NoteRef option =
        createIfValid3 IsValidNoteNumber create (chapterNumber, verseNumber, noteNumber)

type Note = { Ref: NoteRef; Text: string }

type Verse =
    { Ref: VerseRef
      Text: string
      Notes: Note array }

module Verse =
    let private create (ref, text, notes) : Verse =
        { Ref = ref
          Text = text
          Notes = notes }

    let Of (ref: VerseRef) (text: string) (notes: Note array) : Verse option =
        IsValidVerseNumber ref.ChapterNumber ref.VerseNumber
        |> function
            | true -> Some(create (ref, text, notes))
            | false -> None

type Chapter =
    { Number: ChapterNumber
      Name: string
      Verses: Verse array }

module Chapter =
    let private create (number, name, verses) =
        { Number = number
          Name = name
          Verses = verses }

    let Of (number: ChapterNumber) (name: string) (verses: Verse array) : Chapter option =
        IsValidChapterNumber number
        |> function
            | true -> Some(create (number, name, verses))
            | false -> None
