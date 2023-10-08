module FileParser

open System.Reflection
open Newtonsoft.Json
open System.IO

type ChaptersJson = int * string list
type VersesJson = int * int * string list
type NotesJson = string * string list

let loadEmbeddedResource<'T> (resourcePath: string) =
    let assembly = Assembly.GetExecutingAssembly()
    use stream = assembly.GetManifestResourceStream(resourcePath)
    use reader = new StreamReader(stream)
    let content = reader.ReadToEnd()
    JsonConvert.DeserializeObject<'T>(content)

let loadChaptersJson () =
    loadEmbeddedResource<ChaptersJson> ("Quran.Data.chapters.json")
