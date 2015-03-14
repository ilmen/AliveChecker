using System;

namespace AliveCheckerService.Classes.Models
{
    public class PingModel
    {
        public string Name
        { get; set; }

        public Guid Uid
        { get; set; }

        public DateTime LastPingTime
        { get; set; }
    }
}
