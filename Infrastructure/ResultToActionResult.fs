namespace FSharpTestApi.Infrastructure

open FSharpTestApi
open FSharpTestApi.Infrastructure.ModelValidationErrorExtensions
open Microsoft.AspNetCore.Mvc

type ResultToActionResultPipeline<'T> = ControllerBase -> Result<'T, ModelValidationError> -> ActionResult<'T>

module ResultToActionResult =

    let toActionResult (controller: ControllerBase) (result: Result<'T, 'Error>) : ActionResult<'T> =
        match result with
        | Ok value -> value |> ActionResult<'T>
        | Error err -> err |> toErrorDto |> controller.BadRequest |> ActionResult<'T>
