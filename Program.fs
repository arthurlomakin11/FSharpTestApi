namespace FSharpTestApi

#nowarn "20"

open System
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

        let messageModelToDomain (model: MessageModel) : Message =
            match model.Type with
            | MessageType.Image ->
                ImageMessage
                    { Content = model.Content
                      Url = model.Url.Value }

            | MessageType.Text -> TextMessage { Content = model.Content }
            | _ -> ArgumentOutOfRangeException() |> raise

        let domainToMessageModel (message: Message) : MessageModel =
            match message with
            | TextMessage message ->
                { Type = MessageType.Text
                  Content = message.Content
                  Url = Option.None }
            | ImageMessage message ->
                { Type = MessageType.Image
                  Content = message.Content
                  Url = Option.Some message.Url }


        builder.Services.AddSingleton<MessageModelToDomain>(messageModelToDomain)
        builder.Services.AddSingleton<DomainToMessageModel>(domainToMessageModel)

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
