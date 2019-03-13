using System;
using System.Collections.Generic;
using System.Text;

namespace RCM.Mobile.ViewModels
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public int StartTime { get; set; }
        public int ExecutionDay { get; set; }
    }
}
