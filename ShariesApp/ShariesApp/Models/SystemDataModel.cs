using SQLite;
using System;

namespace ShariesPrototype
{
    class SystemDataModel
    {
        [PrimaryKey]
        public double creditLimit { get; set; }
        [PrimaryKey]
        public double textLimit { get; set; }
        [PrimaryKey]
        public double dataLimit { get; set; }
        [PrimaryKey]
        public double minutesLimit { get; set; }
    }
}
