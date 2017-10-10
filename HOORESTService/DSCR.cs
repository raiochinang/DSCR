using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Spire.Xls;
using Newtonsoft.Json;

namespace HOORESTService
{
    public class DSCR
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string prefix { get; set; }
        [DataMember]
        public string rr_number { get; set; }
        [DataMember]
        public string si_number { get; set; }
        [DataMember]
        public string or_number { get; set; }
        [DataMember]
        public int branch_id { get; set; }
        [DataMember]
        public string branch_name { get; set; }
        [DataMember]
        public string branch_code { get; set; }
        [DataMember]
        public int doctor_id { get; set; }
        [DataMember]
        public string doctor_name { get; set; }
        [DataMember]
        public int nurse_id { get; set; }
        [DataMember]
        public string nurse_name { get; set; }
        [DataMember]
        public int patient_id { get; set; }
        [DataMember]
        public string patient_name { get; set; }
        [DataMember]
        public string trx_date { get; set; }
        [DataMember]
        public string trx_time_from { get; set; }
        [DataMember]
        public string trx_time_to { get; set; }
        [DataMember]
        public decimal cash { get; set; }
        [DataMember]
        public decimal ar_amount { get; set; }
        [DataMember]
        public decimal payment { get; set; }
        [DataMember]
        public int cashier_id { get; set; }
        [DataMember]
        public string cashier_name { get; set; }
        [DataMember]
        public int current_user_id { get; set; }
        [DataMember]
        public List<Detail> details { get; set; }
        [DataMember]
        public List<Check> checks { get; set; }
        [DataMember]
        public decimal totalchecks { get; set; }
        [DataMember]
        public List<CreditCard> creditcards { get; set; }
        [DataMember]
        public decimal totalcreditcards { get; set; }
        [DataMember]
        public string trx_date_to { get; set; }
        [DataMember]
        public string trx_date_from { get; set; }
        [DataMember]
        public bool resultMessage { get; set; }
    }

    public class Detail
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int item_id { get; set; }
        [DataMember]
        public string item_name { get; set; }
        [DataMember]
        public string prefix { get; set; }
        [DataMember]
        public string rr_number { get; set; }
        [DataMember]
        public decimal gross { get; set; }
        [DataMember]
        public string quantity { get; set; }
        [DataMember]
        public string session { get; set; }
        [DataMember]
        public decimal discount { get; set; }
        [DataMember]
        public decimal deduction { get; set; }
        [DataMember]
        public decimal advance_payment { get; set; }
        [DataMember]
        public decimal program_availed { get; set; }
        [DataMember]
        public decimal add_ons { get; set; }
        [DataMember]
        public decimal net_sales { get; set; }
        [DataMember]
        public string explanation { get; set; }
    }

    public class ResultDSCR
    {
        public string quantity { get; set; }
        public string item_name_fld { get; set; }
        public string trx_date { get; set; }
        public string trx_time_from { get; set; }
        public string trx_time_to { get; set; }
        public string ar_amount { get; set; }
        public string cash { get; set; }
        public string net_sales { get; set; }        
        public string doctor_name { get; set; }
        public string nurse_name { get; set; }
        public string cashier_name { get; set; }
        public string patient_name { get; set; }
        public string Card { get; set; }
        public string Check { get; set; }
        public string CreditCard { get; set; }        
    }


    public partial class DSCRs
    {
        private int rowCount = 0;
        private double totalNetSale = 0.0;
        private double netOfVat = 0.0;
        private double retention = 0.0;
        private double totalAmountOfSharing = 0.0;
        private double MDShare60 = 0.0;
        private static readonly DSCRs _instance = new DSCRs();
        private DSCRs() { }
        private string folder = System.Configuration.ConfigurationManager.AppSettings["ReportFolder"];
        public static DSCRs Instance
        {
            get { return _instance; }
        }
        public DSCR GetDSCR(DSCR dscr)
        {
            DSCR result = new DSCR();
            MySQL m = new MySQL();
            string fullRR = dscr.prefix + "-" + dscr.rr_number;
            List<Detail> details = new List<Detail>();
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_dscr`('{0}', '{1}');", dscr.prefix, dscr.rr_number);
            System.Data.DataTable data = m.Select(sql);
            result.resultMessage = false;
            if (data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    result.prefix = row["prefix"].ToString();
                    result.rr_number = row["rr_number"].ToString();
                    result.si_number = row["si_number"].ToString();
                    result.or_number = row["or_number"].ToString();
                    result.doctor_id = Convert.ToInt32(row["doctor_id"]);
                    result.doctor_name = row["doctor_name"].ToString();
                    result.branch_id = Convert.ToInt32(row["branch_id"]);
                    result.branch_name = row["branch_name"].ToString();
                    result.nurse_id = Convert.ToInt32(row["nurse_id"]);
                    result.nurse_name = row["nurse_name"].ToString();
                    result.patient_id = Convert.ToInt32(row["patient_id"]);
                    result.patient_name = row["patient_name"].ToString();
                    result.cashier_id = Convert.ToInt32(row["cashier_id"]);
                    result.cashier_name = row["cashier_name"].ToString();
                    result.trx_date = row["trx_date"].ToString();
                    result.trx_time_from = row["trx_time_from"].ToString();
                    result.trx_time_to = row["trx_time_to"].ToString();
                    result.cash = Convert.ToDecimal(row["cash"]);
                    result.ar_amount = Convert.ToDecimal(row["ar_amount"]);
                    result.payment = Convert.ToDecimal(row["payment"]);
                    if (row["Check"] == System.DBNull.Value)
                    {
                        result.totalchecks = 0;
                    }
                    else
                    {
                        result.totalchecks = Convert.ToDecimal(row["Check"]);
                    }
                    if (row["Card"] == System.DBNull.Value)
                    {
                        result.totalcreditcards = 0;
                    }
                    else
                    {
                        result.totalcreditcards = Convert.ToDecimal(row["Card"]);
                    }

                    result.resultMessage = true;

                    Detail detail = new Detail
                    {
                        prefix = row["prefix"].ToString(),
                        rr_number = row["rr_number"].ToString(),
                        add_ons = Convert.ToDecimal(row["add_ons"]),
                        advance_payment = Convert.ToDecimal(row["advance_payment"]),
                        deduction = Convert.ToDecimal(row["deduction"]),
                        discount = Convert.ToDecimal(row["discount"]),
                        explanation = row["explanation"].ToString(),
                        gross = Convert.ToDecimal(row["gross"]),
                        item_id = Convert.ToInt32(row["item_id"]),
                        item_name = row["item_name_fld"].ToString(),
                        quantity = row["quantity"].ToString(),
                        session = row["session"].ToString(),
                        net_sales = Convert.ToDecimal(row["net_sales"]),
                        program_availed = Convert.ToDecimal(row["program_availed"]),
                    };
                    details.Add(detail);
                }
                result.details = details;

            }

            //check
            sql = string.Format("CALL `prod_syshoo_db`.`sp_dscr_checks`('{0}');", fullRR);
            data = m.Select(sql);

            if (data.Rows.Count > 0)
            {
                List<Check> checks = new List<Check>();
                foreach (DataRow row in data.Rows)
                {
                    Check check = new Check();
                    check.bank_name = row["bank_name"].ToString();
                    check.date_of_check = row["date_of_check"].ToString();
                    check.check_number = row["check_number"].ToString();
                    check.amount = row["amount"].ToString();
                    checks.Add(check);
                }
                result.checks = checks;
            }

            //Credit Card
            sql = string.Format("CALL `prod_syshoo_db`.`sp_dscr_credit_cards`('{0}');", fullRR);
            data = m.Select(sql);

            if (data.Rows.Count > 0)
            {
                List<CreditCard> creditcards = new List<CreditCard>();
                foreach (DataRow row in data.Rows)
                {
                    CreditCard creditcard = new CreditCard();
                    creditcard.name = row["Name"].ToString();
                    creditcard.term = row["term"].ToString();
                    creditcard.note = row["note"].ToString();
                    creditcard.amount = row["amount"].ToString();
                    creditcards.Add(creditcard);
                }
                result.creditcards = creditcards;
            }

            return result;
        }

        public string DSCRPrint(DSCR dscr)
        {   
            MySQL m = new MySQL();         
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_dscrprint`('{0}', '{1}');", dscr.prefix, dscr.rr_number);
            System.Data.DataTable data = m.Select(sql);
            List<ResultDSCR> ResultsDSCR = new List<ResultDSCR>();
            foreach (DataRow row in data.Rows)
            {
                ResultDSCR item = new ResultDSCR();                
                item.quantity = row["quantity"].ToString();
                item.item_name_fld = row["item_name_fld"].ToString();
                item.trx_date = row["trx_date"].ToString();
                item.trx_time_from = row["trx_time_from"].ToString();
                item.trx_time_to = row["trx_time_to"].ToString();
                item.ar_amount = row["ar_amount"].ToString();
                item.cash = row["cash"].ToString();
                item.net_sales = row["net_sales"].ToString();
                item.doctor_name = row["doctor_name"].ToString();
                item.nurse_name = row["nurse_name"].ToString();
                item.cashier_name = row["cashier_name"].ToString();
                item.patient_name = row["patient_name"].ToString();
                item.Card = row["Card"].ToString();
                item.Check = row["Check"].ToString();
                item.CreditCard = row["CreditCard"].ToString();                
                ResultsDSCR.Add(item);
            }

            return JsonConvert.SerializeObject(ResultsDSCR);           
        }

        public DSCR GetDSCRbyRange(DSCR dscr)
        {
            DSCR result = new DSCR();
            MySQL m = new MySQL();
            string fullRR = dscr.prefix + "-" + dscr.rr_number;
            List<Detail> details = new List<Detail>();
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_dscr_by_date_range`('{0}', '{1}');", dscr.trx_date_from, dscr.trx_date_to);

            System.Data.DataTable data = m.Select(sql);
            if (data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    result.prefix = row["prefix"].ToString();
                    result.rr_number = row["rr_number"].ToString();
                    result.si_number = row["si_number"].ToString();
                    result.or_number = row["or_number"].ToString();
                    result.doctor_id = Convert.ToInt32(row["doctor_id"]);
                    result.doctor_name = row["doctor_name"].ToString();
                    result.branch_id = Convert.ToInt32(row["branch_id"]);
                    result.branch_name = row["branch_name"].ToString();
                    result.nurse_id = Convert.ToInt32(row["nurse_id"]);
                    result.nurse_name = row["nurse_name"].ToString();
                    result.patient_id = Convert.ToInt32(row["patient_id"]);
                    result.patient_name = row["patient_name"].ToString();
                    result.cashier_id = Convert.ToInt32(row["cashier_id"]);
                    result.cashier_name = row["cashier_name"].ToString();
                    result.trx_date = row["trx_date"].ToString();
                    result.trx_time_from = row["trx_time_from"].ToString();
                    result.trx_time_to = row["trx_time_to"].ToString();
                    result.cash = Convert.ToDecimal(row["cash"]);
                    result.ar_amount = Convert.ToDecimal(row["ar_amount"]);
                    result.payment = Convert.ToDecimal(row["payment"]);
                    result.totalchecks = Convert.ToDecimal(row["Check"]);
                    result.totalcreditcards = Convert.ToDecimal(row["Card"]);

                    Detail detail = new Detail
                    {
                        rr_number = row["rr_number"].ToString(),
                        prefix = row["prefix"].ToString(),
                        add_ons = Convert.ToDecimal(row["add_ons"]),
                        advance_payment = Convert.ToDecimal(row["advance_payment"]),
                        deduction = Convert.ToDecimal(row["deduction"]),
                        discount = Convert.ToDecimal(row["discount"]),
                        explanation = row["explanation"].ToString(),
                        gross = Convert.ToDecimal(row["gross"]),
                        item_id = Convert.ToInt32(row["item_id"]),
                        item_name = row["item_name_fld"].ToString(),
                        quantity = row["quantity"].ToString(),
                        session = row["session"].ToString(),
                        net_sales = Convert.ToDecimal(row["net_sales"]),
                        program_availed = Convert.ToDecimal(row["program_availed"]),
                    };
                    details.Add(detail);
                }
                result.details = details;

            }

            return result;
        }
        public List<Detail> DSCRDetails(string rr_number)
        {
            MySQL m = new MySQL();
            List<Detail> details = new List<Detail>();
            string sql = string.Format("select * from prod_syshoo_db.dscr_details where rr_number = '{0}';", rr_number);
            System.Data.DataTable data = m.Select(sql);
            foreach (DataRow row in data.Rows)
            {
                Detail item = new Detail
                {
                    id = Convert.ToInt32(row["id"]),
                    rr_number = row["rr_number"].ToString(),
                    gross = Convert.ToDecimal(row["gross"]),
                    quantity = row["quantity"].ToString(),
                    session = row["session"].ToString(),
                    discount = Convert.ToDecimal(row["discount"]),
                    deduction = Convert.ToDecimal(row["deduction"]),
                    advance_payment = Convert.ToDecimal(row["advance_payment"]),
                    program_availed = Convert.ToDecimal(row["program_availed"]),
                    add_ons = Convert.ToDecimal(row["add_ons"]),
                    net_sales = Convert.ToDecimal(row["net_sales"]),
                    explanation = row["explanation"].ToString()
                };
                details.Add(item);
            }
            return details;
        }
        public DSCR Insert(DSCR dscr)
        {
            MySQL m = new MySQL();
            string fullRR = dscr.prefix.Trim() + "-" + dscr.rr_number.Trim();
            StringBuilder master = new StringBuilder();

            string trx_date = dscr.trx_date.Substring(0, dscr.trx_date.Length - 1);

            //dscr
            master.Append("INSERT INTO `prod_syshoo_db`.`dscr` (`prefix`, `rr_number`,`si_number`,`or_number`,`branch_id`,`doctor_id`,`nurse_id`,`patient_id`,`trx_date`,`trx_time_from`,`trx_time_to`,`cash`,`ar_amount`,`payment`, `cashier_id`) VALUES ");
            master.AppendFormat("('{0}', '{1}', '{2}', '{3}', ", dscr.prefix, dscr.rr_number, dscr.si_number, dscr.or_number);
            master.AppendFormat("'{0}', {1}, {2}, {3}, ", dscr.branch_id, dscr.doctor_id, dscr.nurse_id, dscr.patient_id);
            master.AppendFormat("'{0}', '{1}', '{2}', '{3}', ", trx_date, dscr.trx_time_from, dscr.trx_time_to, dscr.cash);
            master.AppendFormat("'{0}', '{1}', {2});", dscr.ar_amount, dscr.payment, dscr.cashier_id);
            m.Insert(master.ToString());

            //detail
            if (dscr.details.Count > 0)
            {
                StringBuilder detail = new StringBuilder();
                detail.Append("INSERT INTO `prod_syshoo_db`.`dscr_details`(`prefix`, `rr_number`,`gross`,`quantity`,`session`,`discount`,`deduction`,`advance_payment`,`program_availed`,`add_ons`,`net_sales`,`explanation`, `item_id` ) VALUES ");
                foreach (Detail i in dscr.details)
                {
                    detail.AppendFormat("('{0}', '{1}', '{2}', {3}, {4}, ", dscr.prefix, dscr.rr_number, i.gross, i.quantity, i.session);
                    detail.AppendFormat("'{0}', '{1}', '{2}', '{3}', ", i.discount, i.deduction, i.advance_payment, i.program_availed);
                    detail.AppendFormat("'{0}', '{1}', '{2}', {3}),", i.add_ons, i.net_sales, i.explanation, i.item_id);
                }
                string detail_query = detail.ToString();
                detail_query = detail_query.Substring(0, detail_query.Length - 1);
                detail_query += ";";
                m.Insert(detail_query);
            }

            //check
            if (dscr.checks.Count > 0)
            {
                StringBuilder check = new StringBuilder();

                check.Append("INSERT INTO `prod_syshoo_db`.`dscr_check`(`rr_number`,`bank_name`,`date_of_check`,`check_number`,`amount`) VALUES ");
                foreach (Check c in dscr.checks)
                {
                    check.AppendFormat("('{0}','{1}','{2}','{3}','{4}'),", fullRR, c.bank_name, c.date_of_check, c.check_number, c.amount);
                }
                string check_query = check.ToString();
                check_query = check_query.Substring(0, check_query.Length - 1);
                check_query += ";";
                m.Insert(check_query);
            }

            //creditcard
            if (dscr.creditcards.Count > 0)
            {
                StringBuilder creditcard = new StringBuilder();
                creditcard.Append("INSERT INTO `prod_syshoo_db`.`dscr_credit_card` (`rr_number`,`name`,`term`,`amount`,`note`) VALUES ");
                foreach (CreditCard cc in dscr.creditcards)
                {
                    creditcard.AppendFormat("('{0}','{1}','{2}','{3}','{4}'),", fullRR, cc.name, cc.term, cc.amount, cc.note);
                }
                string creditcard_query = creditcard.ToString();
                creditcard_query = creditcard_query.Substring(0, creditcard_query.Length - 1);
                creditcard_query += ";";
                m.Insert(creditcard_query);
            }




            //Add Log
            string log = string.Format("INSERT INTO `prod_syshoo_db`.`dscr_logs`(`code`, `user_id`,`time`) VALUES ('{0}', '{1}', NOW());", fullRR, dscr.current_user_id);
            m.Insert(log);
            return dscr;
        }
        public DSCR Update(DSCR dscr)
        {
            MySQL m = new MySQL();
            string fullRR = dscr.prefix.Trim() + "-" + dscr.rr_number.Trim();

            //dscr
            string sql = string.Format("DELETE FROM `prod_syshoo_db`.`dscr` WHERE `prefix` = '{0}' AND `rr_number` = '{1}'", dscr.prefix.Trim(), dscr.rr_number.Trim());
            m.Delete(sql);

            //details
            sql = string.Format("DELETE FROM `prod_syshoo_db`.`dscr_details` WHERE `prefix` = '{0}' AND `rr_number` = '{1}'", dscr.prefix.Trim(), dscr.rr_number.Trim());
            m.Delete(sql);

            //check
            sql = string.Format("DELETE FROM `prod_syshoo_db`.`dscr_check` WHERE `rr_number` = '{0}'", fullRR);
            m.Delete(sql);

            //creditcard
            sql = string.Format("DELETE FROM `prod_syshoo_db`.`dscr_credit_card` WHERE `rr_number` = '{0}'", fullRR);
            m.Delete(sql);

            //insert new dscr
            Insert(dscr);
            return dscr;
        }
        public string MDShare(DSCR param)
        {
            try
            {
                MySQL m = new MySQL();
                int branch_id = param.branch_id;
                string datefrom = param.trx_date_from;
                string dateto = param.trx_date_to;
                string branch_name = param.branch_name;

                string sql = string.Format("CALL `prod_syshoo_db`.`sp_mdshare`({0}, '{1}', '{2}');", branch_id, datefrom, dateto);
                System.Data.DataTable data = m.Select(sql);
                sql = "select id, code_fld, value_fld from prod_syshoo_db.hoo_setupcodes_tbl where category_fld = 'item_group' order by code_fld;";
                System.Data.DataTable CategoryData = m.Select(sql);

                Spire.Xls.Workbook wb = new Spire.Xls.Workbook();

                //Initailize worksheet
                Spire.Xls.Worksheet sheet = wb.Worksheets[0];

                var Doctors = data.AsEnumerable().Select(r => new
                {
                    doctor_id = r.Field<int>("doctor_id"),
                    doctor_name = r.Field<string>("doctor_name")
                }).Distinct();

                int wbCount = wb.Worksheets.Count() - 1;
                int DrCount = Doctors.Count();

                int i = 0;
                foreach (var item in Doctors)
                {
                    rowCount = 7;
                    var dr_name = item.doctor_name;
                    if (item.doctor_name.Length >= 29)
                    {
                        dr_name = item.doctor_name.Substring(0, 29);
                    }

                    if (wbCount >= i)
                    {
                        //rename
                        Worksheet sh = wb.Worksheets[i];
                        sh.Name = dr_name;
                    }
                    else
                    {
                        //add
                        wb.Worksheets.Add(dr_name);
                    }

                    sheet = wb.Worksheets[i];
                    ExcelColumnMDShare(sheet, dr_name, param.branch_name, wb);

                    foreach (DataRow category in CategoryData.Rows)
                    {

                        var PerGroupData = data.AsEnumerable().Where(row => (row.Field<Int32>("item_group") == category.Field<Int32>("id")) && (row.Field<int>("doctor_id") == item.doctor_id));
                        if (PerGroupData.Count() > 0)
                        {
                            totalNetSale = 0.0;
                            netOfVat = 0.0;
                            retention = 0.0;
                            totalAmountOfSharing = 0.0;
                            PerGroup(sheet, PerGroupData, category.Field<string>("value_fld"), category.Field<string>("code_fld"));
                            rowCount += 4;
                        }
                    }

                    sheet.Columns[9].NumberFormat = "#,##0.00";
                    sheet.Columns[10].NumberFormat = "#,##0.00";
                    sheet.Columns[11].NumberFormat = "#,##0.00";
                    sheet.Columns[12].NumberFormat = "#,##0.00";
                    sheet.Columns[13].NumberFormat = "#,##0.00";
                    sheet.AllocatedRange.AutoFitColumns();
                    sheet.AllocatedRange.AutoFitRows();

                    i += 1;

                }

                string dateStamp = DateTime.Now.ToString("MMddyyyy_HHmmss");
                string file = folder.ToString() + branch_name + "_MDSHARE" + dateStamp + ".xlsx";

                wb.SaveToFile(@file, ExcelVersion.Version2013);

                return file;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        private void ExcelColumnMDShare(Worksheet ws, string DoctorName, string BranchName, Workbook wb)
        {
            ws.Range["A1"].Value = "DAILY SALES AND COLLECTIONS REPORT";
            ws.Range["A1:F1"].Merge();
            ws.Range["A4"].Value = DoctorName;
            ws.Range["A4:F4"].Merge();
            ws.Range["A6"].Value = "DATE";
            ws.Range["B6"].Value = "RR#";
            ws.Range["C6"].Value = "SI#";
            ws.Range["D6"].Value = "OR#";
            ws.Range["E6"].Value = "PATIENT NAME";
            ws.Range["F6"].Value = "EXPLANATION";
            ws.Range["G6"].Value = "PARTICULARS";
            ws.Range["H6"].Value = "QUANTITY";
            ws.Range["I6"].Value = "SESSION";
            ws.Range["J6"].Value = "PATIENT PRICE";
            ws.Range["K6"].Value = "MD PRICE";
            ws.Range["L6"].Value = "GROSS PROFIT";
            ws.Range["M6"].Value = "AMOUNT FOR SHARING";
            ws.Range["N6"].Value = "MD SHARE 60%";
            ws.Range["A6:N6"].Style.Interior.Color = System.Drawing.Color.LightBlue;
            ws.Range["A6:N6"].Style.Font.IsBold = true;
            ws.Range["A2"].Value = BranchName;
            ws.Range["A2:F2"].Merge();
        }
        private void PerGroup(Worksheet ws, EnumerableRowCollection<DataRow> dt, string GroupName, string GroupCode)
        {
            if (dt.Count() > 0)
            {
                ws.Range[rowCount, 1].Value = GroupName;

            }
            rowCount += 1;
            foreach (DataRow row in dt)
            {
                string petsa = row["trx_date"].ToString();
                string Prefix = row["prefix"].ToString();
                string rr_number = row["rr_number"].ToString();
                string si_number = row["si_number"].ToString();
                string or_number = row["or_number"].ToString();
                string patient = row["patient_name"].ToString();
                string explanation = row["explanation"].ToString();
                string particular = row["particular"].ToString();
                string quantity = row["quantity"].ToString();
                string session = row["session"].ToString();
                string net_sales = row["net_sales"].ToString();
                double PatientPrice = Convert.ToDouble(net_sales);
                double MDPrice = Convert.ToDouble(row["cost"].ToString());
                double retention = Convert.ToDouble(row["retention"].ToString());
                MDPrice = Convert.ToDouble(MDPrice) * Convert.ToDouble(quantity);
                string SearchForThis = "3plus1";
                bool threePlusOne = explanation.Contains(SearchForThis);
                SearchForThis = "BOGO";
                bool BOGO = explanation.Contains(SearchForThis);
                SearchForThis = "10%";
                bool tenPercent = explanation.Contains(SearchForThis);
                SearchForThis = "15%";
                bool fifteenPercent = explanation.Contains(SearchForThis);
                SearchForThis = "20%";
                bool twentyPercent = explanation.Contains(SearchForThis);
                SearchForThis = "25%";
                bool twentyFivePercent = explanation.Contains(SearchForThis);
                SearchForThis = "30%";
                bool thirtyPercent = explanation.Contains(SearchForThis);
                SearchForThis = "35%";
                bool thirtyFivePercent = explanation.Contains(SearchForThis);
                SearchForThis = "40%";
                bool fortyPercent = explanation.Contains(SearchForThis);
                SearchForThis = "45%";
                bool fortyFivePercent = explanation.Contains(SearchForThis);
                SearchForThis = "50%";
                bool fiftyPercent = explanation.Contains(SearchForThis);
                if (threePlusOne)
                {
                    PatientPrice = PatientPrice * 0.75;
                    MDPrice = MDPrice * 0.75;
                }
                else if (BOGO)
                {
                    MDPrice = MDPrice * 0.5;
                }
                else if (tenPercent)
                {
                    MDPrice = MDPrice * 0.9;
                }
                else if (fifteenPercent)
                {
                    MDPrice = MDPrice * 0.85;
                }
                else if (twentyPercent)
                {
                    MDPrice = MDPrice * 0.8;
                }
                else if (twentyFivePercent)
                {
                    MDPrice = MDPrice * 0.75;
                }
                else if (thirtyPercent)
                {
                    MDPrice = MDPrice * 0.7;
                }
                else if (thirtyFivePercent)
                {
                    MDPrice = MDPrice * 0.65;
                }
                else if (fortyPercent)
                {
                    MDPrice = MDPrice * 0.6;
                }
                else if (fortyFivePercent)
                {
                    MDPrice = MDPrice * 0.55;
                }
                else if (fiftyPercent)
                {
                    MDPrice = MDPrice * 0.5;
                }

                double GrossProfit = PatientPrice - MDPrice;
                if (GrossProfit < 0)
                {
                    PatientPrice = 0;
                    MDPrice = 0;
                    GrossProfit = 0;
                }

                ws.Range[rowCount, 1].Value = petsa.Replace("00:00:00", "");
                ws.Range[rowCount, 2].Value = Prefix + "-" + rr_number;
                ws.Range[rowCount, 3].Value = si_number;
                ws.Range[rowCount, 4].Value = or_number;
                ws.Range[rowCount, 5].Value = patient;
                ws.Range[rowCount, 6].Value = explanation;
                ws.Range[rowCount, 7].Value = particular;
                ws.Range[rowCount, 8].Value = "'" + quantity;
                ws.Range[rowCount, 9].Value = "'" + session;
                ws.Range[rowCount, 10].Value = PatientPrice.ToString();
                ws.Range[rowCount, 11].Value = MDPrice.ToString();
                ws.Range[rowCount, 12].Value = GrossProfit.ToString();

                rowCount += 1;
                ws.Range[rowCount, 11].Value = "TOTAL NET SALE";
                ws.Range[rowCount + 1, 11].Value = "NET OF VAT";
                ws.Range[rowCount + 2, 11].Value = "LESS:RETENTION COST";

                totalNetSale = totalNetSale + GrossProfit;
                netOfVat = totalNetSale / 1.12;

                if (GroupCode == "BTX")
                {
                    retention = (totalNetSale / 420) * 174.07;
                }
                else if (GroupCode == "THER_EYES")
                {
                    retention = 15000.00;
                }
                else if (GroupCode == "THER_BODY")
                {
                    retention = 34000.00;
                }
                else if (GroupCode == "THER_FN")
                {
                    retention = 20000.00;
                }
                else if (GroupCode == "ULTH_FN")
                {
                    retention = 40000.00;
                }
                else if (GroupCode == "ULTH_FACE")
                {
                    retention = 29000.00;
                }
                else if (GroupCode == "ULTH_BROW")
                {
                    retention = 18000.00;
                }
                else if (GroupCode == "ULTH_JAW")
                {
                    retention = 18000.00;
                }
                else if (GroupCode == "ULTH_FOC")
                {
                    retention = 20000.00;
                }
                else
                {
                    retention = netOfVat * retention;
                }

                totalAmountOfSharing = netOfVat - retention;
                MDShare60 = totalAmountOfSharing * 0.6;
                ws.Range[rowCount, 12].Value = totalNetSale.ToString();
                ws.Range[rowCount + 1, 12].Value = netOfVat.ToString();
                ws.Range[rowCount + 2, 12].Value = retention.ToString();
                ws.Range[rowCount + 2, 13].Value = totalAmountOfSharing.ToString();
                ws.Range[rowCount + 2, 14].Value = MDShare60.ToString();
            }
        }
        public string SalesReportPerBranch(DSCR param)
        {
            try
            {
                MySQL m = new MySQL();
                int branch_id = param.branch_id;
                string datefrom = param.trx_date_from;
                string dateto = param.trx_date_to;
                string branch_name = param.branch_name;

                string sql = string.Format("CALL `prod_syshoo_db`.`sp_mdshare`({0}, '{1}', '{2}');", branch_id, datefrom, dateto);
                System.Data.DataTable data = m.Select(sql);
                sql = "select id, code_fld, value_fld from prod_syshoo_db.hoo_setupcodes_tbl where category_fld = 'item_group' order by code_fld;";
                System.Data.DataTable CategoryData = m.Select(sql);

                Spire.Xls.Workbook wb = new Spire.Xls.Workbook();

                //Initailize worksheet
                Spire.Xls.Worksheet sheet = wb.Worksheets[0];
                SalesReportPerBranchColumn(sheet, branch_name);

                //Loop
                rowCount = 7;
                var total = 0.0;
                var GrandTotal = 0.0;
                foreach (DataRow category in CategoryData.Rows)
                {
                    var SalesReportPerBranch = data.AsEnumerable().Where(row => (row.Field<Int32>("item_group") == category.Field<Int32>("id")));
                    if (SalesReportPerBranch.Count() > 0)
                    {
                        sheet.Range[rowCount, 1].Value = category.Field<string>("value_fld");
                        rowCount += 1;
                        total = 0.0;
                        foreach (DataRow row in SalesReportPerBranch)
                        {
                            string petsa = row["trx_date"].ToString();
                            string Prefix = row["prefix"].ToString();
                            string rr_number = row["rr_number"].ToString();
                            string patient = row["patient_name"].ToString();
                            string particular = row["particular"].ToString();
                            string explanation = row["explanation"].ToString();
                            double net_sales = Convert.ToDouble(row["net_sales"].ToString());

                            sheet.Range[rowCount, 1].Value = row["trx_date"].ToString();
                            sheet.Range[rowCount, 2].Value = row["prefix"].ToString() + "-" + row["rr_number"].ToString();
                            sheet.Range[rowCount, 3].Value = row["patient_name"].ToString();
                            sheet.Range[rowCount, 4].Value = row["explanation"].ToString();
                            sheet.Range[rowCount, 5].Value = row["particular"].ToString();
                            sheet.Range[rowCount, 6].Value = row["net_sales"].ToString(); ;
                            rowCount += 1;
                            sheet.Range[rowCount, 5].Value = "TOTAL NET SALE";
                            total += Convert.ToDouble(row["net_sales"].ToString());
                            sheet.Range[rowCount, 6].Value = total.ToString();
                        }
                        rowCount += 2;
                        GrandTotal += total;
                    }
                    sheet.Range[5, 6].Value = GrandTotal.ToString();
                    sheet.Range[5, 5].Value = "GRAND TOTAL:";
                    sheet.Range[5, 6].NumberFormat = "#,##0.00";
                }


                sheet.Columns[5].NumberFormat = "#,##0.00";
                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();

                string dateStamp = DateTime.Now.ToString("MMddyyyy_HHmmss");
                wb.SaveToFile(@folder.ToString() + branch_name + "_SalesPerBranch" + dateStamp + ".xlsx", ExcelVersion.Version2013);
                return folder.ToString() + branch_name + "_SalesPerBranch" + dateStamp + ".xlsx";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        private void SalesReportPerBranchColumn(Worksheet ws, string BranchName)
        {
            ws.Range["A1"].Value = "SALES PER BRANCH REPORT";
            ws.Range["A1:F1"].Merge();
            ws.Range["A4"].Value = BranchName;
            ws.Range["A6"].Value = "DATE";
            ws.Range["B6"].Value = "RR#";
            ws.Range["C6"].Value = "PATIENT NAME";
            ws.Range["D6"].Value = "EXPLANATION";
            ws.Range["E6"].Value = "PARTICULARS";
            ws.Range["F6"].Value = "SALES";
            ws.Range["A6:F6"].Style.Interior.Color = System.Drawing.Color.LightBlue;
            ws.Range["A6:F6"].Style.Font.IsBold = true;
            ws.Range["A2:F2"].Merge();

        }

    }
}