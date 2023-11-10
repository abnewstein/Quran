namespace QuranClient

open WebSharper
open QuranLibrary

[<JavaScript>]
module Utilities =

    module QuranArray =
        let PrimaryQuran (q: array<Quran>) =
            q
            |> Array.tryFind (fun q -> q.Translation.Language = Language "ar")
            |> Option.get
        let SecondaryQuran (q: array<Quran>) =
            q
            |> Array.tryFind (fun q -> q.Translation.Author = Author "sam-gerrans")
            |> Option.get