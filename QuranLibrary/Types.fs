namespace QuranLibrary

open WebSharper
open Constants
open TextSearch
open Functions

[<JavaScript>]
type ChapterNumber = int

[<JavaScript>]
type VerseNumber = int

[<JavaScript>]
type NoteNumber = int

[<JavaScript>]
type VerseRef =
    { ChapterNumber: ChapterNumber
      VerseNumber: VerseNumber }

[<JavaScript>]
type NoteRef =
    { VerseRef: VerseRef
      NoteNumber: NoteNumber }

[<JavaScript>]
type Note = { Ref: NoteRef; Text: string }

[<JavaScript>]
type Verse =
    { Ref: VerseRef
      Text: string
      Notes: Note array }

[<JavaScript>]
type Chapter =
    { Number: ChapterNumber
      Name: string
      Verses: Verse array }

[<JavaScript>]
type Author =
    | Author of string

    override this.ToString() =
        match this with
        | Author name -> name

[<JavaScript>]
type Language =
    | Language of string

    override this.ToString() =
        match this with
        | Language name -> name

[<JavaScript>]
type Translation =
    { Author: Author
      Language: Language }

    override this.ToString() = $"{this.Language}_{this.Author}"

[<JavaScript>]
type Quran =
    { Translation: Translation
      Chapters: Chapter array }

[<JavaScript>]
module VerseRef =
    let IsValid verseRef =
        IsValidVerseNumber verseRef.ChapterNumber verseRef.VerseNumber

    let Of (chapterNumber: int, verseNumber: int) : VerseRef option =
        let verseRef =
            { ChapterNumber = chapterNumber
              VerseNumber = verseNumber }

        if IsValid verseRef then Some(verseRef) else None

    let FromString (s: string) : VerseRef option = Option.bind Of (parseTuple2 s)

[<JavaScript>]
module NoteRef =
    let IsValid noteRef =
        IsValidNoteNumber noteRef.VerseRef.ChapterNumber noteRef.VerseRef.VerseNumber noteRef.NoteNumber

    let Of (chapterNumber: int, verseNumber: int, noteNumber: int) : NoteRef option =
        VerseRef.Of(chapterNumber, verseNumber)
        |> function
            | Some verseRef ->
                let noteRef =
                    { VerseRef = verseRef
                      NoteNumber = noteNumber }

                if IsValid noteRef then Some(noteRef) else None
            | None -> None

    let FromString (s: string) : NoteRef option = Option.bind Of (parseTuple3 s)

[<JavaScript>]
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

[<JavaScript>]
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

[<JavaScript>]
module Translation =
    let Of (author: Author) (language: Language) : Translation =
        { Author = author; Language = language }

[<JavaScript>]
module Quran =

    [<JavaScript>]
    let Of (translation: Translation) (chapters: Chapter array) : Quran =
        { Translation = translation
          Chapters = chapters }

    [<JavaScript>]
    let GetChapter (quran: Quran) (chapterNumber: ChapterNumber) : Chapter = quran.Chapters[chapterNumber - 1]

    [<JavaScript>]
    let GetChapterNames (quran: Quran) : array<string> = quran.Chapters |> Array.map (fun c -> c.Name)

    [<JavaScript>]
    let GetVerse (quran: Quran) (verseRef: VerseRef) : Verse =
        GetChapter quran verseRef.ChapterNumber
        |> (fun c -> c.Verses.[verseRef.VerseNumber - 1])

    [<JavaScript>]
    let GetVersesByChapter (quran: Quran) (chapterNumber: ChapterNumber) : array<Verse> =
        GetChapter quran chapterNumber |> (fun c -> c.Verses)

    [<JavaScript>]
    let GetNotes (quran: Quran) (noteRef: NoteRef) : array<Note> =
        GetVerse quran noteRef.VerseRef |> (fun v -> v.Notes)
    
    [<JavaScript>]
    let FilterVersesByTextWithScore (quran: Quran) (query: string) : array<Verse * float> =
        quran.Chapters
        |> Array.collect (fun c -> c.Verses)
        |> Array.map (fun v -> (v, calculateMatchingScore (query.ToLower()) (v.Text.ToLower())))
        |> Array.filter (snd >> ((<) 0.0))

    [<JavaScript>]
    let FilterVersesByText (quran: Quran) (text: string) : array<Verse * float> = FilterVersesByTextWithScore quran text

    [<JavaScript>]
    let GetChapterCount (quran: Quran) : int = Array.length quran.Chapters

    [<JavaScript>]
    let GetVerseCount (quran: Quran) : int =
        quran.Chapters |> Array.sumBy (fun c -> Array.length c.Verses)
