using Aspensys.GlobalUsers.WebServiceClient.UserService;
using GlobalUsers.DataHelper;
using System;
using System.Runtime.CompilerServices;

namespace ManagementApplication.Models
{
	public static class ApplicationExtension
	{
		public static Application ToApplicationInfo(this ApplicationInformation ai)
		{
			return ai.Copy<Application>();
		}
	}
}