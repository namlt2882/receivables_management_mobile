﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RCM.Mobile.Style
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPageStyle : ResourceDictionary
    {
		public MainPageStyle()
		{
			InitializeComponent ();
		}
	}
}