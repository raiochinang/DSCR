using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HOORESTService
{
    public class CreditCard
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string rr_number { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string term { get; set; }
        [DataMember]
        public string amount { get; set; }
        [DataMember]
        public string note { get; set; }
    }
    public partial class CreditCards
    {
        private static readonly CreditCards _instance = new CreditCards();
        private CreditCards() { }
        public static CreditCards Instance
        {
            get { return _instance; }
        }
        public List<CreditCard> CreditCardList(string rr_number)
        {
            MySQL m = new MySQL();
            List<CreditCard> creditcards = new List<CreditCard>();
            string sql = string.Format("select * from prod_syshoo_db.dscr_credit_card where rr_number = '{0}';", rr_number);
            DataTable data = m.Select(sql);
            foreach (DataRow row in data.Rows)
            {
                CreditCard item = new CreditCard
                {
                    id = Convert.ToInt32(row["id"]),
                    rr_number = row["rr_number"].ToString(),
                    name = row["name"].ToString(),
                    term = row["term"].ToString(),
                    amount = row["amount"].ToString(),
                    note = row["note"].ToString()                    
                };
                creditcards.Add(item);
            }
            return creditcards;
        }
    }
}