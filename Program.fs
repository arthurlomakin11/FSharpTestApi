namespace FSharpTestApi

#nowarn "20"

open FSharpTestApi.Application
open FSharpTestApi.Infrastructure
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Scalar.AspNetCore
open System.Text.Json.Serialization

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services
            .AddControllers()
            .AddJsonOptions(fun options ->
                JsonFSharpOptions.Default().AddToJsonSerializerOptions(options.JsonSerializerOptions))


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
        
        builder.Services.AddOpenApi()


        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            app.MapOpenApi()
            app.MapScalarApiReference() |> ignore

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        exitCode
