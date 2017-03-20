<%--<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GuamControl.ascx.cs" Inherits="GuamControl" %> 
<style type="text/css">
    fieldset
    {
        margin-bottom: 10px;
        border:0;
    }
    legend
    {
        padding: 0 2px;
        font-weight: bold;
    }
    label
    {
        display: inline-block;
        line-height: 1.8;
        vertical-align: top;
    }
    fieldset ol
    {
        margin: 0;
        padding: 0;
    }
    fieldset li
    {
        list-style: none;
        padding: 5px;
        margin: 0;
    }
    fieldset fieldset
    {
        border: none;
        margin: 3px 0 0;
    }
    fieldset fieldset legend
    {
        padding: 0 0 5px;
        font-weight: normal;
    }
    fieldset fieldset label
    {
        display: block;
        width: auto;
    }
    label
    {
        width: 175px; /* Width of labels */
    }

</style>
<div id="login">
    <p>
        <asp:Label ID="lblFailureMessage" Font-Bold="true" ForeColor="Red" runat="server" />
    </p>
    <asp:PlaceHolder ID="phLogin" runat="server">
        <fieldset>
            <legend></legend>
            <ol>
                <li>
                    <label>User Name:</label>
                    <asp:TextBox ID="txtUsername" runat="server" />
                </li>
                <li>
                    <label>Password:</label>
                    <asp:TextBox ID="txtPassword" AutoCompleteType="None" TextMode="Password" runat="server" />
                </li>
                <li id="liPersistent" visible="false" runat="server">
                    <asp:label ID="Label1" AssociatedControlID="chkPersistent" runat="server">Keep me logged in:</asp:label>
                    <asp:CheckBox id="chkPersistent" runat="server" />
                </li>
                <li>
                    <label>&nbsp;</label>
                    <asp:LinkButton ID="lbForgot" OnClick="lbForgot_Click" Text="Forgot password" runat="server" />
                </li>
                <li>
                    <label>&nbsp;</label>
                    <asp:LinkButton ID="lnkRegister" OnClick="lnkRegister_Click" Text="Register" runat="server" />
                </li>                
                <li>
                    <label>&nbsp;</label>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                </li>
            </ol>
        </fieldset>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phExpirationWarning" runat="server">
            <p>Your password will expire in <asp:Label ID="lblExpirationDays" runat="server" /> day(s).  Would you like to change it now?</p>
            <div style="text-align:center;">
                <asp:Button ID="btnChangePasswordNow" Text="Yes" CommandArgument="1" OnCommand="ChangePasswordNow" runat="server" />
                &nbsp;&nbsp;
                <asp:Button ID="btnChangePasswordLater" Text="No" CommandArgument="0" OnCommand="ChangePasswordNow" runat="server" />
            </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phChangePassword" runat="server">
        <p style="display:<%= CurrentMode == Mode.LOGIN ? "block" : "none" %>">
            You must change your password before accessing this site.
        </p>
        <fieldset>
            <legend></legend>
            <ol>
                <li>
                    <label>Current Password:<span style="color:red">*</span></label>
                    <asp:TextBox ID="txtCurrentPassword" AutoCompleteType="None" TextMode="Password" runat="server" />
                </li>
                <li>
                    <label>New Password:<span style="color:red">*</span></label>
                    <asp:TextBox ID="txtNewPassword" AutoCompleteType="None" TextMode="Password" runat="server" />
                </li>
                <li>
                    <label>Confirm New Password:<span style="color:red">*</span></label>
                    <asp:TextBox ID="txtConfirmPassword" AutoCompleteType="None" TextMode="Password" runat="server" />
                </li>
            </ol>
            <p>
                <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" OnClick="btnChangePassword_Click" />
            </p>
        </fieldset>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phSetQuestions" runat="server">
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                var hide_repeats = function () {
                    $('option').css('display', 'block');
                    var selected_options = $('select option:selected').map(function (i, obj) { return $(obj).val(); });
                    $('select').each(function (i) {
                        var current_select_value = $(this).val();
                        $(this).children().each(function (j) {
                            if (j == 0) return;
                            var current_option_value = $(this).val();
                            if ($.inArray(current_option_value, selected_options) != -1 && current_select_value != current_option_value) {
                                $(this).css('display', 'none');
                            }
                        });
                    });
                }
                hide_repeats();
                $('select').change(function () {
                    hide_repeats();
                });
            });
        </script>
        <p>
            Select <%= QuestionCount%> question<%= QuestionCount > 1 ? "s" : ""%> and provide <%= QuestionCount > 1 ? "" : "an "%>answer<%= QuestionCount > 1 ? "s" : ""%>.
        </p>
        <asp:ValidationSummary ShowSummary="true" ShowMessageBox="false" HeaderText="Fill in the following fields:" runat="server" />
        <fieldset>
            <legend></legend>
            <asp:Repeater id="repQuestions" runat="server">
                <ItemTemplate>
                    <li>
                        <asp:DropDownList id="ddlQuestions" DataSource='<%# Questions %>' DataValueField="QuestionID" DataTextField="QuestionText" AppendDataBoundItems="true" runat="server">
                            <asp:ListItem Text="Please select a question" Value="" />
                        </asp:DropDownList>
                        <asp:TextBox ID="txtQuestionAnswer" Width="300px" runat="server" /> 
                        <br />
                        <asp:RequiredFieldValidator ControlToValidate="ddlQuestions" ErrorMessage='<%# "Question " + Container.DataItem  %>' Display="None" runat="server" />    
                        <asp:RequiredFieldValidator ControlToValidate="txtQuestionAnswer" ErrorMessage='<%# "Answer " + Container.DataItem  %>' Display="None" runat="server" />       
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </fieldset>
        <p>
            <asp:Button ID="btnSetQuestions" runat="server" Text="Set Questions" OnClick="btnSetQuestions_Click" />
        </p>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phForgotPassword" runat="server">
        <asp:PlaceHolder ID="phSetUsername" Visible="<%# String.IsNullOrEmpty(txtUsername.Text) %>" runat="server">
            <fieldset>
                <ol>
                    <li>
                        <label for="username">User name:</label>
                        <asp:TextBox ID="txtUsernameForgot" runat="server" />
                    </li>
                    <li>
                        <label>&nbsp;</label>
                        <asp:Button ID="btnSetUsername" OnClick="btnSetUsername_Click" runat="server" Text="Reset Password" />
                    </li>
                </ol>
            </fieldset>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phAnswerQuestions" Visible="<%# !String.IsNullOrEmpty(txtUsername.Text) %>" runat="server">
            <asp:PlaceHolder ID="phQuestions" runat="server">
                 <fieldset>
                    <legend></legend>
                    <asp:Repeater id="repAnswers" runat="server">
                        <ItemTemplate>
                            <li>
                                <asp:HiddenField ID="hidUserQuestionID" Value='<%# DataBinder.Eval(Container.DataItem, "UserQuestionID") %>' runat="server" />
                                <asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "QuestionText") %>' runat="server" />
                                <asp:TextBox ID="txtQuestionAnswer" Width="300px" runat="server" /> 
                                <br />
                                <asp:RequiredFieldValidator ControlToValidate="txtQuestionAnswer" ErrorMessage='<%# "Answer " + Container.DataItem  %>' Display="None" runat="server" />       
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Button ID="btnResetPassword" OnClick="btnResetPassword_Click" runat="server" Text="Reset Password" />
                </fieldset>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phResetSuccessful" Visible="false" runat="server">
                <p>
                    The password for <%=txtUsername.Text %> has been successfully reset.  An email will be sent to the address on record for the account.  
                </p>
                <p>
                    Please return to the <asp:LinkButton ID="lbReturnLogin" OnClick="lbReturnLogin_Click" Text="login page" runat="server" />.
                </p>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phNoQuestions" Visible="false" runat="server">
                <p>
                    No questions have been defined for this user.
                </p>
                <p>
                    Please return to the <asp:LinkButton OnClick="lbReturnLogin_Click" Text="login page" runat="server" />.
                </p>
            </asp:PlaceHolder>
        </asp:PlaceHolder>
    </asp:PlaceHolder>
</div>
--%>