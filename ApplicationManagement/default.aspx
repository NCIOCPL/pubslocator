<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ManagementApplication.Login" %>
<%@ Register Src="~/GuamControl.ascx" TagPrefix="uc" TagName="GuamControl" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <meta http-equiv="Pragma" content="no-cache" />
        <meta http-equiv="Cache-Control" content="no-cache,no-Store" />
        <title>Application Management</title>
        <link href="<%=ResolveClientUrl("~/Content/Site.css") %>" rel="stylesheet" type="text/css" />
        <link href="<%=ResolveClientUrl("~/Content/editor.css") %>" rel="stylesheet" type="text/css" />
    </head>

    <body>
        <div class="page">

            <div id="header">
                <div id="title">
                    <h1>GUAM Application Management</h1>
                </div>
              
                <div id="logindisplay">
                    &nbsp;
                </div> 
            
                <div id="menucontainer">
                </div>
            </div>

            <div id="main" style="">
				<div id="login">
					<form id="loginForm" runat="server">
						<uc:GuamControl id="GuamControl" runat="server" />
					</form>
					<div id="footer">
						<div>NOTICE TO USERS</div>
						<div>
							This warning banner provides privacy and security notices consistent with applicable federal laws, directives, and other federal guidance for accessing this Government system, which includes (1) this computer network, (2) all computers connected to this network, and (3) all devices and storage media attached to this network or to a computer on this network.
						</div>
						<div>
							This system is provided for Government-authorized use only.
						</div>
						<div>
							Unauthorized or improper use of this system is prohibited and may result in disciplinary action and/or civil and criminal penalties.
							Personal use of social media and networking sites on this system is limited as to not interfere with official work duties and is subject to monitoring.
						</div>
						<div>
							By using this system, you understand and consent to the following:
						</div>
						<div>
							The Government may monitor, record, and audit your system usage, including usage of personal devices and email systems for official duties or to conduct HHS business. Therefore, you have no reasonable expectation of privacy regarding any communication or data transiting or stored on this system. At any time, and for any lawful Government purpose, the government may monitor, intercept, and search and seize any communication or data transiting or stored on this system.
						</div>
						<div>
							Any communication or data transiting or stored on this system may be disclosed or used for any lawful Government purpose.
						</div>
						<p>
							<asp:Literal runat="server" ID="VersionStatement" />
						</p>
					</div>
				</div>
            </div>
        </div>
    </body>
</html>