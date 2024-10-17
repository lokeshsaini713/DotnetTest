using Newtonsoft.Json.Linq;
using System.Data;

namespace API
{
    public class ExternalApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<DataSet> FetchDataFromApiAsync(string apiUrl)
        {
            try
            {

                DataSet dataSet = new DataSet();
                var client = _httpClientFactory.CreateClient();

                // Make the request to the external API
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    // Parse the response into a DataSet (assumes JSON response)
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(jsonResponse))
                    {                     // Assuming you convert JSON to DataSet (you can use Newtonsoft.Json or System.Text.Json)
                        dataSet = ConvertJsonToDataSet(jsonResponse);
                    }
                }
                return dataSet;
            }
            catch (Exception)
            {

                throw new HttpRequestException();
            }
        }

        private DataSet ConvertJsonToDataSet(string json)
        {

            var dataSet = new DataSet();
            JArray jsonArray = JArray.Parse(json);

            // Create a DataTable to hold the data
            DataTable dataTable = new DataTable();

            // Loop through the first object to create DataTable columns
            if (jsonArray.Count > 0)
            {
                JObject firstObject = (JObject)jsonArray[0];
                foreach (JProperty property in firstObject.Properties())
                {
                    // Create a column for each property in the first JSON object
                    dataTable.Columns.Add(property.Name, typeof(string)); // Use string, you can customize type based on data
                }
            }

            // Populate the DataTable with rows
            foreach (JObject obj in jsonArray)
            {
                DataRow row = dataTable.NewRow();
                foreach (JProperty property in obj.Properties())
                {
                    // Assign each property value to the corresponding column
                    row[property.Name] = property.Value.ToString();
                }
                dataTable.Rows.Add(row);
            }

            // Create a DataSet and add the DataTable to it
            //DataSet set = new DataSet();
            dataSet.Tables.Add(dataTable);

            return dataSet;

        }
    }

}
