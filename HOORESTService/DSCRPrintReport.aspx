<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DSCRPrintReport.aspx.cs" Inherits="HOORESTService.dscrPrintReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Pages/styles/reset.css" rel="stylesheet" />
    <link href="Pages/kendo/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="Pages/kendo/styles/kendo.default.min.css" rel="stylesheet" />
    <link href="Pages/kendo/styles/kendo.default.mobile.min.css" rel="stylesheet" />
    <a href="DSCRPrintReport.aspx">DSCRPrintReport.aspx</a>
    <link href="Pages/styles/index.css" rel="stylesheet" />

    <script src='<%=ResolveUrl("~/crystalreportviewers13/js/crviewer/crv.js")%>' type="text/javascript"></script>
    <script src="Pages/kendo/js/jquery.min.js"></script>
    <script src="Pages/kendo/js/kendo.all.min.js"></script>
    <script src="Pages/scripts/print.js?v=1"></script>
    <script type="text/javascript">
        function Print() {
            var dvReport = document.getElementById("ReportContainer");
            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            }
            else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <input type="text" class="r-text" id="UserBranch" runat="server" style="float: left; width: 200px;" />
        </div>
        <div>
            <input type="text" class="r-text" id="UserRR" runat="server" placeholder="RR NUMBER" style="float: left; width: 200px;" />
        </div>
        <div>
            <asp:Button ID="btnPrint" runat="server" Text="Print" class="btnLogin k-button" OnClick="btnPrint_Click" />

        </div>
        <div style="margin-left: 0px; width: 700px; margin-top: 20px;" id="ReportContainer">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
        </div>
        <input type="button" value="Print" onclick="Print()" class="btnLogin k-button" />

        <!--window-->
        <div id="LoginWindow">
            <div class="windowHeader">
                Login
            </div>
            <div>
                <input type="text" class="r-text" id="LoginWindowUsername" placeholder="USERNAME" value="" />
            </div>
            <div>
                <input type="password" class="r-text" id="LoginWindowPassword" placeholder="PASSWORD" value="" />
            </div>
            <div>
                <button class="btnLogin" data-align="center" style="position: absolute; bottom: 4px; right: 10px;">LOG IN</button>
            </div>
        </div>
        <span id="notification"></span>
    </form>
</body>
</html>
