namespace Quran

open FSharpPlus
open Constants
open Utilities.TextSearch
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

    let Of (chapterNumber: int, verseNumber: int) : VerseRef option =
        let verseRef =
            { ChapterNumber = chapterNumber
              VerseNumber = verseNumber }

        if isValid verseRef then Some(verseRef) else None

    let fromString (s: string) : VerseRef option = Option.bind Of (parseTuple2 s)

module NoteRef =
    let isValid noteRef =
        IsValidNoteNumber noteRef.VerseRef.ChapterNumber noteRef.VerseRef.VerseNumber noteRef.NoteNumber

    let Of (chapterNumber:int, verseNumber: int, noteNumber: int) : NoteRef option =
        VerseRef.Of (chapterNumber, verseNumber)
        |> function
            | Some verseRef ->
                let noteRef =
                    { VerseRef = verseRef
                      NoteNumber = noteNumber }

                if isValid noteRef then Some(noteRef) else None
            | None -> None

    let fromString (s: string) : NoteRef option =
        Option.bind Of (parseTuple3 s)

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

    /// <summary>Fetches a chapter by its number.</summary>
    let getChapter (quran: Quran) (chapterNumber: ChapterNumber) : Chapter =
        quran.Chapters[chapterNumber - 1]

    /// <summary>Fetches a verse given its reference.</summary>
    let getVerse (quran: Quran) (verseRef: VerseRef) : Verse =
        getChapter quran verseRef.ChapterNumber
        |> (fun c -> c.Verses.[verseRef.VerseNumber - 1])

    /// <summary>Fetches all verses for a given chapter number.</summary>
    let getChapterVerses (quran: Quran) (chapterNumber: ChapterNumber) : array<Verse> =
        getChapter quran chapterNumber
        |> (fun c -> c.Verses)

    /// <summary>Fetches a note given its reference.</summary>
    let getNote (quran: Quran) (noteRef: NoteRef) : Note =
        getVerse quran noteRef.VerseRef
        |> (fun v -> v.Notes.[noteRef.NoteNumber - 1])

    let filterVersesByTextWithScore (quran: Quran) (query: string) : array<Verse * float> =
        quran.Chapters
        |> Array.collect (fun c -> c.Verses)
        |> Array.map (fun v -> (v, calculateMatchingScore (query.ToLower()) (v.Text.ToLower())))
        |> Array.filter (snd >> ((<) 0.0))

    /// <summary>Finds and scores verses based on text matching.</summary>
    let filterVersesByText (quran: Quran) (text: string) : array<Verse * float> =
        filterVersesByTextWithScore quran text

    /// <summary>Fetches the count of chapters.</summary>
    let getChapterCount (quran: Quran) : int = Array.length quran.Chapters

    /// <summary>Fetches the total count of verses.</summary>
    let getVerseCount (quran: Quran) : int =
        quran.Chapters |> Array.sumBy (fun c -> Array.length c.Verses)
