using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Who.Models
{
    public class AccountSet
    {
        public class Manager
        {
            public string Name { get; set; }
            public string Role { get; set; }
        }

        public class Account : IComparable
        {
            public string Name { get; set; }
            public string Guid { get; set; }

            public void AddManager(string name, string role)
            {
                if(!Managers.ContainsKey(role))
                {
                    Managers.Add(role, new Manager() { Name = name, Role = role });
                }
            }

            public Account(string name, string guid)
            {
                Name = name;
                Guid = guid;
            }

            public SortedDictionary<string, Manager> Managers = new SortedDictionary<string, Manager>();

            public int CompareTo(object obj)
            {
                return Name.CompareTo(((Account)obj).Name);
            }
        }

        public Account AddAccount(string name, string guid)
        {
            Account n;
            if(!Accounts.ContainsKey(name))
            {
                n = new Account(name, guid);
                Accounts.Add(name, n);
            }
            else
            {
                n = Accounts[name];
            }
            return n;
        }

        public void Add(string accountGuid, string accountName, string managerRole, string managerName)
        {
            if(managerRole != null && managerRole.Length > 0)
            {
                Account n = AddAccount(accountName, accountGuid);
                n.AddManager(managerName, managerRole);
                Roles.Add(managerRole);
            }
        }

        string connectionString = "Data Source=EDPSSQLCP01-SQL\\PSVSQLPROD;Initial Catalog=PRM;Integrated Security=True";

        public AccountSet()
        {
        }

        public void Load(string region)
        {
            SqlConnection db;
            using(db = new SqlConnection(connectionString))
            {
                // https://edcrm/PRISM/sfa/accts/edit.aspx?id={3682898C-4F0A-4068-82AC-4121CA1FD919}
                try
                {
                    db.Open();
                    SqlCommand cmd = new SqlCommand(@"  SELECT
	                                                        xap_managersid as AccountGuid,
                                                            xap_managersidname as Account,
	                                                        xap_manageridname as Manager,
	                                                        xap_managerrolename as Role
                                                        FROM filteredxap_additionalmanager
                                                        WHERE
	                                                        xap_salesregionid = @region
                                                        ORDER BY
	                                                        Account, Manager, Role
                                                        ", db);
                    cmd.Parameters.Add(new SqlParameter("region", region));
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        object accountGuid = r["AccountGuid"];
                        object account = r["Account"];
                        object manager = r["Manager"];
                        object role = r["Role"];
                        if (accountGuid.GetType() == typeof(DBNull))
                        {
                            accountGuid = "";
                        }
                        if (account.GetType() == typeof(DBNull))
                        {
                            account = "-";
                        }
                        if (manager.GetType() == typeof(DBNull))
                        {
                            manager = "-";
                        }
                        if (role.GetType() == typeof(DBNull))
                        {
                            role = "-";
                        }
                        Add(accountGuid.ToString(), (string)account, (string)role, (string)manager);
                    }
                }
                catch (SqlException e)
                {
                    Error = e.ToString();
                }
            }
        }

        public string Error { get; set; }

        public SortedDictionary<string, Account> Accounts = new SortedDictionary<string, Account>();

        public SortedSet<string> Roles = new SortedSet<string>();
    }
}
