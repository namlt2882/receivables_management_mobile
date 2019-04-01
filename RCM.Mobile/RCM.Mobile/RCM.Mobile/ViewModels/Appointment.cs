﻿using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{
    public class Appointment : IAppointment
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public bool IsAllDay { get; set; }
        public Color Color { get; set; }
        public string Detail { get; set; }
    }
}
