using RCM.Mobile.Commands.NotificationListPage;
using RCM.Mobile.Models;
using RCM.Mobile.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.DataControls.ListView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RCM.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationListPage : ContentPage
    {
        public NotificationListPage()
        {
            InitializeComponent();
            this.list.ItemStyleSelector = new NotificationListViewStyleSelector();
            list.Commands.Add(new ItemTappedUserCommand());


        }
        private void OnButtonClicked(object sender, EventArgs eventArgs)
        {
            this.list.EndItemSwipe();
        }

        //private void ListViewTextCell_Tapped(object sender, EventArgs e)
        //{
        //    //var sourceItem = sender as Notification;
        //    //sourceItem.IsSeen = true;
        //}

    }
}