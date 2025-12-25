namespace FSharpTestApi.Controllers

open FSharpTestApi
open FSharpTestApi.Infrastructure.ResultToActionResult
open Microsoft.AspNetCore.Mvc

[<ApiController>]
[<Route("[controller]")>]
type MessageController(pipeline: MessagePipeline) =
    inherit ControllerBase()

    [<HttpPost>]
    member this.Post([<FromBody>] model: UnvalidatedMessageModel) =
        model |> pipeline |> toActionResult this
