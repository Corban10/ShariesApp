using SQLite;
using System;

namespace ShariesPrototype
{
    class BlockedAccounts // create own class file? *
    {
        [PrimaryKey]
        public int blocker { get; set; }
        [PrimaryKey]
        public int blockee { get; set; }
    }
}
