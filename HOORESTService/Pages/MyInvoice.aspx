<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyInvoice.aspx.cs" Inherits="HOOReport.MyInvoice" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <iframe id="frmPrint" name="IframeName" width="1200" height="500" runat="server" style="display: none" runat="server"></iframe>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>            
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="464px" Width="767px">
                <LocalReport ReportPath="Report1.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DataSet1" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySqlConnectionString %>" ProviderName="<%$ ConnectionStrings:MySqlConnectionString.ProviderName %>" SelectCommand="sp_dscr" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="pre" Type="String" />
                    <asp:Parameter Name="rr" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
