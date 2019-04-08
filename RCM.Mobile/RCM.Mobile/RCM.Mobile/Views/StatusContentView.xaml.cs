using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RCM.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatusContentView : ContentView
	{
		public StatusContentView()
		{
			InitializeComponent ();
            statusList.ItemsSource = Constant.STATUSES;
        }
	}
}