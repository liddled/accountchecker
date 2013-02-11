using System.ServiceModel;

namespace DL.AccountChecker.Framework
{
    [ServiceContract]
    public interface IWorkflowService
    {
        void Start();
        void Stop();
    }
}
