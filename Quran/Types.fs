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

type NoteRef =
    { VerseRef: VerseRef
      NoteNumber: NoteNumber }

type Note = { Ref: NoteRef; Text: string }

type Verse =
    { Ref: VerseRef
      Text: string
      Notes: Note array }

type Chapter =
    { Number: ChapterNumber
      Name: string
      Verses: Verse array }

type Author =
    | Author of string

    override this.ToString() =
        match this with
        | Author name -> name

type Language =
    | Language of string

    override this.ToString() =
        match this with
        | Language name -> name

type Translation =
    { Author: Author
      Language: Language }

    override this.ToString() = $"{this.Language}_{this.Author}"

type Quran =
    { Translation: Translation
      Chapters: Chapter array }

module VerseRef =
    let private create (chapterNumber, verseNumber) : VerseRef =
        { ChapterNumber = chapterNumber
          VerseNumber = verseNumber }

    let isValid verseRef =
        IsValidVerseNumber verseRef.ChapterNumber verseRef.VerseNumber

    let Of (chapterNumber: ChapterNumber) (verseNumber: VerseNumber) : VerseRef option =
        let verseRef = create (chapterNumber, verseNumber)
        if isValid verseRef then Some(verseRef) else None

    let fromString (s: string) : VerseRef option = parseTuple2 s >>= (uncurry Of)

module NoteRef =
    let private create (verseRef: VerseRef, noteNumber) =
        { VerseRef = verseRef
          NoteNumber = noteNumber }

    let isValid noteRef =
        IsValidNoteNumber noteRef.VerseRef.ChapterNumber noteRef.VerseRef.VerseNumber noteRef.NoteNumber

    let Of (verseRef: VerseRef) (noteNumber: NoteNumber) : NoteRef option =
        create (verseRef, noteNumber)
        |> function
            | noteRef when isValid noteRef -> Some(noteRef)
            | _ -> None

    let fromString (s: string) : NoteRef option =
        parseTuple3 s
        >>= (fun (chapterNumber, verseNumber, noteNumber) ->
            VerseRef.Of chapterNumber verseNumber
            >>= (fun verseRef -> Of verseRef noteNumber))

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

module Translation =
    let private create (author, language) =
        { Author = author; Language = language }

    let Of (author: Author) (language: Language) : Translation = create (author, language)

module Quran =
    let private create (translation, chapters) =
        { Translation = translation
          Chapters = chapters }

    let Of (translation: Translation) (chapters: Chapter array) : Quran = create (translation, chapters)
