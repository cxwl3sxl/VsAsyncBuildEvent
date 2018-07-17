using System.ServiceModel;

namespace VsAsyncBuildEvent.Contact
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IBuild
    {
        [OperationContract]
        void Build(string cmd, string arguments);

        [OperationContract]
        bool AreYouOk();
    }
}
