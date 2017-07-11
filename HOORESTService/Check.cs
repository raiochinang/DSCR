using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HOORESTService
{
    public class Check
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string rr_number { get; set; }
        [DataMember]
        public string bank_name { get; set; }
        [DataMember]
        public string date_of_check { get; set; }
        [DataMember]
        public string check_number { get; set; }
        [DataMember]
        public string amount { get; set; }
    }

    public partial class Checks
    {
        private static readonly Checks _instance = new Checks();
        private Checks() { }
        public static Checks Instance
        {
            get { return _instance; }
        }
        public List<Check> CheckList(string rr_number)
        {

            MySQL m = new MySQL();
            List<Check> checks = new List<Check>();
            string sql = string.Format("select * from prod_syshoo_db.dscr_check where rr_number = '{0}';", rr_number);
            DataTable data = m.Select(sql);
            foreach (DataRow row in data.Rows)
            {
                Check item = new Check
                {
                    id = Convert.ToInt32(row["id"]),
                    rr_number = row["rr_number"].ToString(),
                    bank_name = row["bank_name"].ToString(),
                    date_of_check = Convert.ToDateTime(row["date_of_check"].ToString(), CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"),                    
                    check_number = row["check_number"].ToString(),
                    amount = row["amount"].ToString()

                };
                checks.Add(item);
            }
            return checks;

        }
    }
}