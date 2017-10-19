using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HOORESTService
{
    public class User
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int user_id { get; set; }
        [DataMember]
        public string full_name { get; set; }
        [DataMember]
        public string user_name { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string branch { get; set; }
        [DataMember]
        public string branch_code { get; set; }
        [DataMember]
        public int current_user_id { get; set; }
    }

    public class History
    {
        [DataMember]
        public string module { get; set; }
        [DataMember]
        public int transaction_id { get; set; }
        [DataMember]
        public string transaction_type { get; set; }
        [DataMember]
        public int user_id { get; set; }
    }

    public partial class Users
    {
        private static readonly Users _instance = new Users();
        private Users() { }
        public static Users Instance
        {
            get { return _instance; }
        }
        public List<User> UserList
        {
            get
            {
                MySQL m = new MySQL();
                List<User> users = new List<User>();
                string sql = "select * from prod_syshoo_db.dscr_user_vw";
                DataTable data = m.Select(sql);
                foreach (DataRow row in data.Rows)
                {
                    User item = new User
                    {
                        id = Convert.ToInt32(row["id"]),
                        user_name = row["user_name"].ToString(),
                        password = string.IsNullOrEmpty(row["password"].ToString()) ? row["password"].ToString() : string.Empty,
                        full_name = string.IsNullOrEmpty(row["full_name"].ToString()) ? row["full_name"].ToString() : string.Empty,
                        branch = string.IsNullOrEmpty(row["branch"].ToString()) ? row["branch"].ToString() : string.Empty,
                        branch_code = string.IsNullOrEmpty(row["branch_code"].ToString()) ? row["branch_code"].ToString() : string.Empty,
                    };
                    users.Add(item);
                }
                return users;
            }
        }

        public List<User> UserSave(User user)
        {
            MySQL m = new MySQL();
            List<User> users = new List<User>();
            string sql = string.Format("CALL prod_syshoo_db.dscr_users_save({0}, '{1}')", user.user_id, user.password);
            m.Insert(sql);

            return UserList;
        }

        public void log(History p)
        {
            MySQL m = new MySQL();
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_inv_history_log_insert`('{0}', {1}, '{2}', {3});", p.module, p.transaction_id, p.transaction_type, p.user_id);
            m.Insert(sql);
        }
    }
}