namespace FSharpTestApi.Controllers

open FSharpTestApi
open Microsoft.AspNetCore.Mvc

[<ApiController>]
[<Route("[controller]")>]
type MessageController(messageModelToDomain: MessageModelToDomain, domainToMessageModel: DomainToMessageModel) =
    inherit ControllerBase()

    [<HttpPost>]
    member _.Post([<FromBody>] model: MessageModel) =
        let message = messageModelToDomain model |> domainToMessageModel
        message
