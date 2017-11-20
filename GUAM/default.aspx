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
							The use of this system is restricted to authorized users. Unauthorized access, use, or modification of this U.S. Government computer system, or of the data contained herein or in transit to/from this system, constitutes a violation of 18 U.S.C. §1030. This system is monitored to ensure proper performance of applicable security features and procedures. Such monitoring may result in the acquisition, recording and analysis of data being communicated, transmitted, processed or stored in this system by a user. In particular, we monitor the identity of all who access this system, as well as the date and time of their access. In addition, we review all user-submitted information and data. Unauthorized or improper use of this system may result in administrative disciplinary action, as well as civil and criminal penalties.
						</div>
						<div>
							By continuing to use this system, you indicate your awareness of and consent to these terms and conditions of use.
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