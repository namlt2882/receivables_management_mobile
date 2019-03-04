using System;
using System.Collections.Generic;
using System.Text;

namespace RCM.Mobile.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public String Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string NData { get; set; }
        public bool IsSeen { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
