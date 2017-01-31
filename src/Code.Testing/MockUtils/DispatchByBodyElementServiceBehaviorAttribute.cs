using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Code.Testing.MockUtils
{
    public class DispatchByBodyElementServiceBehaviorAttribute : Attribute, IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            //
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            //
            foreach (var endpoint in serviceDescription.Endpoints)
            {
                endpoint.Contract.Behaviors.Add(new DispatchByBodyElementBehaviorAttribute());
            }
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            //
        }
    }
}
