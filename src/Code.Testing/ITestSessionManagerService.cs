using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Code.Testing
{
    [ServiceContract]
    public interface ITestSessionManagerService
    {
        [OperationContract]
        string Initialize();

        [OperationContract]
        void CleanUp(string sessionId);
    }
}
