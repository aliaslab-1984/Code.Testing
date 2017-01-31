using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Testing
{
    public interface ITestContextBuilder
    {
        ITestContextBuilder SetValue<T>(string key, T value);

        ITestContextBuilder ForSession(string sessionId);
        ITestContextBuilder WithDelayedResponse(int delayMillis);
        ITestContextBuilder WithSuccess();
        ITestContextBuilder WithFailure(string errorCode, string errorMessage);
        ITestContextBuilder WithException<T>() where T : Exception;

        ITestContext Build();
    }
}
