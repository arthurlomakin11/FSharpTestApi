namespace FSharpTestApi.Application

open System
open FSharpTestApi

module MessageValidation =

    // Active pattern
    let (|Text|ImageWithUrl|ImageWithoutUrl|) (model: UnvalidatedMessageModel) =
        match model.Type, model.Url with
        | MessageType.Text, _ -> Text
        | MessageType.Image, Some url when not (String.IsNullOrWhiteSpace url) -> ImageWithUrl url
        | MessageType.Image, _ -> ImageWithoutUrl
        | _ -> ArgumentOutOfRangeException() |> raise

    let unvalidatedMessageModelToValidated
        (model: UnvalidatedMessageModel)
        : Result<MessageModel, ModelValidationError> =

        match model with
        | Text ->
            Ok
                { Type = MessageType.Text
                  Content = model.Content
                  Url = None }

        | ImageWithUrl url ->
            Ok
                { Type = MessageType.Image
                  Content = model.Content
                  Url = Some url }

        | ImageWithoutUrl -> Error ModelValidationError.UrlIsRequired
