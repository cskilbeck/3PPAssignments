﻿using System;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Who.Models
{
    public class RegionsSet
    {
        public class Region : IComparable
        {
            public string Name { get; set; }
            public Guid GUID { get; set; }

            public int CompareTo(object obj)
            {
                return Name.CompareTo(((Region)obj).Name);
            }
        }

        public Region AddRegion(string name, Guid guid)
        {
            Region r;
            if (Regions.ContainsKey(name))
            {
                r = Regions[name];
            }
            else
            {
                r = new Region() { Name = name, GUID = guid };
                Regions.Add(name, r);
            }
            return r;
        }

        public RegionsSet()
        {
            SqlConnection db;
            using (db = new SqlConnection(connectionString))
            {
                try
                {
                    db.Open();
                    SqlCommand cmd = new SqlCommand(@"SELECT [xap_name] AS Name ,[xap_salesregionid] AS GUID FROM [PRISM_MSCRM].[dbo].[FilteredXap_salesregion] WHERE statuscode=1", db);
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        var name = r["Name"];
                        var guid = r["GUID"];
                        if (name == DBNull.Value)
                        {
                            name = "&#x2014;";
                        }
                        if (guid != DBNull.Value)
                        {
                            AddRegion((string)name, (Guid)guid);
                        }
                    }
                }
                catch (SqlException e)
                {
                    Error = "Database error: " + e.ToString();
                }
            }
        }

        public SortedDictionary<string, Region> Regions = new SortedDictionary<string, Region>();
        public string Error { get; set; }

        string connectionString = "Data Source=EDCRMSQLCP01;Initial Catalog=PRISM_MSCRM;Integrated Security=True";
    }
}
