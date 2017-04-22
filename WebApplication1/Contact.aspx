<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="WebApplication1.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h3>Contact Information</h3>
    <address>
        Sekretariat AAR <br />
        Jalan Ganesha no 10<br />
        <abbr title="Phone">P:</abbr>
        081824980023
    </address>

    <address>
        <strong>Support:</strong>   <a href="mailto:Support@example.com">support@AAR.com</a><br />
    </address>
</asp:Content>
