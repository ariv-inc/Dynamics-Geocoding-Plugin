using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ServiceModel;

namespace Ariv.Dynamics.BingGeocoding.Plugin
{
    public class BingGeocodingPlugin : IPlugin
    {
        private readonly string secureConfig;

        public BingGeocodingPlugin(string unsecureConfig, string secureConfig)
        {
            if (string.IsNullOrWhiteSpace(secureConfig))
            {
                throw new InvalidPluginExecutionException("Secure config is required for this plugin to execute.");
            }

            this.secureConfig = secureConfig;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    entity = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));

                    if (HasRequiredAttributes(entity))
                    {
                        var settings = JsonSerializer.Deserialize<Settings>(this.secureConfig);
                        var bingWebClient = new BingWebClient(tracingService, settings.Key);
                        var address = entity.GetAttributeValue<String>("address1_composite");

                        var coordinates = bingWebClient.GetCoordinates(address);
                        if(coordinates != null)
                        {
                            Entity updateEntity = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(false));
                            updateEntity["address1_latitude"] = coordinates.Latitude;
                            updateEntity["address1_longitude"] = coordinates.Longitude;

                            service.Update(updateEntity);
                        }
                    }
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in BingGeocoding plugin.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("BingGeocoding pluging: {0}", ex.ToString());
                    throw;
                }
            }
        }

        private bool HasRequiredAttributes(Entity entity)
        {
            return entity.Contains("address1_composite") && entity.Contains("address1_latitude") && entity.Contains("address1_longitude");
        }
    }
}
