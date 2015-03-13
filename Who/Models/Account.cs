using System;
using System.Collections.Generic;

namespace Who.Models
{
    public class AccountSet
    {
        public class Account : IComparable
        {
            public string Name { get; set; }
            public string DAM { get; set; }
            public string AM { get; set; }
            public string RM { get; set; }

            public int CompareTo(object obj)
            {
                return Name.CompareTo(((Account)obj).Name);
            }
        }

        public string Error { get; set; }
        public SortedSet<Account> Accounts = new SortedSet<Account>();
    }
}