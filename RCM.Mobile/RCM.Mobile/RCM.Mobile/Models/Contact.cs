using System;
using System.Collections.Generic;
using System.Text;

namespace RCM.Mobile.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string IdNo { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool HasAddress => !string.IsNullOrEmpty(Address);
    }

}
