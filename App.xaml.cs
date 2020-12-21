using DevExpress.Xpf.Core;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace WorkerUse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SplashScreenManager.CreateFluent(new DevExpress.Mvvm.DXSplashScreenViewModel { Title = "生产测试系统", IsIndeterminate = true, Status = "加载中...", Copyright = $"Copyright @{DateTime.Now.Year} GreenWhale", Logo = null, Subtitle = "Designed by wichie" }).ShowOnStartup();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin");
            var assemblyName = new AssemblyName(args.Name);

            path = Path.Combine(path, assemblyName.Name);
            path = string.Format(@"{0}.dll", path);
            if (File.Exists(path))
            {
                return System.Reflection.Assembly.LoadFrom(path);

            }
            return null;

        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
