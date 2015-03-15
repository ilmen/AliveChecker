using AliveCheckerService.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace AliveCheckerService.Classes.ViewModels
{
    public class PingViewModel : ViewModelBase
    {
        private PingModel model;

        public string Name
        {
            get
            {
                return model.Name;
            }
            set
            {
                if (model != null && model.Name.Equals(value)) return;

                model.Name = value;
                RaisePropertyChange("Name");
            }
        }

        public Guid Uid
        {
            get
            {
                return model.Uid;
            }
            set
            {
                model.Uid = value;

                RaisePropertyChange("Uid");
            }
        }

        public DateTime LastPingTime
        {
            get
            {
                return model.LastPingTime;
            }
            set
            {
                if (model != null && model.LastPingTime.Equals(value)) return;

                model.LastPingTime = value;
                RaisePropertyChange("LastPingTime");
                RaisePropertyChange("PingDelay");
                RaisePropertyChange("IsOffline");
            }
        }

        public TimeSpan PingDelay
        {
            get
            {
                return DateTime.Now - LastPingTime;
            }
        }

        public bool IsOffline
        {
            get
            {
                return PingDelay >= TimeSpan.FromMinutes(5);
            }
        }

        public PingViewModel(PingModel model)
        {
            this.model = model;
        }

        public PingViewModel(PingModel model, Timer timer) : this(model)
        {
            timer.Elapsed += (s, e) =>
            {
                RaisePropertyChange("PingDelay");
                RaisePropertyChange("IsOffline");
            };
        }
    }
}
