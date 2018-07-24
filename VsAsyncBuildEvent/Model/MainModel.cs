using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VsAsyncBuildEvent.Model
{
    public class MainModel : INotifyPropertyChanged
    {
        private string _message;
        private int _delay = 5;
        private bool _runImmediately;
        private bool _runDelay;
        private bool _runManually;
        public ObservableCollection<BuildProcess> AllBuildProcesses { get; } = new ObservableCollection<BuildProcess>();
        public ICommand SuspendCommand { get; }
        public ICommand ShowProcess { get; }
        public ICommand HideProcess { get; }
        public ICommand StartCommand { get; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public int Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                OnPropertyChanged(nameof(Delay));
            }
        }

        public bool RunImmediately
        {
            get => _runImmediately;
            set
            {
                _runImmediately = value;
                OnPropertyChanged(nameof(RunImmediately));
            }
        }

        public bool RunDelay
        {
            get => _runDelay;
            set
            {
                _runDelay = value;
                OnPropertyChanged(nameof(RunDelay));
            }
        }

        public bool RunManually
        {
            get => _runManually;
            set
            {
                _runManually = value;
                OnPropertyChanged(nameof(RunManually));
            }
        }

        public MainModel()
        {
            SuspendCommand = new RealCommand<BuildProcess>(Suspend);
            ShowProcess = new RealCommand<BuildProcess>(p => p?.Show());
            HideProcess = new RealCommand<BuildProcess>(p => p?.Hide());
            StartCommand = new RealCommand<BuildProcess>(p =>
            {
                if (p == null) return;
                p.Start();
                Message = $"正在执行 {p.Cmd} {p.Argument}";
            });
            IsServiceOk = false;
            RunImmediately = true;
            RunDelay = false;
            RunManually = false;
        }

        void Suspend(BuildProcess process)
        {
            if (process == null) return;
            process.Stop();
            AllBuildProcesses.Remove(process);
        }

        public void Build(string cmd, string arguments)
        {
            var buildProcess = new BuildProcess(cmd, arguments) { BuildFinfished = BuildProcessFinished };
            var msg = $"正在执行 {cmd} {arguments}";
            if (MainModelResover.StaticMainModel.RunImmediately)
                buildProcess.Start();
            else
                msg = $"任务已经添加 {cmd} {arguments}";

            if (MainModelResover.StaticMainModel.RunDelay)
            {
                Task.Factory.StartNew(DelayRun, buildProcess);
            }

            Application.Current.Dispatcher.Invoke(new Action<BuildProcess>(bp => AllBuildProcesses.Add(bp)),
                buildProcess);
            Message = msg;
        }

        void BuildProcessFinished(BuildProcess process)
        {
            Application.Current.Dispatcher.Invoke(new Action<BuildProcess>(bp => AllBuildProcesses.Remove(bp)),
                process);
            Message = $"{process.Cmd} {process.Argument}执行完毕\n{process.ErrorException?.Message}";
        }

        public bool IsServiceOk { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void DelayRun(object bpObject)
        {
            if (!(bpObject is BuildProcess buildProcess)) return;
            Thread.Sleep(MainModelResover.StaticMainModel.Delay * 1000);
            buildProcess.Start();
            Message = $"正在执行 {buildProcess.Cmd} {buildProcess.Argument}";
        }
    }
}
