using System;
using System.Collections.Generic;
using System.Text;

namespace RCM.Mobile.Models
{
    public class Receivable
    {
        public int Id { get; set; }
        public int? ClosedDay { get; set; }
        public int PayableDay { get; set; }
        public long PrepaidAmount { get; set; }
        public long DebtAmount { get; set; }
        public int CustomerId { get; set; }
        public int? LocationId { get; set; }
    }
}
