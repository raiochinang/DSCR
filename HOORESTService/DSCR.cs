using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

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

    public partial class DSCRs
    {
        private static readonly DSCRs _instance = new DSCRs();
        private DSCRs() { }
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
            DataTable data = m.Select(sql);
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

        public DSCR GetDSCRbyRange(DSCR dscr)
        {
            DSCR result = new DSCR();
            MySQL m = new MySQL();
            string fullRR = dscr.prefix + "-" + dscr.rr_number;
            List<Detail> details = new List<Detail>();
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_dscr_by_date_range`('{0}', '{1}');", dscr.trx_date_from, dscr.trx_date_to);

            DataTable data = m.Select(sql);
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
            DataTable data = m.Select(sql);
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
    }
}