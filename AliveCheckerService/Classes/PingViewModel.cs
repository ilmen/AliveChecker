using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AliveCheckerService.Classes
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
            }
        }

        public TimeSpan PingDelay
        {
            get
            {
                return DateTime.Now - LastPingTime;
            }
        }

        public PingViewModel(PingModel model)
        {
            this.model = model;
        }

        public PingViewModel(PingModel model, DispatcherTimer timer) : this(model)
        {
            timer.Tick += (s, e) =>
            {
                RaisePropertyChange("PingDelay");
            };
        }
    }
}
