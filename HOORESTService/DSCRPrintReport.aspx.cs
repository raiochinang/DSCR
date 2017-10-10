using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HOORESTService
{
    public partial class dscrPrintReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string prefix = UserBranch.Value.ToString();
            string rr = UserRR.Value.ToString();
            ShowReport(prefix, rr);
        }

        private void ShowReport(string prefix, string rr)
        {
            try
            {
                DataTable dt = GetData(prefix, rr);
                ReportDocument rpt = new ReportDocument();
                rpt.Load(Server.MapPath("~/dscrReport.rpt"));
                rpt.SetDataSource(dt);
                var x = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
                CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.HasToggleParameterPanelButton = false;
                CrystalReportViewer1.ReportSource = rpt;
                //rpt.PrintToPrinter(1, false, 0, 0);
                rpt.PrintToPrinter(1, true, 0, 0);
            }
            catch (Exception e) {
                throw e;
            }
        }

        private DataTable GetData(string prefix, string rr)
        {
            string query = string.Format("CALL `prod_syshoo_db`.`sp_dscr`('{0}', '{1}');", prefix, rr);
            DataTable dataTable = new DataTable();
            MySqlConnection connection;
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
            connection = new MySqlConnection(connStr);
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataTable.Load(dataReader);
            dataReader.Close();
            connection.Close();
            return dataTable;
        }
    }
}