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

        [DisplayOptions(Position = 0, ColumnSpan = 2, Header = "Prepaid Amount", Group = "Receivable")]
        [ReadOnly]
        [Converter(typeof(MoneyConverter))]
        public decimal PrepaidAmount { get; set; }

        [DisplayOptions(Position = 0, ColumnSpan = 2, Header = "Debt Amount", Group = "Receivable")]
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

        
        private int? _payableDay;
        public int? PayableDay
        {
            get
            {
                return this._payableDay;
            }
            set { SetProperty(ref _payableDay, value); }
        }
        private int? _closedDay;
        [DisplayOptions(Position = 1, ColumnSpan = 2, Header = "End Day", Group = "Receivable")]
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
        public bool HasCloseDay => ClosedDay.HasValue;
        private int _expectationClosedDay;
        public int ExpectationClosedDay
        {
            get
            {
                return this._expectationClosedDay;
            }
            set { SetProperty(ref _expectationClosedDay, value); }
        }
        [DisplayOptions(Position = 2, ColumnSpan = 2, Header = "Customer Name", Group = "Debtor")]
        [ReadOnly]
        public string CustomerName { get; set; }
        
        [DisplayOptions(Position = 3, ColumnSpan = 2, Header = "Debtor Name", Group = "Debtor")]
        [ReadOnly]
        public string DebtorName { get; set; }

        [ReadOnly]
        [Ignore]
        //[DisplayOptions(Header = "Confirnm", Group = "Receivable")]
        public bool IsConfirmed { get; set; }

        [ReadOnly]
        [Ignore]
        public List<Contact> Contacts { get; set; }

        [ReadOnly]
        [DisplayOptions(Position = 4, ColumnSpan = 2, Header = "Assign Date", Group = "Receivable")]
        [Converter(typeof(IntToDateConverter))]
        public int AssignDate { get; set; }
        public double TimeRate { get; set; }
        public string Stage { get; set; }
        public string Action { get; set; }
    }



    public class ReceivableCloseModel
    {
        public int Id { get; set; }
        public bool isPayed { get; set; }
    }
}
