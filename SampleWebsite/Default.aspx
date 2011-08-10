<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Navigation</h2>
    <p>
        <a href="/index/">Index (should fire RenderIndex)</a><br />
        <a href="/about/">About (should fire RenderAbout)</a><br />
        <a href="/dummy/">Dummy (not mapped)</a><br />
    </p>

    <h2>Output</h2>
    <p>
        <asp:Label runat="server" ID="lblInfo" />
    </p>

    <p>
        <asp:TextBox runat="server" ID="txtInfo" TextMode="MultiLine" Rows="15" Width="50%"></asp:TextBox>
    </p>
</asp:Content>
