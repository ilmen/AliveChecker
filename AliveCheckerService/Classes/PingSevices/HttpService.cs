using AliveCheckerService.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AliveCheckerService.Classes.PingSevices
{
    public class HttpService : IPingService, IDisposable
    {
        public event NewPingEventHandler NewPing = delegate { };

        HttpListener listener = new HttpListener();

        public HttpService()
        {
            listener.Prefixes.Add("http://localhost:5555/");
            listener.Start();

            GetContext();
        }

        public async void GetContext()
        {
            var context = await listener.GetContextAsync();

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var getParams = context.Request.QueryString;
            
            Guid uid;
            if (Guid.TryParse(getParams["uid"], out uid))
            {
                var model = new PingModel()
                {
                    Name = getParams["name"] ?? "DeviceName",
                    Uid = uid,
                    LastPingTime = DateTime.Now,
                };

                NewPing(this, new PingEvengArgs(model));

                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }

            context.Response.Close();

            GetContext();
        }

        public void Dispose()
        {
            if (listener.IsListening) listener.Stop();
        }
    }
}
