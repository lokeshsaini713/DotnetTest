using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalDataController : ControllerBase
    {
        private readonly ExternalApiService _externalApiService;

        public ExternalDataController(ExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }

        [HttpGet]
        [Route("getdata")]
        public async Task<IActionResult> GetData()
        {
            // Replace with the URL of the external API
            

            var dataSet = await GetDataByAPI();

            if (dataSet == null)
            {
                return NotFound("No data found");
            }

            return Ok(dataSet);
        }
        public async Task<DataSet> GetDataByAPI()
        {
            string apiUrl = "https://jsonplaceholder.org/posts";
            var dataSet = await _externalApiService.FetchDataFromApiAsync(apiUrl);
            return dataSet??new DataSet();
        }
    }
}
