namespace FSharpTestApi

type ImageMessage = {
    Content: string
    Url: string
}

type TextMessage = {
    Content: string   
}


// Discriminated union for domain model
type Message =
    | ImageMessage of ImageMessage
    | TextMessage of TextMessage