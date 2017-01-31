using CodeProbe.ExecutionControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Testing
{
    public abstract class TestContextBase : ITestContext
    {
        protected IDisposable _innerCtx;
        public TestContextBase()
        {
            _innerCtx = ExecutionControlManager.GetScope();
            TestContextFactoryBase.Current = this;
        }

        public Exception Exception
        {
            get
            {
                string type = GetValue<string>(TestContextBuilderBase.REQ_EXCEPTIONTYPE);
                try
                {
                    if (string.IsNullOrEmpty(type))
                        return null;
                    else
                        return (Exception)Activator.CreateInstance(Type.GetType(type));
                }
                catch (Exception e)
                {
                    return new Exception(e.Message);
                }
            }
        }

        public int ResponseDelayMillis
        {
            get
            {
                return GetValue<int>(TestContextBuilderBase.REQ_DELAY, 0);
            }
        }

        public string ResultCode
        {
            get
            {
                return GetValue<string>(TestContextBuilderBase.REQ_RESULTCODE, WithSuccess ? "OK" : "DEFAULT-ERR");
            }
        }

        public string ResultMessage
        {
            get
            {
                return GetValue<string>(TestContextBuilderBase.REQ_RESULTMESSAGE, WithSuccess ? "OK" : "DEFAULT-ERR");
            }
        }

        public string SessionId
        {
            get
            {
                return GetValue<string>(TestContextBuilderBase.SESSION_ID);
            }
        }

        public bool WithSuccess
        {
            get
            {
                return GetValue<bool>(TestContextBuilderBase.REQ_SUCCESS, true);
            }
        }
        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            key = $"{TestContextFactoryBase.Instance.TestContextPrefix}.{key}";
            return ExecutionControlManager.Current.GetCtxKeys().Contains(key) ? ExecutionControlManager.Current.GetCtxValue<T>(key) : defaultValue;
        }

        public void Dispose()
        {
            TestContextFactoryBase.Current = null;
            _innerCtx.Dispose();
        }
    }
}
