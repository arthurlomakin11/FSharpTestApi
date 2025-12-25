# FSharpTestApi

## Project Description

## Project Description

## Project Description

FSharpTestApi is a sample API written in F# demonstrating **functional domain modeling** inspired by Scott Wlaschin's *Domain Modeling in Functional Way*.

In addition, this project applies concepts learned from *Essential F#* by Ian Russell, including **Active Patterns** and **Partial Active Patterns**, to implement validation and error handling in a composable, type-safe manner.


The project showcases:

- **Clear separation of domain and infrastructure**
- Validation using **Active Patterns**
- Explicit error handling with **Result<'T, 'Error>**
- Function composition via **pipeline**
- Simple integration with ASP.NET Core and Swagger

---

## Domain Models

### Message

```
type ImageMessage = { Content: string; Url: string }
type TextMessage = { Content: string }
type Message =
| ImageMessage of ImageMessage
| TextMessage of TextMessage
```

- `Message` is a discriminated union representing all possible message types.
- `ImageMessage` and `TextMessage` are separate records for easier extension and manipulation.

---

### API Models

```
type MessageType = Image | Text

type MessageModel = { Type: MessageType; Content: string; Url: string option }
type UnvalidatedMessageModel = { Type: MessageType; Content: string; Url: string option }
```

- `MessageModel` — validated API model.
- `UnvalidatedMessageModel` — incoming request model before validation.

---

## Validation

Validation uses **Active Patterns**:

```
let (|Text|ImageWithUrl|ImageWithoutUrl|) (model: UnvalidatedMessageModel) = ...
```

- Returns `Result<MessageModel, ModelValidationError>`:
    - `Ok` — model is valid
    - `Error` — validation failed (e.g., `UrlIsRequired`)

---

## Mapping Between Layers

```
let messageModelToDomain (model: MessageModel) : Message = ...
let domainToMessageModel (message: Message) : MessageModel = ...
```

- Pure functions mapping API models to domain and back.

---

## Message Processing Pipeline

```
let processMessagePipeline
(unvalidatedMessageModelToValidated: UnvalidatedMessageModel -> Result<MessageModel, ModelValidationError>)
(messageModelToDomain: MessageModel -> Message)
(domainToMessageModel: Message -> MessageModel)
(model: UnvalidatedMessageModel)
: Result<MessageModel, ModelValidationError> =

    model
    |> unvalidatedMessageModelToValidated
    |> Result.map messageModelToDomain
    |> Result.map domainToMessageModel
```

- Combines **validation and mapping** in a single flow.
- Uses `Result.map` to transform data while propagating errors.

---

## Error Handling and ActionResult

```
type ValidationErrorDto = { Code: string; Message: string }

let toErrorDto (error: ModelValidationError) : ValidationErrorDto = ...
```

- In the controller:

```
model
|> pipeline
|> ResultToActionResult.toActionResult this
```

- `Ok` returns `ActionResult<MessageModel>`
- `Error` returns `BadRequest` with the DTO

---

## Controller

```
[<ApiController>]
[<Route("[controller]")>]
type MessageController(pipeline: MessagePipeline) =
inherit ControllerBase()

    [<HttpPost>]
    member this.Post([<FromBody>] model: UnvalidatedMessageModel) =
        model |> pipeline |> ResultToActionResult.toActionResult this
```

- Controller is clean and **does not contain domain logic**.

---

## Dependency Injection

```
builder.Services.AddSingleton<UnvalidatedMessageModelToValidated>(
    MessageValidation.unvalidatedMessageModelToValidated)

builder.Services.AddSingleton<MessageModelToDomain>(
    MessageMapping.messageModelToDomain)

builder.Services.AddSingleton<DomainToMessageModel>(
    MessageMapping.domainToMessageModel)
       
// Partial application
builder.Services.AddSingleton<MessagePipeline>(
    MessageProcessing.processMessagePipeline
        MessageValidation.unvalidatedMessageModelToValidated
        MessageMapping.messageModelToDomain
        MessageMapping.domainToMessageModel)
```

- Functions and pipeline are registered via **DI**.

---

## ASCII Flow Diagram

```
UnvalidatedMessageModel
│
▼
Validation (Active Pattern)
│
├──> Error (UrlIsRequired) ──> ValidationErrorDto ──> BadRequest
│
▼
MessageModel (validated)
│
▼
messageModelToDomain
│
▼
Domain Message (DU)
│
▼
domainToMessageModel
│
▼
MessageModel (ready for API)
│
▼
ResultToActionResult.toActionResult
│
├──> Ok -> HTTP 200 with MessageModel
└──> Error -> HTTP 400 with ValidationErrorDto
```

- Shows how data flows from **incoming request** to **HTTP response**, with validation and mapping steps.

---

## Key Features

- Purely **functional architecture**: pure functions, Result, pipeline.
- Clear separation of **domain and infrastructure**.
- Easily extensible for new message types or validation rules.
- Swagger displays `MessageModel` as the successful type and DTO for errors.
