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
    let isValid verseRef =
        IsValidVerseNumber verseRef.ChapterNumber verseRef.VerseNumber

    let Of (chapterNumber: ChapterNumber) (verseNumber: VerseNumber) : VerseRef option =
        let verseRef =
            { ChapterNumber = chapterNumber
              VerseNumber = verseNumber }

        if isValid verseRef then Some(verseRef) else None

    let fromString (s: string) : VerseRef option = parseTuple2 s >>= (uncurry Of)

module NoteRef =
    let isValid noteRef =
        IsValidNoteNumber noteRef.VerseRef.ChapterNumber noteRef.VerseRef.VerseNumber noteRef.NoteNumber

    let Of (verseRef: VerseRef) (noteNumber: NoteNumber) : NoteRef option =
        { VerseRef = verseRef
          NoteNumber = noteNumber }
        |> function
            | noteRef when isValid noteRef -> Some(noteRef)
            | _ -> None

    let fromString (s: string) : NoteRef option =
        parseTuple3 s
        >>= (fun (chapterNumber, verseNumber, noteNumber) ->
            VerseRef.Of chapterNumber verseNumber
            >>= (fun verseRef -> Of verseRef noteNumber))

module Verse =
    let Of (ref: VerseRef) (text: string) (notes: Note array) : Verse option =
        IsValidVerseNumber ref.ChapterNumber ref.VerseNumber
        |> function
            | true ->
                { Ref = ref
                  Text = text
                  Notes = notes }
                |> Some
            | false -> None

module Chapter =
    let Of (number: ChapterNumber) (name: string) (verses: Verse array) : Chapter option =
        IsValidChapterNumber number
        |> function
            | true ->
                { Number = number
                  Name = name
                  Verses = verses }
                |> Some
            | false -> None

module Translation =
    let Of (author: Author) (language: Language) : Translation =
        { Author = author; Language = language }

module Quran =
    let Of (translation: Translation) (chapters: Chapter array) : Quran =
        { Translation = translation
          Chapters = chapters }
