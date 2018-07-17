using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace VsAsyncBuildEvent.Model
{
    public class MainModel : INotifyPropertyChanged
    {
        private string _message;
        public ObservableCollection<BuildProcess> AllBuildProcesses { get; } = new ObservableCollection<BuildProcess>();
        public ICommand SuspendCommand { get; }
        public ICommand ShowProcess { get; }
        public ICommand HideProcess { get; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public MainModel()
        {
            SuspendCommand = new RealCommand<BuildProcess>(Suspend);
            ShowProcess = new RealCommand<BuildProcess>(p => p?.Show());
            HideProcess = new RealCommand<BuildProcess>(p => p.Hide());
            IsServiceOk = false;
        }

        void Suspend(BuildProcess process)
        {
            process.Stop();
            AllBuildProcesses.Remove(process);
        }

        public void Build(string cmd, string arguments)
        {
            var buildProcess = new BuildProcess(cmd, arguments) { BuildFinfished = BuildProcessFinished };
            buildProcess.Start();
            Application.Current.Dispatcher.Invoke(new Action<BuildProcess>(bp => AllBuildProcesses.Add(bp)),
                buildProcess);
            Message = $"正在执行 {cmd} {arguments}";
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
    }
}
