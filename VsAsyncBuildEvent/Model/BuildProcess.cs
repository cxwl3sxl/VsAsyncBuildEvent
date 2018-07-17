using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace VsAsyncBuildEvent.Model
{
    public class BuildProcess
    {
        public string Cmd { get; }
        public string Argument { get; }
        public Guid BuildId { get; }
        public Exception ErrorException { get; private set; }


        private Process _process;

        public BuildProcess(string cmd, string argument)
        {
            Cmd = cmd;
            Argument = argument;
            BuildId = Guid.NewGuid();
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    ProcessStartInfo processStart = new ProcessStartInfo(Cmd, Argument);
                    _process = new Process();
                    _process.StartInfo = processStart;
                    _process.Start();
                    Hide();
                    _process.WaitForExit();
                    BuildFinfished?.Invoke(this);
                }
                catch (Exception ex)
                {
                    ErrorException = ex;
                    BuildFinfished?.Invoke(this);
                }
            });
        }

        public void Stop()
        {
            _process?.Kill();
        }

        public void Hide()
        {
            var windowIntPtr = IntPtr.Zero;
            var maxTry = 500;
            while (windowIntPtr == IntPtr.Zero)
            {
                windowIntPtr = _process?.MainWindowHandle ?? IntPtr.Zero;
                Thread.Sleep(50);
                maxTry--;
                if (maxTry < 0) return;
            }

            if (windowIntPtr == IntPtr.Zero) return;
            WinApics.ShowWindow(windowIntPtr, WinApics.SW_HIDE);
        }

        public void Show()
        {
            var mainWindow = _process?.MainWindowHandle ?? IntPtr.Zero;
            if (mainWindow == IntPtr.Zero) return;
            WinApics.ShowWindow(mainWindow, WinApics.SW_SHOW);
        }

        public Action<BuildProcess> BuildFinfished { get; set; }
    }
}