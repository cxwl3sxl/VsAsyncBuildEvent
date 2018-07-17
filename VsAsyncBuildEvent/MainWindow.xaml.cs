using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace VsAsyncBuildEvent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NotifyIcon _notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            _notifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.main,
                Visible = true
            };
            _notifyIcon.DoubleClick += _notifyIcon_DoubleClick;
            _notifyIcon.ContextMenu = new ContextMenu();
            _notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("退出", (s, e) =>
            {
                Application.Current.Shutdown();
            }));
            MainModelResover.StaticMainModel.PropertyChanged += StaticMainModel_PropertyChanged;
        }

        private void _notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void StaticMainModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(MainModelResover.StaticMainModel.Message)) return;
            _notifyIcon.BalloonTipText = MainModelResover.StaticMainModel.Message;
            _notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            _notifyIcon.BalloonTipTitle = "提示";
            _notifyIcon.ShowBalloonTip(3000);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
            Hide();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            MainModelResover.StaticMainModel.IsServiceOk = true;
        }
    }
}
