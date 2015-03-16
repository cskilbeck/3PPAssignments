using System;
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

            public void AddManager(string name, string role)
            {
                if(!Managers.ContainsKey(role))
                {
                    Managers.Add(role, new Manager() { Name = name, Role = role });
                }
            }

            public SortedDictionary<string, Manager> Managers = new SortedDictionary<string, Manager>();

            public int CompareTo(object obj)
            {
                return Name.CompareTo(((Account)obj).Name);
            }
        }

        public Account AddAccount(string name)
        {
            Account n;
            if(!Accounts.ContainsKey(name))
            {
                n = new Account();
                Accounts.Add(name, n);
            }
            else
            {
                n = Accounts[name];
            }
            return n;
        }

        public void Add(string accountName, string managerRole, string managerName)
        {
            if(managerRole != null && managerRole.Length > 0)
            {
                Account n = AddAccount(accountName);
                n.AddManager(managerName, managerRole);
                Roles.Add(managerRole);
            }
        }

        public string Error { get; set; }

        public SortedDictionary<string, Account> Accounts = new SortedDictionary<string, Account>();

        public SortedSet<string> Roles = new SortedSet<string>();
    }
}
