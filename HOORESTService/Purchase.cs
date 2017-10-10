using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Data;

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
            var isProcess = "Y";
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
    }
}