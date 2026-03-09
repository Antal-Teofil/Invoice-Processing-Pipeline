using Azure.AI.DocumentIntelligence;
using Azure.Core;
using Azure.Identity;
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

builder.Services.AddSingleton<TokenCredential, DefaultAzureCredential>();

// Cosmos Client configuration
builder.Services.AddCosmosClient();

// Blob Client configuration
builder.Services.AddBlobClient();

// Azure Document Intelligence Cleint configuration
builder.Services.AddDocumentIntelligenceClient();

builder.Build().Run();
