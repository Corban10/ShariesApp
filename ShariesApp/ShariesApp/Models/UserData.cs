using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShariesApp
{
    public class UserData
    {
        [PrimaryKey, Unique]
        public int AccountNumber { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }
    }
}
