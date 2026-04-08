using Azure.AI.DocumentIntelligence;
using Azure.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;

namespace InvoiceProcessingPipeline.Infrastructure.Configurations
{
    public static class DocumentIntelligenceConfigurationExtension
    {
        public static IServiceCollection AddDocumentIntelligenceClient(this IServiceCollection services)
        {
            services.AddOptionsWithValidateOnStart<DocumentIntelligenceOptions>()
                .BindConfiguration(DocumentIntelligenceOptions.SectionName)
                .ValidateDataAnnotations()
                .Validate(
                    options => Uri.TryCreate(options.Endpoint, UriKind.Absolute, out _),
                    "Endpoint must be a valid absolute URI.");

            services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<DocumentIntelligenceOptions>>().Value;
                var credential = sp.GetRequiredService<TokenCredential>();

                return new DocumentIntelligenceClient(
                    new Uri(options.Endpoint),
                    credential);
            });

            return services;
        }
    }

    public sealed record DocumentIntelligenceOptions
    {
        public const string SectionName = "DOCUMENT_INTELLIGENCE";

        [Url]
        public required string Endpoint { get; init; }
    }
}