using Prism.Mvvm;
using RCM.Mobile.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.Common.DataAnnotations;

namespace RCM.Mobile.Models
{
    public class Receivable : BindableBase
    {
        [Ignore]
        public int Id { get; set; }

        [DisplayOptions(Header = "Prepaid Amount", Group = "Receivable Info")]
        [ReadOnly]
        [Converter(typeof(MoneyConverter))]
        public decimal PrepaidAmount { get; set; }

        [DisplayOptions(Header = "Debt Amount", Group = "Receivable Info")]
        [Converter(typeof(MoneyConverter))]
        [ReadOnly]
        public decimal DebtAmount { get; set; }

        [Ignore]
        public int? LocationId { get; set; }

        private int _collectionProgressStatus;
        [Ignore]
        public int CollectionProgressStatus
        {
            get
            {
                return this._collectionProgressStatus;
            }
            set { SetProperty(ref _collectionProgressStatus, value); }
        }

        [DisplayOptions(Header = "Start Day", Group = "Receivable Info")]
        [Converter(typeof(IntToDateConverter))]
        [ReadOnly]
        public int? PayableDay { get; set; }

        private int? _closedDay;
        [DisplayOptions(Header = "End Day", Group = "Receivable Info")]
        [Converter(typeof(IntToDateConverter))]
        [ReadOnly]
        public int? ClosedDay
        {
            get
            {
                return this._closedDay;
            }
            set { SetProperty(ref _closedDay, value); }
        }

        [DisplayOptions(Header = "Customer Name", Group = "Debtor Info")]
        [ReadOnly]
        public string CustomerName { get; set; }
        
        [DisplayOptions(Header = "Debtor Name", Group = "Debtor Info")]
        [ReadOnly]
        public string DebtorName { get; set; }
        [ReadOnly]
        [Ignore]
        [DisplayOptions(Header = "Confirnm", Group = "Receivable Info")]
        public bool IsConfirmed { get; set; }

        [ReadOnly]
        [Ignore]
        public IEnumerable<Contact> Contacts { get; set; }

        [ReadOnly]
        [DisplayOptions(Header = "Assign Date", Group = "Receivable Info")]
        [Converter(typeof(IntToDateConverter))]
        public int AssignDate { get; set; }

    }



    public class ReceivableCloseModel
    {
        public int Id { get; set; }
        public bool isPayed { get; set; }
    }
}
