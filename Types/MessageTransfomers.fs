namespace FSharpTestApi

type ModelValidationError =
    | UrlIsRequired

type UnvalidatedMessageModelToValidated =
    UnvalidatedMessageModel -> Result<MessageModel, ModelValidationError>

type MessageModelToDomain = MessageModel -> Message

type DomainToMessageModel = Message -> MessageModel

type MessagePipeline = UnvalidatedMessageModel -> Result<MessageModel, ModelValidationError>