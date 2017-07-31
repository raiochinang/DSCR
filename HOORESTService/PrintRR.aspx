<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintRR.aspx.cs" Inherits="HOORESTService.PrintRR" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Pages/styles/reset.css" rel="stylesheet" />
    <link href="Pages/kendo/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="Pages/kendo/styles/kendo.default.min.css" rel="stylesheet" />
    <link href="Pages/kendo/styles/kendo.default.mobile.min.css" rel="stylesheet" />
    <link href="Pages/styles/index.css" rel="stylesheet" />

    <script src="Pages/kendo/js/jquery.min.js"></script>
    <script src="Pages/kendo/js/kendo.all.min.js"></script>
    <script src="Pages/scripts/print.js?v=1"></script>
</head>
<body>

    <form id="form1" runat="server">        
        <div>
           
            <input type="text" class="r-text" id="UserBranch" runat="server" style="float: left; width: 200px;"/>
        </div>
        <div>
            <input type="text" class="r-text" id="UserRR" runat="server" placeholder="RR NUMBER" style="float: left; width: 200px;" />
        </div>
        <div>
            <asp:Button ID="btnPrint" runat="server" Text="Print" class="btnLogin k-button" OnClick="btnPrint_Click" />
            
        </div>          
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%">
                
            </rsweb:ReportViewer>
        </div>
        <!--window-->
        <div id="LoginWindow">
            <div class="windowHeader">
                Login
            </div>
            <div>
                <input type="text" class="r-text" id="LoginWindowUsername" placeholder="USERNAME" value="mjagapinan" />
            </div>
            <div>
                <input type="password" class="r-text" id="LoginWindowPassword" placeholder="PASSWORD" value="password12" />
            </div>
            <div>
                <button class="btnLogin" data-align="center" style="position: absolute; bottom: 4px; right: 10px;">LOG IN</button>
            </div>
        </div>
        <span id="notification"></span>
    </form>
</body>
</html>

