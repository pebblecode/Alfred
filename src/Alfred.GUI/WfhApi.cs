using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var byteArray = Encoding.ASCII.GetBytes("admin:passwordremoved");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            var content = string.Format(
                "{{ \"email\":\"james.gregory@pebblecode.com\", \"status\":\"{0}\" }}",
                status.ToString());
            var result = await client.PutAsync(WfhUri, new StringContent(content, Encoding.UTF8, "application/json"));
            Debug.WriteLine(result.StatusCode);
        }
    }
}
