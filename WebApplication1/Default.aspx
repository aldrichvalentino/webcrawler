<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Web Crawl</h3>
    <asp:Label ID="SearchLabel" runat="server" Text="Input your keyword: "></asp:Label>
    <asp:TextBox ID="TextBox" runat="server"></asp:TextBox>
    <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="Page_Load"/>

    <div style="max-height:350px; overflow:auto">
        <asp:GridView ID="theRss" runat="server" AutoGenerateColumns="false" ShowHeader="false" Width="90%" BorderWidth="0">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <table width="100%" border="0" cellpadding="0" cellspacing="5">
                            <tr>
                                <td>
                                    <h3 style="color:#3E7CFF"><%#Eval("Title") %></h3>
                                </td> 
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                    <%#Eval("Description") %>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td align="right">
                                    <a href='<%#Eval("Link") %>' target="_blank">Read More...</a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    
</asp:Content>
