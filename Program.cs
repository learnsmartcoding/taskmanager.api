using LearnSmartCoding.CosmosDb.Linq.API.Data;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Cosmos;
using Serilog;

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

                // Configure Serilog with the settings
                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()                
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

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
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