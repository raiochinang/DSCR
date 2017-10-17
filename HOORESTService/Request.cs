using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Data;

namespace HOORESTService
{
    public class Request
    {
        [DataMember]
        public int request_id { get; set; }
        [DataMember]
        public int branch_id { get; set; }
        [DataMember]
        public string branch_name { get; set; }
        [DataMember]
        public string transaction_date { get; set; }
        [DataMember]
        public int created_by { get; set; }
        [DataMember]
        public string created_by_name { get; set; }
        [DataMember]
        public int approved_by { get; set; }
        [DataMember]
        public string approved_by_name { get; set; }
    }

    public partial class Requests
    {
        private static readonly Requests _instance = new Requests();
        public static Requests Instance
        {
            get { return _instance; }
        }

        public List<Request> List()
        {
            MySQL m = new MySQL();
            string sql = string.Format("SELECT * FROM prod_syshoo_db.inv_request_vw;");
            DataTable data = m.Select(sql);
            List<Request> result = data.AsEnumerable().Select(row =>
                new Request
                {
                    request_id = row.Field<int>("request_id"),
                    branch_id = row.Field<int>("branch_id"),
                    branch_name = row.Field<string>("branch_name"),
                    transaction_date = Convert.ToString(row.Field<DateTime>("transaction_date")),
                    created_by =row.Field<int>("created_by"),
                    created_by_name = row.Field<string>("created_by_name")                    
                }).ToList();


            return result;
        }

        public Request FindOne(string id)
        {
            MySQL m = new MySQL();            
            string sql = string.Format("SELECT * FROM prod_syshoo_db.inv_request_vw WHERE request_id={0};", id);
            DataTable data = m.Select(sql);
            Request result = data.AsEnumerable()
                .Select(row =>
                new Request
                {
                    request_id = row.Field<int>("request_id"),
                    branch_id = row.Field<int>("branch_id"),
                    branch_name = row.Field<string>("branch_name"),
                    transaction_date = Convert.ToString(row.Field<DateTime>("transaction_date")),
                    created_by = row.Field<int>("created_by"),
                    created_by_name = row.Field<string>("created_by_name")
                })
                .Where(r => (r.request_id.ToString() == id)).FirstOrDefault();

            return result;
        }

    }

}