using Microsoft.Reporting.WebForms;
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
    public partial class PrintRR : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
               
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
          
            string prefix = UserBranch.Value.ToString();
            string rr = UserRR.Value.ToString();
            ShowReport(prefix, rr);
            
        }

        private void ShowReport(string prefix, string rr)
        {
            ReportViewer1.Reset();
            DataTable dt = GetData(prefix, rr);
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = "Report1.rdlc";
            ReportViewer1.LocalReport.Refresh();
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