namespace FSharpTestApi.Application

open System
open FSharpTestApi

module MessageProcessing =

    let processMessagePipeline
        (unvalidatedMessageModelToValidated: UnvalidatedMessageModelToValidated)
        (messageModelToDomain: MessageModelToDomain)
        (domainToMessageModel: DomainToMessageModel)
        (model: UnvalidatedMessageModel)
        : Result<MessageModel, ModelValidationError> =

        model
        |> unvalidatedMessageModelToValidated
        |> Result.map messageModelToDomain
        |> Result.map domainToMessageModel
