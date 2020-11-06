<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="PXWeb.UserControls.Login" %>




    <asp:Login runat="server" ID="LoginControl" OnAuthenticate="LogIn" RenderOuterTable="false">
	    <LayoutTemplate>
		   

                <div id="button" class="box-top">
                    <asp:Button ID="ShowLoginSection" runat="server" Text="LOGG INN" onclick="LoginExpand" />
                    <asp:Label runat="server" Visible="false" ID="ShowLoginSectionHolder" Text="show"></asp:Label>
                </div>

                <div class="box" id="LoginSection" runat="server" visible="false">
                  <fieldset>
                  <legend>Logg inn for å se upubliserte tall</legend>
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
    			                <asp:Button ID="Login" runat="server" Text="Logg inn" CommandName="Login"/>
                            <a class="pxweb-link px-login-hjelp" href="https://wiki.ssb.no/x/JwLCB">
                            <span class="link-text">Veiledning</span>
                                </a>
                                </div>
                            <div class="row">
                   		        <asp:Label ID="FailureText" runat="server" CssClass="login-form-failure" ></asp:Label>
                            </div>
                         </div>
                            </asp:Panel>
                    </div>
                      </fieldset>
                </div>

                <div id="LoggedInDiv" runat="server" class="wrap">
                    <asp:Label ID="LoggedInAs" runat="server" Text="" ></asp:Label>
                </div>

                <div id="LoggedOutDiv" class="wrap">
                    <asp:Label ID="lblLoggedOutMessage" Visible="false" runat="server" ></asp:Label>
                </div>

		  
	    </LayoutTemplate>
    </asp:Login>



