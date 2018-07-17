using System;
using System.ServiceModel;
using System.Threading;
using VsAsyncBuildEvent.Contact;
using VsAsyncBuildEvent.Server;

namespace VsAsyncBuildEvent.Client
{
    public class NamedPipeClient
    {
        public static bool TryBuild(string cmd, string argument)
        {
            var factory = new ChannelFactory<IBuild>(new NetNamedPipeBinding(),
                new EndpointAddress(NamedPipeServer.ServerAddress));
            try
            {
                var build = factory.CreateChannel();
                build.Build(cmd, argument);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                try
                {
                    factory.Close();
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static void WaitServerStart()
        {
            while (true)
            {
                var factory = new ChannelFactory<IBuild>(new NetNamedPipeBinding(),
                    new EndpointAddress(NamedPipeServer.ServerAddress));
                try
                {
                    var build = factory.CreateChannel();
                    if (build.AreYouOk())
                        break;
                }
                catch
                { // ignored
                }
                finally
                {
                    Thread.Sleep(500);
                    try
                    {
                        factory.Close();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
    }
}
