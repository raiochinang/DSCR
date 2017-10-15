using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Data;
using System.Text;

namespace HOORESTService
{
    public class Purchase
    {
        [DataMember]
        public int po_id { get; set; }
        [DataMember]
        public string transaction_date { get; set; }
        [DataMember]
        public string reference { get; set; }
        [DataMember]
        public string supplier { get; set; }
        [DataMember]
        public string note { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public string approved_by { get; set; }
        [DataMember]
        public List<PurchaseDetails> details { get; set; }
        [DataMember]
        public int total_count { get; set; }
    }

    public class PurchaseDetails
    {
        [DataMember]
        public int po_id { get; set; }
        [DataMember]
        public int item_id { get; set; }
        [DataMember]
        public string item_name_fld { get; set; }
        [DataMember]
        public string lot_number { get; set; }
        [DataMember]
        public int quantity { get; set; }
        [DataMember]
        public string currency { get; set; }
        [DataMember]
        public decimal md_price { get; set; }
        [DataMember]
        public decimal actual_cost { get; set; }
        [DataMember]
        public string expiration_date { get; set; }
    }

    public partial class Purchases
    {
        private static readonly Purchases _instance = new Purchases();
        private Purchases() { }
        public static Purchases Instance
        {
            get { return _instance; }
        }

        public List<Purchase> PurchaseList()
        {
            MySQL m = new MySQL();
            List<Purchase> purchases = new List<Purchase>();
            var isProcess = "N";
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_inv_purchases`('{0}');", isProcess);
            DataTable data = m.Select(sql);
            foreach (DataRow row in data.Rows)
            {
                Purchase item = new Purchase
                {
                    po_id = Convert.ToInt32(row["po_id"]),
                    transaction_date = row["transaction_date"].ToString(),
                    reference = row["reference"].ToString(),
                    supplier = row["supplier"].ToString(),
                    note = row["note"].ToString(),
                    created_by = row["created_by"].ToString(),
                    approved_by = row["approved_by"].ToString()
                };
                purchases.Add(item);
            }
            return purchases;
        }

        public List<PurchaseDetails> FindDetails(string id)
        {
            MySQL m = new MySQL();
            List<PurchaseDetails> result = new List<PurchaseDetails>();
            string sql = string.Format("SELECT a.* , b.item_name_fld FROM prod_syshoo_db.inv_purchase_order_details a JOIN prod_syshoo_db.hoo_item_tbl b ON a.item_id = b.id WHERE po_id = {0};", id);
            DataTable purchase_details = m.Select(sql);
            foreach (DataRow row in purchase_details.Rows)
            {
                PurchaseDetails item = new PurchaseDetails
                {
                    po_id = Convert.ToInt32(row["po_id"]),
                    item_id = Convert.ToInt32(row["item_id"]),
                    item_name_fld = row["item_name_fld"].ToString(),
                    actual_cost = Convert.ToDecimal(row["actual_cost"]),
                    currency = row["currency"].ToString(),
                    quantity = Convert.ToInt32(row["quantity"]),
                    md_price = Convert.ToDecimal(row["md_price"]),
                    expiration_date = row["expiration_date"].ToString(),
                    lot_number = row["lot_number"].ToString()
                };
                result.Add(item);

            }
            return result;
        }

        public Purchase FindOne(string id)
        {
            MySQL m = new MySQL();
            Purchase result = new Purchase();
            string sql = string.Format("SELECT * FROM prod_syshoo_db.inv_purchase_order WHERE po_id={0};", id);
            DataTable purchase_order = m.Select(sql);
            if (purchase_order.Rows.Count > 0)
            {
                result.transaction_date = purchase_order.Rows[0]["transaction_date"].ToString();
                result.reference = purchase_order.Rows[0]["reference"].ToString();
                result.supplier = purchase_order.Rows[0]["supplier"].ToString();
                result.note = purchase_order.Rows[0]["note"].ToString();
                result.po_id = Convert.ToInt32(id);
                result.details = this.FindDetails(id);
                result.total_count = result.details.Count();
            }
            return result;
        }

        public Purchase Insert(Purchase p)
        {            
            MySQL m = new MySQL();
            StringBuilder master = new StringBuilder();
            int PO_ID = 0;
            string trx_date = p.transaction_date.Substring(0, p.transaction_date.Length - 1);

            //main
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_inv_purchase_order_insert`('{0}', '{1}', '{2}', '{3}', {4});", p.transaction_date, p.reference, p.supplier, p.note, p.created_by);
            System.Data.DataTable data = m.Select(sql);
            if (data.Rows.Count > 0)
            {
                PO_ID = Convert.ToInt32(data.Rows[0][0]);
            }
            //detail
            if (p.details.Count > 0)
            {
                StringBuilder detail = new StringBuilder();
                detail.Append("INSERT INTO `prod_syshoo_db`.`inv_purchase_order_details` (`po_id`,`item_id`,`lot_number`,`quantity`,`currency`,`md_price`,`actual_cost`,`expiration_date`) VALUES ");
                foreach (PurchaseDetails i in p.details)
                {
                    detail.AppendFormat("({0}, {1}, '{2}', ", PO_ID, i.item_id, i.lot_number);
                    detail.AppendFormat(" {0}, '{1}', '{2}', ", i.quantity, i.currency, i.md_price);
                    detail.AppendFormat(" '{0}' , '{1}'),", i.actual_cost, Convert.ToDateTime(i.expiration_date).ToString("yyyy-MM-dd HH:mm:ss"));                    
                }
                string detail_query = detail.ToString();
                detail_query = detail_query.Substring(0, detail_query.Length - 1);
                detail_query += ";";
                m.Insert(detail_query);
            }


            p.po_id = PO_ID;
            return p;
        }
        public Purchase Update(Purchase p)
        {
            return null;
            //MySQL m = new MySQL();
            //StringBuilder master = new StringBuilder();
            //int PO_ID = 0;
            //string trx_date = p.transaction_date.Substring(0, p.transaction_date.Length - 1);

            ////main
            //string sql = string.Format("CALL `prod_syshoo_db`.`sp_inv_purchase_order_insert`('{0}', '{1}', '{2}', '{3}', {4});", p.transaction_date, p.reference, p.supplier, p.note, p.created_by);
            //System.Data.DataTable data = m.Select(sql);
            //if (data.Rows.Count > 0)
            //{
            //    PO_ID = Convert.ToInt32(data.Rows[0][0]);
            //}
            ////detail
            //if (p.details.Count > 0)
            //{
            //    StringBuilder detail = new StringBuilder();
            //    detail.Append("INSERT INTO `prod_syshoo_db`.`inv_purchase_order_details` (`po_id`,`item_id`,`lot_number`,`quantity`,`currency`,`md_price`,`actual_cost`,`expiration_date`) VALUES ");
            //    foreach (PurchaseDetails i in p.details)
            //    {
            //        detail.AppendFormat("({0}, {1}, '{2}', ", PO_ID, i.item_id, i.lot_number);
            //        detail.AppendFormat(" {0}, '{1}', '{2}', ", i.quantity, i.currency, i.md_price);
            //        detail.AppendFormat(" '{0}' , '{1}'),", i.actual_cost, Convert.ToDateTime(i.expiration_date).ToString("yyyy-MM-dd HH:mm:ss"));
            //    }
            //    string detail_query = detail.ToString();
            //    detail_query = detail_query.Substring(0, detail_query.Length - 1);
            //    detail_query += ";";
            //    m.Insert(detail_query);
            //}


            //p.po_id = PO_ID;
            //return p;
        }
    }
}