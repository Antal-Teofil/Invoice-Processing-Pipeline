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
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.DefaultIgnoreCondition = JsonIgnoreCondition.Never;

    options.Converters.Add(
        new JsonStringEnumConverter(
            JsonNamingPolicy.CamelCase));
});

var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;

typeAdapterConfig.Scan(
    typeof(CloudEventToDocumentEventMetadata).Assembly);

builder.Services.AddSingleton(typeAdapterConfig);
builder.Services.AddSingleton<IMapper, ServiceMapper>();

builder.Services.AddSingleton<
    TokenCredential,
    DefaultAzureCredential>();

// Cosmos DB
builder.Services.AddCosmosClient();

// Azure Blob Storage:
// - BlobServiceClient
// - keyed "documents-xml" BlobContainerClient
builder.Services.AddBlobClient();

// Azure Document Intelligence
builder.Services.AddDocumentIntelligenceClient();

// Azure Storage Queue
builder.Services.AddQueueClient();

// Application services
builder.Services.AddSingleton<
    IDocumentEventOrchestrator,
    DocumentEventOrchestratorService>();

// Extractors
builder.Services.AddSingleton<
    IDocumentDataExtractor,
    AzureDocumentIntelligenceExtractor>();

// Structured document storage in Cosmos DB
builder.Services.AddSingleton<
    IDocumentDataStore,
    CosmosDocumentSchemeStore>();

// XML document storage in Azure Blob Storage
builder.Services.AddSingleton<
    IDocumentXmlStore,
    AzureBlobDocumentStorage>();

// UBL XML exporter
builder.Services.AddSingleton<
    IDocumentSchemeExporter,
    UblExporter>();

builder.Build().Run();