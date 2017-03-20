using Aspensys.GlobalUsers.WebServiceClient;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace ManagementApplication
{
	public class ApplicationImpersonationMessageInspector : IClientMessageInspector
	{
		private string _ApplicationToImpersonate;

		public ApplicationImpersonationMessageInspector(string ApplicationToImpersonate)
		{
			this._ApplicationToImpersonate = ApplicationToImpersonate;
		}

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
		}

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			MessageHeader<string> ip_header = new MessageHeader<string>(this._ApplicationToImpersonate);
			request.Headers.Add(ip_header.GetUntypedHeader(ApplicationNameMessageInspector.APPLICATION_HEADER, ApplicationNameMessageInspector.APPLICATION_NAMESPACE));
			return null;
		}
	}
}