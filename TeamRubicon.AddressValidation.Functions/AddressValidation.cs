using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;



namespace TeamRubicon.AddressValidation.Functions
{
    public static class AddressValidation
    {
        [FunctionName("AddressValidation")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"Webhook was triggered!");

            string jsonContent = await req.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(jsonContent);
            log.Info(jsonContent);

            if (data.street == null || data.zip == null || data.city == null || data.state == null)
            {
                return req.CreateResponse<ResponseContent>(
                HttpStatusCode.BadRequest,
                new ResponseContent
                {
                    version = "1.0.0",
                    status = (int)HttpStatusCode.BadRequest,
                    userMessage = "Please provide street address city state and zip code"
                },
                new JsonMediaTypeFormatter(),
                "application/json");
            }

            string experianApiQuery = data.street + " " + data.city + " " + data.state + " " + data.zip;
            string experianApiCountry = "US";

            //call experian API here and return a response similar to the above if it isn't valid.  


            //OTHERWISE return OK
            log.Info("returning 200");
            return req.CreateResponse(HttpStatusCode.OK);

        }

        public class ResponseContent

        {

            public string version { get; set; }

            public int status { get; set; }
            
            public string userMessage { get; set; }

        }

    }
}
