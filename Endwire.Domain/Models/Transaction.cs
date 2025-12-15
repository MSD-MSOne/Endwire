using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class Transaction
    {

        public long Id { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<int> TotalTimeInMinutes { get; set; }
        public string Description { get; set; }
        public Nullable<int> TransactionType { get; set; }
        public Nullable<int> ServiceType { get; set; }
        public int FromUserId { get; set; }
        public Nullable<int> ToUserId { get; set; }
        public Nullable<int> ChatLogId { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
        public virtual ChatLog ChatLog { get; set; }
    }
}
