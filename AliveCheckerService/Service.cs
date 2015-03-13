using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AliveCheckerService
{
    public class Service : IDisposable
    {
        static Dictionary<Guid, DateTime> lastPing = new Dictionary<Guid, DateTime>();

        HttpListener listener = new HttpListener();

        public Service()
        {
            listener.Prefixes.Add("http://localhost:5555/");
            listener.Start();

            GetContext();
        }

        public async void GetContext()
        {
            var context = await listener.GetContextAsync();

            bool result = false;

            var keys = context.Request.QueryString.AllKeys.Select(x => x.ToLower()).ToList();
            if (keys.Any(x => x == "uid"))
            {
                var uidString = context.Request.QueryString["uid"];

                Guid uid;
                if (Guid.TryParse(uidString, out uid))
                {
                    lastPing[uid] = DateTime.Now;
                    result = true;
                }
            }

            var content = Encoding.Default.GetBytes(result.ToString());
            context.Response.OutputStream.Write(content, 0, content.Length);
            context.Response.Close();

            GetContext();
        }

        public void Dispose()
        {
            if (listener.IsListening) listener.Stop();
        }
    }
}
