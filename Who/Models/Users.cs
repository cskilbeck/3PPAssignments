using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Who.Models
{
    public class User
    {
        public Guid ID;
        public string Name;
        public string Account;
        public string Email;
    }

    public class Users
    {
        public string Error;
        public List<User> UserList;

        private static string Get(SqlDataReader o, string field, string def="?")
        {
            return o[field] == DBNull.Value ? def : (string)o[field];
        }

        public Users(string search)
        {
            UserList = new List<User>();

            SqlConnection db;
            using (db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    search = search.Replace('*', '%');
                    search = search.Replace('?', '_');
                    var query = String.Format("SELECT TOP 100 [ContactId] as GUID, [FullName] as Name, [Xap_registrationid] as RegistrationID, [AccountIdName] as Account, [EMailAddress1] as Email FROM [PRISM_MSCRM].[dbo].[Contact] WHERE StatusCode = 1 AND [FullName] LIKE '%{0}%';", search);
                    SqlCommand cmd = new SqlCommand(query, db);
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        if(r["GUID"] != DBNull.Value)
                        {
                            string name = Get(r, "Name");
                            Guid guid = (Guid)r["GUID"];
                            string account = Get(r, "Account");
                            string email = Get(r, "Email");
                            UserList.Add(new User { ID = guid, Name = name, Account = account, Email = email });
                        }
                    }
                }
                catch (SqlException e)
                {
                    Error = "Database error: " + e.ToString();
                }
            }
        }
        
        string connectionString = "Data Source=EDCRMSQLCP01;Initial Catalog=PRISM_MSCRM;Integrated Security=True";
   }
}