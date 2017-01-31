using CodeProbe.ExecutionControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Testing
{
    public abstract class TestContextBuilderBase : ITestContextBuilder
    {
        public const string SESSION_ID = "__test__.session.id";
        
        public const string REQ_DELAY = "__test__.request.delayMillis";
        public const string REQ_SUCCESS = "__test__.request.success";
        public const string REQ_RESULTCODE = "__test__.request.resultCode";
        public const string REQ_RESULTMESSAGE = "__test__.request.resultMessage";
        public const string REQ_EXCEPTIONTYPE = "__test__.request.exceptionType";
        
        public ITestContextBuilder SetValue<T>(string key, T value)
        {
            ExecutionControlManager.Current.SetCtxValue($"{TestContextFactoryBase.Instance.TestContextPrefix}.{key}", value);

            return this;
        }

        public ITestContextBuilder ForSession(string sessionId)
        {
            SetValue(SESSION_ID, sessionId);

            return this;
        }
                
        public ITestContextBuilder WithDelayedResponse(int delayMillis)
        {
            SetValue(REQ_DELAY, delayMillis);

            return this;
        }

        public ITestContextBuilder WithSuccess()
        {
            SetValue(REQ_SUCCESS, true);

            return this;
        }

        public ITestContextBuilder WithFailure(string errorCode, string errorMessage)
        {
            SetValue(REQ_SUCCESS, false);
            SetValue(REQ_RESULTCODE, errorCode);
            SetValue(REQ_RESULTMESSAGE, errorMessage);

            return this;
        }

        public ITestContextBuilder WithException<T>() where T : Exception
        {
            SetValue(REQ_SUCCESS, false);
            SetValue(REQ_EXCEPTIONTYPE, typeof(T).AssemblyQualifiedName);

            return this;
        }

        public abstract ITestContext Build();
    }
}
