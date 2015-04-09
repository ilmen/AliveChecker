using AliveCheckerService.Classes;
using AliveCheckerService.Classes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AliveCheckerService
{
    public class Program
    {
        public static bool WaitToShow
        { get; set; }

        public static bool CanClose
        { get; set; }

        public static string HttpServicePrefix
        { get; set; }

        [STAThread]
        public static void Main(string[] args)
        {
            ReadArgs(args);

            var notifyIcon = CreateNotifyIcon();
            var vm = new MainWindowViewModel();
            var view = new MainWindow() { DataContext = vm };
            
            while(!Program.CanClose)
            {
                if (!view.IsActive && (vm.ExistOfflineDevices || Program.WaitToShow))
                {
                    Program.WaitToShow = false;
                    view.Show();
                }

                System.Threading.Thread.Sleep(200);
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private static void ReadArgs(string[] args)
        {
            // parsing http_prefix argument
            var httpPrefixArg = args
                .Where(x => x.Contains('='))
                .Select(x => new
                {
                    key = x.Substring(0, x.IndexOf('=')).ToLower(),
                    value = x.Substring(x.IndexOf('=') + 1),
                })
                .Where(x => x.key == "http_prefix")
                .Select(x => x.value)
                .FirstOrDefault();
            HttpServicePrefix = httpPrefixArg ?? "http://+:8888/";
        }

        private static NotifyIcon CreateNotifyIcon()
        {
            var ni = new NotifyIcon();
            ni.Text = "Device Checker";
            ni.Icon = System.Drawing.Icon.FromHandle(Properties.Resources.NotifyIcon.GetHicon());
            ni.Visible = true;
            ni.DoubleClick += (s, e) => Program.WaitToShow = true;

            ni.ContextMenuStrip = new ContextMenuStrip();
            ni.ContextMenuStrip.Items.Add("Открыть").Click += (s, e) => Program.WaitToShow = true;
            ni.ContextMenuStrip.Items.Add("Выход").Click += (s, e) => Program.CanClose = true;
            
            return ni;
        }
    }
}
