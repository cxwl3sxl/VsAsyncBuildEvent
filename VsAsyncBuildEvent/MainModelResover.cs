using VsAsyncBuildEvent.Model;

namespace VsAsyncBuildEvent
{
    public class MainModelResover
    {
        private static readonly MainModel MainModel;
        static MainModelResover()
        {
            MainModel = new MainModel();
        }

        public MainModel Model => MainModel;

        public static MainModel StaticMainModel => MainModel;
    }
}
