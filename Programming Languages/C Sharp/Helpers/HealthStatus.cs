using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

/**
HealthStatus healthStatus = new();

var results = new List<HealthResult>
{
    // Check 1: Application is running
    new("Application", true, "Running")
};

// Check 2: Database connections
results.AddRange(healthStatus.CheckAllDbContexts());

// Check 3: External APIs
#region ExternalAPIs
results.Add(await healthStatus.CheckExternalGetApiAsync("ExternalAPI - CommEngine", new RestClient($"{ConfigurationManager.AppSettings["BICommunicationEnginePassthroughUrl"]}/go")));

// Gravity Forms
List<string> gfCompanies = ["SSI", "LCI", "RGI", "SSI_CA", "SSI_AU"];
foreach (var companyName in gfCompanies) {
    var gfClient = new RestClient($"{ConfigurationManager.AppSettings[$"GravityFormsURL_{companyName}"]}/wp-json/gf/v2/forms")
    {
        Authenticator = OAuth1Authenticator.ForProtectedResource(
                ConfigurationManager.AppSettings[$"GravityFormsConsumerKey_{companyName}"],
                ConfigurationManager.AppSettings[$"GravityFormsConsumerSecret_{companyName}"], null, null)
    };
    results.Add(await healthStatus.CheckExternalGetApiAsync($"ExternalAPI - {companyName} Gravity Forms", gfClient));
}

// Call Center Hours of Operation
List<string> ccohCompanies = ["SSI", "RGI"];
foreach (var companyName in ccohCompanies)
{
    var credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
        $"{ConfigurationManager.AppSettings[$"PublicWebsiteUserName_{companyName}"]}:{ConfigurationManager.AppSettings[$"PublicWebsitePassword_{companyName}"]}"));

    var ccohClient = new RestClient($"{ConfigurationManager.AppSettings[$"PublicWebsiteURL_{companyName}"]}/wp-json/rest-api/v1/call-center-operating-hours/");
    ccohClient.AddDefaultHeader("Authorization", $"Basic {credentials}");
    results.Add(await healthStatus.CheckExternalGetApiAsync($"ExternalAPI - {companyName} Call Center Hours Of Operation", ccohClient));
}

// LCI Call Center Hours of Operation
var lciCcohClient = new RestClient($"{ConfigurationManager.AppSettings[$"LCI_WebAPI_URL"]}/api/Dashboard/GetAllCallBackSchedules");
lciCcohClient.AddDefaultHeader("Authorization", $"Bearer {ConfigurationManager.AppSettings[$"LCI_WebAPI_Key"]}");
results.Add(await healthStatus.CheckExternalGetApiAsync($"ExternalAPI - Service Desk", lciCcohClient));

//Service Desk
var sdClient = new RestClient($"{AppSettings.SupportCenter_BasePath}/accounts");
sdClient.AddDefaultHeader("AUTHTOKEN", AppSettings.SupportCenter_APIKey);
results.Add(await healthStatus.CheckExternalGetApiAsync($"ExternalAPI - Service Desk", sdClient));

//Phone System
var _phoneSysConfig = PhoneSystemConfiguration.Instance;
var phoneClient = new RestClient($"https://{_phoneSysConfig.ApiServerAddress}:{_phoneSysConfig.WebsvcsApiPort}/websvcs/serverConfiguration");
results.Add(await healthStatus.CheckExternalGetApiAsync($"ExternalAPI - Phone System", phoneClient));
#endregion ExternalAPIs

// Overall status
bool allHealthy = results.All(r => r.IsHealthy);
var OverallStatus = allHealthy ? "Healthy" : "Unhealthy";

// Add custom header
HttpContext.Current.Response.Headers.Add("X-HealthStatus", OverallStatus);

return Ok(new
{
    OverallStatus,
    timestamp = DateTime.UtcNow,
    checks = results
});
**/

namespace SmartStartWebAPI.Business.Utility
{
    public class HealthStatus
    {
        public List<HealthResult> CheckAllDbContexts()
        {
            var results = new List<HealthResult>();

            // You can replace Assembly.GetExecutingAssembly() with the specific assembly if needed
            var dbContextTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location)) // skip dynamic and invalid assemblies
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null); // Return only successfully loaded types
                }
                catch
                {
                    return Enumerable.Empty<Type>(); // Fail-safe
                }
            })
            .Where(t =>
                t != null &&
                t.IsClass &&
                !t.IsAbstract &&
                typeof(DbContext).IsAssignableFrom(t) &&
                t.GetConstructor(Type.EmptyTypes) != null);

            foreach (var contextType in dbContextTypes)
            {
                try
                {
                    using (var context = (DbContext)Activator.CreateInstance(contextType))
                    {
                        context.Database.Connection.Open();
                        var isOpen = context.Database.Connection.State == ConnectionState.Open;

                        results.Add(new HealthResult($"Database - {contextType.Name}", isOpen, isOpen ? "Connected" : "Connection failed"));
                    }
                }
                catch (Exception ex)
                {
                    results.Add(new HealthResult($"Database - {contextType.Name}", false, ex.Message));
                }
            }

            return results;
        }

        public async Task<HealthResult> CheckExternalGetApiAsync(string name, RestClient restSharpClient)
        {
            var head = await CheckExternalApiHeadAsync(name, restSharpClient);
            if (head.IsHealthy)
            {
                return head;
            }

            var options = await CheckOptionsAsync(name, restSharpClient);
            if (options.IsHealthy)
            {
                return options;
            }

            var get = await CheckExternalApiLightGetAsync(name, restSharpClient);
            if (get.IsHealthy)
            {
                return get;
            }

            return new HealthResult(name, false, $"Unable to reach API: {restSharpClient.BaseUrl}");
        }

        private async Task<HealthResult> CheckExternalApiHeadAsync(string name, RestClient restSharpClient = null)
        {
            try
            {
                var request = new RestRequest(Method.HEAD);

                var response = await restSharpClient.ExecuteAsync(request);

                return new HealthResult(
                    name,
                    response.IsSuccessful,
                    $"Status {(int)response.StatusCode} {response.StatusDescription}"
                );
            }
            catch (Exception ex)
            {
                return new HealthResult(name, false, ex.Message);
            }
        }

        private async Task<HealthResult> CheckOptionsAsync(string name, RestClient restSharpClient = null)
        {
            try
            {
                var request = new RestRequest(Method.OPTIONS);

                var response = await restSharpClient.ExecuteAsync(request);

                var allowHeader = response.Headers
                    .FirstOrDefault(h => h.Name.Equals("Allow", StringComparison.OrdinalIgnoreCase))?.Value?.ToString();

                return new HealthResult(
                    name,
                    response.IsSuccessful,
                    $"Status {(int)response.StatusCode}, Allow: {allowHeader ?? "N/A"}"
                );
            }
            catch (Exception ex)
            {
                return new HealthResult(name, false, ex.Message);
            }
        }

        private async Task<HealthResult> CheckExternalApiLightGetAsync(string name, RestClient restSharpClient = null)
        {
            try
            {
                var request = new RestRequest(Method.GET);

                // Optional: avoid triggering deep logic by using a light route like `/ping`
                var response = await restSharpClient.ExecuteAsync(request);

                return new HealthResult(
                    name,
                    response.IsSuccessful,
                    $"Status {(int)response.StatusCode} {response.StatusDescription}"
                );
            }
            catch (Exception ex)
            {
                return new HealthResult(name, false, ex.Message);
            }
        }
    }

    public class HealthResult(string name, bool healthy, string message)
    {
        public string Name { get; set; } = name;
        public bool IsHealthy { get; set; } = healthy;
        public string Message { get; set; } = message;
    }
}
