using Azure.AI.DocumentIntelligence;
using Azure.Storage.Blobs;
using InvoiceProcessingPipeline.Infrastructure.Configurations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();


// Cosmos Client configuration
builder.Services.AddCosmosClient();
builder.Services.AddSingleton<BlobServiceClient>();
builder.Services.AddSingleton<DocumentIntelligenceClient>();

builder.Build().Run();
