using Aspensys.GlobalUsers.WebServiceClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ManagementApplication
{
	public class ApplicationImpersonationEndpointBehavior : IEndpointBehavior
	{
		private string _ApplicationToImpersonate;

		public ApplicationImpersonationEndpointBehavior(string ApplicationToImpersonate)
		{
			this._ApplicationToImpersonate = ApplicationToImpersonate;
		}

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			ApplicationNameMessageInspector app_name_inspector = clientRuntime.MessageInspectors.OfType<ApplicationNameMessageInspector>().FirstOrDefault<ApplicationNameMessageInspector>();
			if (app_name_inspector != null)
			{
				clientRuntime.MessageInspectors.Remove(app_name_inspector);
			}
			clientRuntime.MessageInspectors.Add(new ApplicationImpersonationMessageInspector(this._ApplicationToImpersonate));
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
		}

		public void Validate(ServiceEndpoint endpoint)
		{
		}
	}
}