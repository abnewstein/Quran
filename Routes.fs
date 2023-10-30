namespace Routes

open WebSharper
open WebSharper.UI

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