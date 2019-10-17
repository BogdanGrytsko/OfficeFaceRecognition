using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Security;
using System.Text;

namespace FaceRecognition.Door
{
    public class DoorManager
    {
        private const string url = @"https://192.168.4.2/command/open";
        private const string ThumbPrint = "6a18f05530004142a839333ffaefe2b17141e214";
        private readonly HttpClient httpClient;

        public DoorManager()
        {
            httpClient = new HttpClient();
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
            {
                return sslPolicyErrors == SslPolicyErrors.None || string.Equals(cert.GetCertHashString(), ThumbPrint, StringComparison.OrdinalIgnoreCase);
            };
        }

        public async void Open()
        {
            var payload = new DoorPayload { pass = "P_Open!Test" };
            var serialized = JsonConvert.SerializeObject(payload);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync(url, content);
            if (!res.IsSuccessStatusCode)
                Console.WriteLine($"Can't open the door! Code : {res.StatusCode}");
            else
                Console.WriteLine("Door opened!");
        }
    }
}
