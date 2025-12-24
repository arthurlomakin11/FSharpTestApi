namespace FSharpTestApi

type ImageMessage = {
    Content: string
    Url: string
}

type TextMessage = {
    Content: string   
}

type Message =
    | ImageMessage of ImageMessage
    | TextMessage of TextMessage