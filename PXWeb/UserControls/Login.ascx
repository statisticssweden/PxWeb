<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="PXWeb.UserControls.Login" %>




    <asp:Login runat="server" ID="LoginControl" OnAuthenticate="LogIn" RenderOuterTable="false">
	    <LayoutTemplate>
		    <fieldset>

                <div id="button" class="box-top">
                    <asp:Button ID="ShowLoginSection" runat="server" Text="LOGG INN" onclick="LoginExpand" />
                    <asp:Label runat="server" Visible="false" ID="ShowLoginSectionHolder" Text="show"></asp:Label>
                </div>

                <div class="box" id="LoginSection" runat="server" visible="false">
                    <div  class="wrap">
                        <asp:Panel runat="server" DefaultButton ="Login">
			            <div>
				            <asp:Label ID="lblUserName" AssociatedControlID="UserName"  runat="server" Text="Username" />
                            <asp:TextBox ID="UserName" runat="server" style="margin-bottom:10px"></asp:TextBox>
			            </div>
			            <div>
				            <asp:Label ID="lblPassword"  AssociatedControlID="Password" runat="server" Text="Password" />
				            <asp:TextBox ID="Password" runat="server" TextMode="Password" />
			            </div>
                        <div id="login-form-buttons">
                            <div class="row">
    			                <asp:Button ID="Login" runat="server" Text="Logg inn" CommandName="Login"/><asp:ImageButton  ID="loginHelp" runat="server" style="margin-right:12px" Height="15px" Width="15px" OnClientClick="var win = window.open('https://wiki.ssb.no/x/JwLCB', '_blank');win.focus();return false;" ImageUrl="<%$ PxImage: questionmark.gif %>"/>
                            </div>
                            <div class="row">
                   		        <asp:Label ID="FailureText" runat="server" CssClass="login-form-failure" ></asp:Label>
                            </div>
                         </div>
                            </asp:Panel>
                    </div>
                </div>

                <div id="LoggedInDiv" runat="server" class="wrap">
                    <asp:Label ID="LoggedInAs" runat="server" Text="" ></asp:Label>
                </div>

		    </fieldset>
	    </LayoutTemplate>
    </asp:Login>



