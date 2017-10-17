using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Data;
using System.Text;

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

    public class RequestDetails
    {
        [DataMember]
        public int request_details_id { get; set; }
        [DataMember]
        public int request_id { get; set; }
        [DataMember]
        public int item_id { get; set; }
        [DataMember]
        public string item_name_fld { get; set; }
        [DataMember]
        public int quantity { get; set; }
        [DataMember]
        public string note { get; set; }
    }

    public class ObjRequest
    {
        [DataMember]
        public Request Request { get; set; }
        [DataMember]
        public List<RequestDetails> RequestDetails { get; set; }
        [DataMember]
        public int total_count { get; set; }

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
            string sql = string.Format("SELECT * FROM prod_syshoo_db.inv_request_vw WHERE approved_by = 0;");
            DataTable data = m.Select(sql);
            List<Request> result = data.AsEnumerable().Select(row =>
                new Request
                {
                    request_id = row.Field<int>("request_id"),
                    branch_id = row.Field<int>("branch_id"),
                    branch_name = row.Field<string>("branch_name"),
                    transaction_date = Convert.ToString(row.Field<DateTime>("transaction_date")),
                    created_by = row.Field<int>("created_by"),
                    created_by_name = row.Field<string>("created_by_name")
                }).ToList();


            return result;
        }

        public ObjRequest FindOne(string id)
        {
            MySQL m = new MySQL();
            ObjRequest result = new ObjRequest();
            string sql = string.Format("SELECT * FROM prod_syshoo_db.inv_request_vw WHERE request_id={0};", id);
            DataTable data = m.Select(sql);
            Request Request = data.AsEnumerable()
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
            result.Request = Request;

            sql = string.Format("SELECT * FROM prod_syshoo_db.inv_request_details_vw WHERE request_id = {0};", id);
            data = m.Select(sql);
            List<RequestDetails> RequestDetails = data.AsEnumerable().Select(row =>
                new RequestDetails
                {
                    request_id = row.Field<int>("request_id"),
                    request_details_id = row.Field<int>("request_details_id"),
                    item_id = row.Field<int>("item_id"),
                    item_name_fld = row.Field<string>("item_name"),
                    quantity = row.Field<int>("quantity"),
                    note = row.Field<string>("note")
                }).ToList();
            result.RequestDetails = RequestDetails;
            result.total_count = RequestDetails.Count();
            return result;
        }

        public ObjRequest Insert(ObjRequest p)
        {
            MySQL m = new MySQL();
            int request_id = 0;
            string trx_date = p.Request.transaction_date.Substring(0, p.Request.transaction_date.Length - 1);

            //main
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_inv_request_insert`('{0}', '{1}', '{2}', '{3}');", p.Request.branch_id, p.Request.transaction_date, p.Request.created_by, 0);
            System.Data.DataTable data = m.Select(sql);
            if (data.Rows.Count > 0)
            {
                request_id = Convert.ToInt32(data.Rows[0][0]);
            }

            if (p.RequestDetails.Count > 0)
            {
                StringBuilder detail = new StringBuilder();
                detail.Append("INSERT INTO `prod_syshoo_db`.`inv_request_details` (`request_id`,`item_id`,`quantity`,`note`) VALUES ");
                foreach (RequestDetails i in p.RequestDetails)
                {
                    detail.AppendFormat("({0}, {1}, {2}, '{3}'),", request_id, i.item_id, i.quantity, i.note);
                }
                string detail_query = detail.ToString();
                detail_query = detail_query.Substring(0, detail_query.Length - 1);
                detail_query += ";";
                m.Insert(detail_query);
            }

            return p;
        }

        public ObjRequest Update(ObjRequest p)
        {
            MySQL m = new MySQL();
            string trx_date = p.Request.transaction_date.Substring(0, p.Request.transaction_date.Length - 1);

            //main
            string sql = string.Format("UPDATE `prod_syshoo_db`.`inv_request` SET `transaction_date` = '{1}' WHERE `request_id` = {0};", p.Request.request_id, p.Request.transaction_date);
            System.Data.DataTable data = m.Select(sql);

            //delete
            sql = string.Format("DELETE FROM `prod_syshoo_db`.`inv_request_details` WHERE request_id = {0};", p.Request.request_id);
            m.Delete(sql);

            //delete
            if (p.RequestDetails.Count > 0)
            {
                StringBuilder detail = new StringBuilder();
                detail.Append("INSERT INTO `prod_syshoo_db`.`inv_request_details` (`request_id`,`item_id`,`quantity`,`note`) VALUES ");
                foreach (RequestDetails i in p.RequestDetails)
                {
                    detail.AppendFormat("({0}, {1}, {2}, '{3}'),", p.Request.request_id, i.item_id, i.quantity, i.note);
                }
                string detail_query = detail.ToString();
                detail_query = detail_query.Substring(0, detail_query.Length - 1);
                detail_query += ";";
                m.Insert(detail_query);
            }

            return p;
        }


    }

}