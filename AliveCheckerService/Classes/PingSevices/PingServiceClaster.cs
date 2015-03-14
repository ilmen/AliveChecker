using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliveCheckerService.Classes.PingSevices
{
    public class PingServiceClaster : IPingService
    {
        public event NewPingEventHandler NewPing = delegate { };

        public IEnumerable<IPingService> Services
        { get; private set; }

        public PingServiceClaster()
        {
            Services = new List<IPingService>();

            InitializeServices();
        }

        private void InitializeServices()
        {
            var httpService = new HttpService();
            httpService.NewPing += ResendNewPingHandler;
        }

        void ResendNewPingHandler(object sender, PingEvengArgs args)
        {
            NewPing(sender, args);
        }
    }
}
