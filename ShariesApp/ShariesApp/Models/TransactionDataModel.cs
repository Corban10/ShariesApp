using SQLite;
using System;

namespace ShariesPrototype
{
  class TransactionDataModel
  {
      [PrimaryKey]
      public int transactionID { get; set; } //PK
      public int transactionSource { get; set; } //FK1
      public int transactionDestination { get; set; } //FK2
      public int transactionType { get; set; } // enumeration of types?
      public double transactionAmount{ get; set; } 
      public bool NotificationStatus { get; set; } // sent or not sent?
  }
}