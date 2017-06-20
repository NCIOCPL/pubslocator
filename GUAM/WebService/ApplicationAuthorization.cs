using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WebService
{
    public class ApplicationAuthorization : Attribute, IServiceBehavior
    {
        public ApplicationAuthorization()
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        void System.ServiceModel.Description.IServiceBehavior.ApplyDispatchBehavior(ServiceDescription desc, ServiceHostBase host)
        {
            foreach (ChannelDispatcher cDispatcher in host.ChannelDispatchers)
            {
                foreach (EndpointDispatcher eDispatcher in cDispatcher.Endpoints)
                {
                    eDispatcher.DispatchRuntime.MessageInspectors.Add(new ApplicationMessageBehavior());
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}