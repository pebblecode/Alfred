using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.GUI
{
    public enum WfhStatus
    {
        InOffice,
        OutOfOffice,
        Sick,
        Holiday
    }

    public static class WfhApi
    {
        private static readonly Uri WfhUri = new Uri("http://pebblecode-wfh.herokuapp.com/workers");

        public static async Task SetStatus(WfhStatus status)
        {
            var client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes("admin:pebblcodehackday");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            var content = string.Format(
                "{\"email\":\"saverio.castelli@pebblecode.com\", \"status\":\"{0}\" }",
                status.ToString());
            await client.PutAsync(WfhUri, new StringContent(content));
        }
    }
}
