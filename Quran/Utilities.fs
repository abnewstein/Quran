namespace Utilities

open System
open FSharpPlus


type Predicate<'T> = 'T -> bool
type Predicate2<'T1, 'T2> = 'T1 -> 'T2 -> bool
type Predicate3<'T1, 'T2, 'T3> = 'T1 -> 'T2 -> 'T3 -> bool

module Functions =

    let safeParseInt (str: string) =
        match Int32.TryParse(str) with
        | (true, value) -> Some value
        | _ -> None

    let parseTuple2 (str: string) : option<int * int> =
        match str.Split(':') with
        | [| a; b |] ->
            match safeParseInt a, safeParseInt b with
            | Some a, Some b -> Some(a, b)
            | _ -> None
        | _ -> None

    let parseTuple3 (str: string) =
        match str.Split(':') with
        | [| a; b; c |] ->
            match safeParseInt a, safeParseInt b, safeParseInt c with
            | Some a, Some b, Some c -> Some(a, b, c)
            | _ -> None
        | _ -> None

module TextSearch =

    /// <summary>Calculates the matching score for a text against a query.</summary>
    let calculateMatchingScore (query: string) (text: string) : float =
        let queryWords =
            query.Split([| ' '; ','; '.'; '?' |], System.StringSplitOptions.RemoveEmptyEntries)

        let textWords =
            text.Split([| ' '; ','; '.'; '?' |], System.StringSplitOptions.RemoveEmptyEntries)

        let matchingWords =
            queryWords
            |> Array.filter (fun q -> textWords |> Array.exists (fun t -> t.Contains q))

        float matchingWords.Length / float textWords.Length
