namespace FSharpTestApi.Infrastructure

open FSharpTestApi

type ValidationErrorDto = { Code: string; Message: string }

module ModelValidationErrorExtensions =
         
    // Partial active pattern
    let (|UrlIsRequiredError|OtherError|) (error: 'Error) =
        match box error with
        | :? ModelValidationError as e ->
            match e with
            | UrlIsRequired -> UrlIsRequiredError
        | _ -> OtherError error
    
    let toErrorDto<'Error> (error: 'Error) =
        match error with
        | UrlIsRequiredError ->
            { Code = "UrlIsRequired"
              Message = "The 'Url' field is required for image messages." }
        | OtherError e ->
            { Code = "UnknownError"
              Message = $"An unexpected error occurred: %A{e}" }