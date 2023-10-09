namespace Quran

open FSharpPlus
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

type Note = { Ref: NoteRef; Text: string }

type Verse =
    { Ref: VerseRef
      Text: string
      Notes: Note array }

type Chapter =
    { Number: ChapterNumber
      Name: string
      Verses: Verse array }

type Author = Author of string
type Language = Language of string

type Translation =
    { Author: Author
      Language: Language }

    static member (==)(a: Translation, b: Translation) =
        a.Author = b.Author && a.Language = b.Language

type Quran =
    { Translation: Translation
      Chapters: Chapter array }

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

module Note =
    let private create (ref, text) = { Ref = ref; Text = text }

    let Of (ref: NoteRef) (text: string) : Note option =
        IsValidNoteNumber ref.VerseRef.ChapterNumber ref.VerseRef.VerseNumber ref.NoteNumber
        |> function
            | true -> Some(create (ref, text))
            | false -> None

module Translation =
    let private create (author, language) =
        { Author = author; Language = language }

    let Of (author: Author) (language: Language) : Translation = create (author, language)

module Quran =
    let private create (translation, chapters) =
        { Translation = translation
          Chapters = chapters }

    let Of (translation: Translation) (chapters: Chapter array) : Quran = create (translation, chapters)
