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
            public Guid Guid { get; set; }

            public void AddManager(string name, string role)
            {
                if(!Managers.ContainsKey(role))
                {
                    Managers.Add(role, new Manager() { Name = name, Role = role });
                }
            }

            public Account(string name, Guid guid)
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

        public Account AddAccount(string name, Guid guid)
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

        public void Add(Guid accountGuid, string accountName, string managerRole, string managerName)
        {
            if(managerRole != null && managerRole.Length > 0)
            {
                AddAccount(accountName, accountGuid).AddManager(managerName, managerRole);
                Roles.Add(managerRole);
            }
        }

        string connectionString = "Data Source=EDCRMSQLCP01;Initial Catalog=PRISM_MSCRM;Integrated Security=True";

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
                                                        FROM [PRISM_MSCRM].[dbo].[FilteredXap_additionalmanager]
                                                        WHERE
                                                            xap_salesregionid = @region AND
                                                            xap_managerrole IS NOT NULL
                                                        ORDER BY
                                                            Account, Manager, Role", db);
                    cmd.Parameters.Add(new SqlParameter("region", region));
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        var accountGuid = r["AccountGuid"];
                        var account = r["Account"];
                        var manager = r["Manager"];
                        var role = r["Role"];
                        if (accountGuid != DBNull.Value)
                        {
                            if (account == DBNull.Value)
                            {
                                account = "&#x2019;";
                            }
                            if (manager == DBNull.Value)
                            {
                                manager = "KNOB &#x2019;";
                            }
                            if (role == DBNull.Value)
                            {
                                role = "&#x2014;";
                            }
                            Add((Guid)accountGuid, (string)account, (string)role, (string)manager);
                        }
                    }
                }
                catch (SqlException e)
                {
                    Error = e.ToString();
                }
            }
        }

        public string RegionName { get; set; }

        public string Error { get; set; }

        public SortedDictionary<string, Account> Accounts = new SortedDictionary<string, Account>();

        public SortedSet<string> Roles = new SortedSet<string>();
    }
}
