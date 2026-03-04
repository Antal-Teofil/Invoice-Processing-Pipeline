using Azure.AI.DocumentIntelligence;
using Azure.Storage.Blobs;
using InvoiceProcessingPipeline.Application.Auditing;
using InvoiceProcessingPipeline.Infrastructure.Adapters;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton<DocumentAuditOrchestrator>();
builder.Services.AddSingleton<CosmosClient>();
builder.Services.AddSingleton<BlobServiceClient>();
builder.Services.AddSingleton<DocumentIntelligenceClient>();

builder.Build().Run();
