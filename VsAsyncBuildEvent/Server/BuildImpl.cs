using VsAsyncBuildEvent.Contact;

namespace VsAsyncBuildEvent.Server
{
    public class BuildImpl : IBuild
    {
        public void Build(string cmd, string arguments)
        {
            MainModelResover.StaticMainModel.Build(cmd, arguments);
        }

        public bool AreYouOk()
        {
            return MainModelResover.StaticMainModel.IsServiceOk;
        }
    }
}
