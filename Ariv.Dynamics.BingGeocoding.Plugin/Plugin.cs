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
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                var entity = (Entity)context.InputParameters["Target"];

                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    entity = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));

                    if (entity.Contains("address1_composite"))
                    {
                        var settings = JsonSerializer.Deserialize<Settings>(this.secureConfig);

                        var bingWebClient = new BingWebClient(settings.Key);
                        var address = entity.GetAttributeValue<string>("address1_composite");

                        var coordinates = bingWebClient.GetCoordinates(address);
                        if(coordinates != null)
                        {
                            var updateEntity = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(false));
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
    }
}
