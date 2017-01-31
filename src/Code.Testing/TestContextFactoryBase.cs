using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Code.Testing
{
    public abstract class TestContextFactoryBase
    {
        [ThreadStatic]
        protected static ITestContext _current = null;
        public static ITestContext Current { get { return _current; } internal set { _current = value; } }

        protected static TestContextFactoryBase _instance;
        public static TestContextFactoryBase Instance
        {
            get
            {
                return _instance;
            }
        }

        public abstract string TestContextPrefix { get; }
        public abstract ITestContextBuilder GetBuilderImpl();
        public static ITestContextBuilder GetBuilder()
        {
            return _instance.GetBuilderImpl();
        }
        public virtual ITestContext AttachImpl()
        {
            if (Current != null)
            {
                if(!Current.WithSuccess && Current.Exception != null)
                    throw Current.Exception;
                Thread.Sleep(Current.ResponseDelayMillis);
            }

            return Current;
        }
        public static ITestContext Attach()
        {
            return _instance.AttachImpl();
        }
    }
}
