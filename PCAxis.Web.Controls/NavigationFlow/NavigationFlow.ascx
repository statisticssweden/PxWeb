<%@ control  inherits="PCAxis.Web.Controls.NavigationFlowCodebehind" %>

<asp:Panel ID="panelShowNavigationFlow" runat="server" Visible="true">
    <div class="navigationFlowArea">
        <div class="dottedlineLeft" ></div>
        <div class="dottedlineRight" ></div>
        <div class="row1">
            <asp:HyperLink ID="firstLink" runat="server" >
			    <asp:panel cssclass="col1 " id="firstItem" runat="server" >                
				    <div class="number" >
					    1
				    </div>
				    <div class="text" >
					   <asp:Label runat="server" ID="lblFirstStep" />
				    </div>
			    </asp:panel>
            </asp:HyperLink>
            <asp:HyperLink ID="secondLink" runat="server" >
			    <asp:panel cssclass="col2 " id="secondItem" runat="server"  >
				    <div class="number" >
					    2
				    </div>
				    <div class="text">
					    <asp:Label runat="server" ID="lblSecondStep" />
				    </div>
			    </asp:panel>
            </asp:HyperLink>           
			<asp:panel cssclass="col3 " id="thirdItem" runat="server" >
				<div class="number" >
					3
				</div>
				<div class="text" >
					<asp:Label runat="server" ID="lblThirdStep" />
				</div>
			</asp:panel>      
        </div>   
    </div>
    <div style="clear: both;"></div>
    <div class="fullscreen"></div>

</asp:Panel>


