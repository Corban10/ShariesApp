using SQLite;
using System;

namespace ShariesPrototype
{
    class UserCreditModel 
    // This is the only model that isn't included in our ERD and class diagram
    // because it represents the main systems database
    {
        [PrimaryKey]
        public int accountNumber { get; set; }
        public double creditAmount { get; set; }
        public double textAmount { get; set; }
        public double dataAmount { get; set; }
        public double minutesAmount { get; set; }
    }
}
