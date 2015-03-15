using AliveCheckerService.Classes.Models;
using AliveCheckerService.Classes.PingSevices;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;

namespace AliveCheckerService.Classes.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Timer for updating VMs
        /// </summary>
        private static Timer tmrUpdatePVM = new Timer()
        {
            Interval = 1000,
            Enabled = true,
        };

        private Dispatcher currentDispatcher = Dispatcher.CurrentDispatcher;    // для работы с Pings из других потоков (из обработчиков событий)

        private IPingService pingServiceClaster;

        public ObservableCollection<PingViewModel> Pings
        { get; private set; }

        public ICommand HideCommand
        { get; private set; }

        /// <summary>
        /// Необходимость отображения окна
        /// </summary>
        public bool ExistOfflineDevices
        { get; private set; }

        public MainWindowViewModel()
        {
            pingServiceClaster = new PingServiceClaster();
            pingServiceClaster.NewPing += pingServiceClaster_NewPing;

            HideCommand = new DelegateCommand<MainWindow>()
            {
                CanExecuteFunc = (w) => !this.ExistOfflineDevices,
                CommandAction = new Action<MainWindow>((w) =>
                {
                    if (w != null) w.Hide();
                }),
            };

            Pings = new ObservableCollection<PingViewModel>();

            //AddFakePings(tmrUpdatePVM);

            tmrUpdatePVM.Elapsed += (s, e) =>
                {
                    this.ExistOfflineDevices = Pings.Any(x => x.IsOffline);
                };
        }

        void pingServiceClaster_NewPing(object sender, PingEvengArgs args)
        {
            currentDispatcher.BeginInvoke(new Action(() =>
            {
                var model = Pings.FirstOrDefault(x => x.Uid == args.Model.Uid);
                if (model != null)
                {
                    model.LastPingTime = DateTime.Now;
                    model.Name = args.Model.Name;
                }
                else
                {
                    Pings.Add(new PingViewModel(args.Model, tmrUpdatePVM));
                }
            }));
        }

        //void AddFakePings(Timer tmr)
        //{
        //    var item1 = new PingModel()
        //    {
        //        Name = "LG P500",
        //        Uid = Guid.NewGuid(),
        //        LastPingTime = DateTime.Now.AddMinutes(-5).AddSeconds(10),
        //    };

        //    Pings.Add(new PingViewModel(item1, tmr));

        //    var item2 = new PingModel()
        //    {
        //        Name = "Azure Collection Statictic Server",
        //        Uid = Guid.NewGuid(),
        //        LastPingTime = DateTime.Now,
        //    };

        //    Pings.Add(new PingViewModel(item2, tmr));
        //}
    }
}
