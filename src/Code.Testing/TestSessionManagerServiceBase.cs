using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Testing
{
    public abstract class TestSessionManagerServiceBase : ITestSessionManagerService
    {
        public abstract void CleanUp(string sessionId);

        public virtual string Initialize()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
