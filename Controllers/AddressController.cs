using System.Net;
using Microsoft.AspNetCore.Mvc;
using GewerbeRegApi.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Nodes;

namespace gewerbe_reg_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AddressController> _logger;

        private const string OPENPLZAPIURL = "https://openplzapi.org/";


        public AddressController(HttpClient httpClient, ILogger<AddressController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpPost("search_by_plz")]
        public async Task<ActionResult<IEnumerable<PLZResponse>>> SearchOrt(PLZRequest plzRequest)
        {   // Staat = de, at, ch, fl
            try
            {
                if (plzRequest == null || plzRequest.Staat == null || plzRequest.PLZ == null)
                    return BadRequest("Staat und PLZ nicht übergeben");
                string url = $"https://openplzapi.org/{plzRequest.Staat.ToLower()}/Localities?postalCode={plzRequest.PLZ}";

                // 1. Externen Web Service aufrufen
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // 2. JSON als String lesen
                var jsonString = await response.Content.ReadAsStringAsync();

                // 3. Get Response from JSON
                using JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;
                var result = new List<PLZResponse>();

                foreach (JsonElement item in root.EnumerateArray())
                {
                    result.Add(new PLZResponse(item.GetProperty("postalCode").GetString(),
                                               item.GetProperty("name").GetString()));
                }

                // 4. Ergebnis zurückgeben
                return Ok(result.ToArray<PLZResponse>());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
