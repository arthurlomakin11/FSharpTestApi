namespace FSharpTestApi.Application

open System
open FSharpTestApi

module MessageMapping =

    let messageModelToDomain (model: MessageModel) : Message =
        match model.Type, model.Url with
        | MessageType.Text, _ -> TextMessage { Content = model.Content }
        | MessageType.Image, Some url -> ImageMessage { Content = model.Content; Url = url }       
        | _ -> ArgumentOutOfRangeException() |> raise

    let domainToMessageModel (message: Message) : MessageModel =
        match message with
        | TextMessage message ->
            { Type = MessageType.Text
              Content = message.Content
              Url = None }

        | ImageMessage message ->
            { Type = MessageType.Image
              Content = message.Content
              Url = Some message.Url }