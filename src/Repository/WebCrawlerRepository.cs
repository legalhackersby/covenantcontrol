using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace src.Repository
{
    public class WebCrawlerRepository
    {
        private readonly HttpClient client = new HttpClient();

        private const string URL = "https://iswarm.azure-api.net/api/v1/report/socialmessage?html=false&offset={0}&limit=50&sources=OSFI_CA_Chapter&lang=en&features=true";

        public WebCrawlerRepository()
        {
            this.client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "fe8ba2b9adb342dcb8e6bcdccb7e3a54");
        }

        public List<string> GetData()
        {
            List<string> resultList = new List<string>();

            int i = 0;
            while(true)
            {
                var url = string.Format(URL, i * 50);
                var request = WebRequest.Create(url);
                request.Headers.Add("Ocp-Apim-Subscription-Key", "fe8ba2b9adb342dcb8e6bcdccb7e3a54");
                request.Credentials = new NetworkCredential("osfi_ca_demo1", "ErFULDtuSq2ZzcLA");
                string content;

                using (var stream = request.GetResponse().GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadLine();
                        
                    }
                }

                if (content == "[]" || content == "{\"status\":\"invalid_parameter\",\"action\":\"offset\",\"object\":\"No iterator exists for this offset\",\"id\":0}" /*|| i > 0*/)
                {
                    break;
                }
                else
                {
                    resultList.Add(content); 
                }
                
                i++;
            } 

            return resultList;



            /*var response = await this.client.GetAsync(URL);

            var content = await response.Content.ReadAsStringAsync();

            return content;*/
        }
    }
}
