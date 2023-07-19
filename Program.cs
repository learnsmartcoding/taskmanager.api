
using Azure.Storage.Blobs;
using LearnSmartCoding.CosmosDb.Linq.API.Data;
using LearnSmartCoding.CosmosDb.Linq.API.Model;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Azure.Cosmos;
using Serilog;
using Serilog.Events;
using System.Collections;
using System.Net;

namespace LearnSmartCoding.CosmosDb.Linq.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                var configuration = builder.Configuration;

                builder.Services.AddApplicationInsightsTelemetry();

                // for local storage
                //Log.Logger = new LoggerConfiguration()
                //        .WriteTo.Console()
                //        .MinimumLevel.Information()
                //        .WriteTo.File("/app/logs/application.log", rollingInterval: RollingInterval.Day,
                //          fileSizeLimitBytes: 5242880, retainedFileCountLimit: 7,
                //        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                //        .Enrich.FromLogContext()
                //        .CreateBootstrapLogger();


                // Read the Azure Blob Storage settings from appsettings.json
                var azureBlobStorageSettings = configuration.GetSection("AzureBlobStorage").Get<AzureBlobStorageSettings>();

                // Configure Serilog with the settings
                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Information()
                .WriteTo.AzureBlobStorage(
                    blobServiceClient: new BlobServiceClient(azureBlobStorageSettings?.ConnectionString),
                    restrictedToMinimumLevel: LogEventLevel.Verbose,
                    storageContainerName: azureBlobStorageSettings?.ContainerName,
                    storageFileName: "application.log",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    retainedBlobCountLimit: 7,
                    blobSizeLimitBytes: 5242880,
                    useUtcTimeZone: true
                )
                .Enrich.FromLogContext()
                .CreateLogger();

                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                .WriteTo.ApplicationInsights(
                        services.GetRequiredService<TelemetryConfiguration>(),
                        TelemetryConverter.Traces));

                Log.Information("Starting the application...");

                // Add CosmosClient and configure logging
                builder.Services.AddSingleton((provider) =>
                {
                    var endpointUri = configuration["CosmosDbSettings:EndpointUri"];
                    var primaryKey = configuration["CosmosDbSettings:PrimaryKey"];
                    var databaseName = configuration["CosmosDbSettings:DatabaseName"];

                    var cosmosClientOptions = new CosmosClientOptions
                    {
                        ApplicationName = databaseName,
                        ConnectionMode = ConnectionMode.Gateway,

                        //ServerCertificateCustomValidationCallback = (request, certificate, chain) =>
                        //{
                        //    // Always return true to ignore certificate validation errors
                        //    return true; //not for production
                        //}
                    };

                    var loggerFactory = LoggerFactory.Create(builder =>
                    {
                        builder.AddConsole();
                    });

                    var cosmosClient = new CosmosClient(endpointUri, primaryKey, cosmosClientOptions);


                    return cosmosClient;
                });

                builder.Services.AddControllers();

                // In production, modify this with the actual domains you want to allow
                builder.Services.AddCors(o => o.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));


                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddScoped<ITaskRepository, TaskRepository>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();

                var app = builder.Build();


                // Enable Serilog exception logging
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());

                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
                    });
                });

                app.UseMiddleware<RequestResponseLoggingMiddleware>();

                // Configure the HTTP request pipeline.
                // if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseCors("default");

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
                Log.Information("Application started successfully.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }




        }
    }
}