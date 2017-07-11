using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HOORESTService
{
    public class Person
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public Branch branch { get; set; }
        [DataMember]
        public int role { get; set; }
    }
    public partial class Persons
    {
        private static readonly Persons _instance = new Persons();
        private Persons() { }
        public static Persons Instance
        {
            get { return _instance; }
        }
        public List<Person> Doctors(string name)
        {
            MySQL m = new MySQL();
            List<Person> doctors = new List<Person>();
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_dscr_doctor`('{0}');", name);
            DataTable data = m.Select(sql);
            foreach (DataRow row in data.Rows)
            {
                Person item = new Person
                {
                    id = Convert.ToInt32(row["id"]),
                    name = row["full_name_fld"].ToString()
                };
                doctors.Add(item);
            }
            return doctors;
        }
        public List<Person> Nurses(string name)
        {

            MySQL m = new MySQL();
            List<Person> nurses = new List<Person>();
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_dscr_nurse`('{0}');", name);
            DataTable data = m.Select(sql);
            foreach (DataRow row in data.Rows)
            {
                Person item = new Person
                {
                    id = Convert.ToInt32(row["id"]),
                    name = row["full_name_fld"].ToString()
                };
                nurses.Add(item);
            }
            return nurses;
        }

        public List<Person> Cashiers(string name)
        {

            MySQL m = new MySQL();
            List<Person> cashiers = new List<Person>();
            string sql = string.Format("CALL `prod_syshoo_db`.`sp_dscr_cashier`('{0}');", name);
            DataTable data = m.Select(sql);
            foreach (DataRow row in data.Rows)
            {
                Person item = new Person
                {
                    id = Convert.ToInt32(row["id"]),
                    name = row["full_name_fld"].ToString()
                };
                cashiers.Add(item);
            }
            return cashiers;
        }

        public List<Person> Patients(string name)
        {

            MySQL m = new MySQL();
            List<Person> patients = new List<Person>();
            string sql = string.Format("select id, full_name_fld from prod_syshoo_db.hoo_patient_vw where full_name_fld like '%{0}%' order by full_name_fld limit 1000;", name);
            DataTable data = m.Select(sql);
            foreach (DataRow row in data.Rows)
            {
                Person item = new Person
                {
                    id = Convert.ToInt32(row["id"]),
                    name = row["full_name_fld"].ToString()
                };
                patients.Add(item);
            }
            return patients;
        }

        public Person Login(Person person)
        {
            MySQL m = new MySQL();
            Person result = new Person();
            string sql = string.Format("SELECT * FROM prod_syshoo_db.dscr_user_vw where user_name = '{0}' and password = '{1}'", person.username, person.password);
            DataTable data = m.Select(sql);
            if (data.Rows.Count != 0)
            {
                Branch branch = new Branch();
                branch.store_code_fld = data.Rows[0]["branch_code"].ToString();
                branch.store_name_fld = data.Rows[0]["branch"].ToString();
                branch.id = Convert.ToInt32(data.Rows[0]["branch_id"]);
                result.branch = branch;
                result.username = person.username;
                result.id = Convert.ToInt32(data.Rows[0]["id"]);
                result.role = Convert.ToInt32(data.Rows[0]["role_id"]);
            }
            return result;
        }

        public List<Person> DSCRUsers()
        {

            MySQL m = new MySQL();
            List<Person> users = new List<Person>();
            string sql = "select * from prod_syshoo_db.dscr_user_vw;";
            DataTable data = m.Select(sql);
            foreach (DataRow row in data.Rows)
            {

                Branch branch = new Branch();
                branch.store_code_fld = "no value";
                branch.store_name_fld = "no value";
                branch.id = 0;

                string pword = string.Empty;

                if (row["password"] != System.DBNull.Value)
                {
                    pword = row["password"].ToString();
                }

                Person item = new Person
                {

                    id = Convert.ToInt32(row["id"]),
                    username = row["user_name"].ToString(),
                    name = row["full_name_fld"].ToString(),
                    branch = branch,
                    role = Convert.ToInt32(row["role_id"]),
                    password = pword
                };

                users.Add(item);
            }
            return users;
        }

        public bool UpdateUser(Person p)
        {
            try {
                MySQL m = new MySQL();
                string sql = string.Format("DELETE FROM `prod_syshoo_db`.`dscr_users` WHERE `user_id` = {0};", p.id);
                m.Delete(sql);
                sql = string.Format("INSERT INTO `prod_syshoo_db`.`dscr_users` (`user_id`, `password`) VALUES ('{0}','{1}');", p.id, p.password);
                m.Insert(sql);                
                return true;
            }
            catch(Exception e) {
                return false;
            }                      
        }
    }
}