using System;
namespace AliveCheckerService.Classes.PingSevices
{
    public interface IPingService
    {
        event NewPingEventHandler NewPing;
    }

    public delegate void NewPingEventHandler(object sender, PingEvengArgs args);

    public class PingEvengArgs : EventArgs
    {
        public PingModel Model
        { get; private set; }

        public PingEvengArgs(PingModel model)
        {
            this.Model = model;
        }
    }
}
