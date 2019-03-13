using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RCM.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{


        public MainPage ()
		{
			InitializeComponent ();

            //var drawerContent = new StackLayout();
            //drawerContent.Children.Add(new Button { Text = "Mail" });
            //drawerContent.Children.Add(new Button { Text = "Calendar" });
            //drawerContent.Children.Add(new Button { Text = "People" });
            //drawerContent.Children.Add(new Button { Text = "Tasks" });

            //var sideDrawer = new RadSideDrawer
            //{
            //    MainContent = new Label { Text = "Main content" },
            //    DrawerContent = drawerContent,
            //    DrawerLength = 200
            //};
            //NavigationPage.SetHasNavigationBar(this, false);
            
        }
        private void OnMenuTapped(object sender, EventArgs eventArgs)
        {
            this.drawer.IsOpen = !this.drawer.IsOpen;
        }

    }
}