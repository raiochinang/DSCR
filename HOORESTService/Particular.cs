using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using MySql.Data;
using System.Data;

namespace HOORESTService
{
    [DataContract]
    public class Particular
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string BrandCode { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Price { get; set; }
        [DataMember]
        public string Gross { get; set; }
        [DataMember]
        public string Retention { get; set; }
        [DataMember]
        public string Barcode { get; set; }
    }

    public partial class Particulars
    {
        private static readonly Particulars _instance = new Particulars();

        private Particulars() { }
        string _ParticularName;
        public string ParticularName { set { _ParticularName = value; } get { return _ParticularName; } }
        public static Particulars Instance
        {
            get { return _instance; }
        }
        public List<Particular> ParticularList
        {
            get
            {
                MySQL m = new MySQL();
                List<Particular> particulars = new List<Particular>();
                string sql = "select id, brand_code_fld, item_name_fld, item_price_fld, item_gross_amount_fld, item_retention_fld,concat(brand_code_fld,item_code_fld) as 'Barcode'  from prod_syshoo_db.hoo_item_tbl";
                DataTable data = m.Select(sql);
                foreach (DataRow row in data.Rows)
                {
                    Particular item = new Particular
                    {
                        Id = Convert.ToInt32(row["id"]),
                        BrandCode = row["brand_code_fld"].ToString(),
                        Gross = row["item_gross_amount_fld"].ToString(),
                        Name = row["item_name_fld"].ToString(),
                        Price = row["item_price_fld"].ToString(),
                        Retention = row["item_retention_fld"].ToString(),
                        Barcode = row["Barcode"].ToString()
                    };
                    particulars.Add(item);
                }
                return particulars;
            }
        }
        public List<Particular> SearchParticulars(string barcode)
        {
            MySQL m = new MySQL();
            barcode = barcode.Substring(0, 5);
            List<Particular> particulars = new List<Particular>();
            string sql = string.Format("select id, brand_code_fld, item_name_fld, item_price_fld, item_gross_amount_fld, item_retention_fld,concat(brand_code_fld,item_code_fld) as 'Barcode' from prod_syshoo_db.hoo_item_tbl where concat(brand_code_fld, item_code_fld) = '{0}'; ", barcode);
            DataTable data = m.Select(sql);
            foreach (DataRow row in data.Rows)
            {
                Particular item = new Particular
                {
                    Id = Convert.ToInt32(row["id"]),
                    BrandCode = row["brand_code_fld"].ToString(),
                    Gross = row["item_gross_amount_fld"].ToString(),
                    Name = row["item_name_fld"].ToString(),
                    Price = row["item_price_fld"].ToString(),
                    Retention = row["item_retention_fld"].ToString(),
                    Barcode = row["Barcode"].ToString()
                };
                particulars.Add(item);
            }
            return particulars;

        }
        public bool Connection()
        {
            MySQL m = new MySQL();
            bool result = m.OpenConnection();
            return result;
        }
    }
}