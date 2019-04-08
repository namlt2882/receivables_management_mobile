using System;
using System.Collections.Generic;
using System.Text;

namespace RCM.Mobile.Models
{
    public class UpdateTaskModel
    {
        public int Id { get; set; }
        public File File { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }

    }
}
