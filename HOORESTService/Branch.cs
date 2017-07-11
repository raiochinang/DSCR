using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HOORESTService
{
    public class Branch
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string store_code_fld { get; set; }
        [DataMember]
        public string store_name_fld { get; set; }
    }

    public partial class Branches
    {
        private static readonly Branches _instance = new Branches();
        private Branches() { }
        public static Branches Instance
        {
            get { return _instance; }
        }
        public List<Branch> BranchList
        {
            get
            {
                MySQL m = new MySQL();
                List<Branch> branches = new List<Branch>();
                string sql = "select id, store_code_fld, store_name_fld from prod_syshoo_db.hoo_branch_tbl";
                DataTable data = m.Select(sql);
                foreach (DataRow row in data.Rows)
                {
                    Branch item = new Branch
                    {
                        id = Convert.ToInt32(row["id"]),
                        store_code_fld = row["store_code_fld"].ToString(),
                        store_name_fld = row["store_name_fld"].ToString(),

                    };
                    branches.Add(item);
                }
                return branches;
            }
        }
    }
}