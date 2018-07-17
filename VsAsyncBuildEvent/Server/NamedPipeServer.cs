using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;
using VsAsyncBuildEvent.Contact;

namespace VsAsyncBuildEvent.Server
{
    public class NamedPipeServer : IDisposable
    {
        public const string ServerAddress = "net.pipe://localhost/VsAsyncBuild";
        private readonly ServiceHost _serviceHost;

        public NamedPipeServer()
        {
            _serviceHost = new ServiceHost(typeof(BuildImpl));
            _serviceHost.AddServiceEndpoint(typeof(IBuild), new NetNamedPipeBinding(), ServerAddress);
        }

        public void Start()
        {
            try
            {
                _serviceHost.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动服务失败:\n{ex.Message}");
            }
        }

        public void Dispose()
        {
            try
            {
                _serviceHost.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止服务失败:\n{ex.Message}");
            }
        }
    }
}
