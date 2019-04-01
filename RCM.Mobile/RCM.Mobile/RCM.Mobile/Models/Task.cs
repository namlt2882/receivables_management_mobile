using RCM.Mobile.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.Common.DataAnnotations;

namespace RCM.Mobile.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //[Converter(typeof(TaskStatusConverter))]
        public int Status { get; set; }
        public int Type { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime ExecutionDay { get; set; }
        public DateTime? UpdateDay { get; set; }
        public string Evidence { get; set; }
        public string Note { get; set; }
        public int ReceivableId { get; set; }
        public string UserId { get; set; }

    }


}
