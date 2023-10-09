namespace Utilities

open System

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

    let createIfValid (isValid: Predicate<'T>) create (input: 'T) =
        if isValid input then Some(create input) else None

    let createIfValid2 (isValid: Predicate2<'T1, 'T2>) create (input1, input2) =
        if isValid input1 input2 then
            Some(create (input1, input2))
        else
            None

    let createIfValid3 (isValid: Predicate3<'T1, 'T2, 'T3>) create (input1, input2, input3) =
        if isValid input1 input2 input3 then
            Some(create (input1, input2, input3))
        else
            None
