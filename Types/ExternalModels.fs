namespace FSharpTestApi

open System.Text.Json.Serialization

[<JsonConverter(typeof<JsonStringEnumConverter>)>]
type MessageType =
    | Image = 0
    | Text = 1

type MessageModel = {
    Type: MessageType
    Content: string
    Url: string option
}

type UnvalidatedMessageModel = {
    Type: MessageType
    Content: string
    Url: string option
}