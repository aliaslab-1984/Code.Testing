using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Testing
{
    public interface ITestContext : IDisposable
    {
        T GetValue<T>(string key, T defaultValue);

        string SessionId { get; }
        int ResponseDelayMillis { get; }
        bool WithSuccess { get; }
        string ResultCode { get; }
        string ResultMessage { get; }
        Exception Exception { get; }
    }
}
