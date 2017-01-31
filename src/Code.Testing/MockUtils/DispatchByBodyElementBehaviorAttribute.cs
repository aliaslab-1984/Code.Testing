using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Code.Testing.MockUtils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    sealed class DispatchByBodyElementBehaviorAttribute : Attribute, IContractBehavior
    {
        #region IContractBehavior Members

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            return;
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime)
        {
            List<string> operations = new List<string>();
            foreach (OperationDescription operationDescription in contractDescription.Operations)
            {
                operations.Add(operationDescription.Name);
            }

            dispatchRuntime.OperationSelector = new DispatchByBodyElementOperationSelector(operations, dispatchRuntime.UnhandledDispatchOperation.Name);
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
