using Microsoft.Reporting.WebForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Printing;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace HOOReport
{
    public partial class MyInvoice : System.Web.UI.Page
    {
        private string param;
        private string pref;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                param = Request.QueryString["P"];
                pref = Request.QueryString["pref"];
                ShowReport(param);
                PrintReport();
            }
        }
        
        private void PrintReport()
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            File.Delete(HttpContext.Current.Server.MapPath(pref + "output.pdf"));
            File.Delete(HttpContext.Current.Server.MapPath(pref + "Print.pdf"));
            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(pref + "output.pdf"), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            //Open existing PDF        
            Document document = new Document(PageSize.A4);//*Note
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath(pref + "output.pdf"));

            //Getting a instance of new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
               HttpContext.Current.Server.MapPath(pref + "Print.pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);

            float width = psize.Width;
            float height = psize.Height;

            //Add Page to new document
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;

                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }

            //Attach javascript to the document
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();

            //Attach pdf to the iframe
            frmPrint.Attributes["src"] = pref + "Print.pdf";
        }
        private void ShowReport(string param)
        {
            ReportViewer1.Reset();
            DataTable dt = GetData(param);
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.ReportPath = "Report1.rdlc";
            ReportViewer1.LocalReport.Refresh();
        }

        private DataTable GetData(string brandcode)
        {
            string query = string.Format("CALL `prod_syshoo_db`.`sp_dscr`('{0}', '{1}');", "TRNMA", "RR2");
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

        protected void Load_Click(object sender, EventArgs e)
        {
            ShowReport("aa");
        }




    }
}