using Azure.Core;
using Azure.Identity;
using InvoiceProcessingPipeline.Application.MapperConfigurations;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Infrastructure.Adapters;
using InvoiceProcessingPipeline.Infrastructure.Configurations;
using Mapster;
using MapsterMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;

typeAdapterConfig.Scan(typeof(CloudEventToDocumentEventMetadata).Assembly);

builder.Services.AddSingleton(typeAdapterConfig);
builder.Services.AddSingleton<IMapper, ServiceMapper>();

builder.Services.AddSingleton<TokenCredential, DefaultAzureCredential>();

// Cosmos Client configuration
builder.Services.AddCosmosClient();

// Blob Client configuration
builder.Services.AddBlobClient();

// Azure Document Intelligence Cleint configuration
builder.Services.AddDocumentIntelligenceClient();

// Azure Strorage Queue
builder.Services.AddQueueClient();

builder.Services.AddSingleton<IDocumentEventOrchestrator, DocumentEventOrchestratorService>();


builder.Build().Run();
