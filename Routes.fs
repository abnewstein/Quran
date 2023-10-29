module Routes

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.JavaScript

[<JavaScript>]
type EndPoint = 
    | Home
    | Chapter of string
    | About

[<JavaScript>]
module RouteMap =
    let value =
        RouteMap.Create
        <| function
            | Home -> []
            | Chapter num -> ["";"chapter"; num]
            | About -> ["";"about"]
        <| function
            | [] -> Home
            | ["";"chapter"; num] -> Chapter num
            | ["";"about"] -> About
            | _ -> failwith "404"