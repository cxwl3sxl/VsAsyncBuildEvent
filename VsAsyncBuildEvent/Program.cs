using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using VsAsyncBuildEvent.Client;
using VsAsyncBuildEvent.Server;

namespace VsAsyncBuildEvent
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                if (NamedPipeClient.TryBuild(args[0], string.Join(" ", args.Skip(1)))) return;
                var exe = Assembly.GetEntryAssembly().Location;
                Process.Start(exe);
                NamedPipeClient.WaitServerStart();
                NamedPipeClient.TryBuild(args[0], string.Join(" ", args.Skip(1)));
            }
            else
            {
                StartApp();
            }
        }

        static void StartApp()
        {
            App app = new App();
            app.InitializeComponent();
            var nps = new NamedPipeServer();
            nps.Start();
            app.Run();
            nps.Dispose();
        }
    }
}
