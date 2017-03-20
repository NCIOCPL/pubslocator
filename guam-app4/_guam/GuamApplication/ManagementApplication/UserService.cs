using Aspensys.GlobalUsers.WebServiceClient;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ManagementApplication
{
	public static class UserService
	{
		public static UserServiceClient GetManagementClient(string ApplicationToImpersonate)
		{
			UserServiceClient client = new UserServiceClient();
			client.Endpoint.Behaviors.Add(new ApplicationImpersonationEndpointBehavior(ApplicationToImpersonate));
			return client;
		}
	}
}