using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.DataControls.ListView.Commands;

namespace RCM.Mobile.Commands.NotificationListPage
{
    public class ItemTappedUserCommand : ListViewCommand
    {

        public ItemTappedUserCommand()
        {
            Id = CommandId.ItemTap;
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override void Execute(object parameter)
        {
            var tappedItem = (parameter as ItemTapCommandContext).Item;
            //add your logic here
            //Application.Current.MainPage.DisplayAlert("", "You've selected " + tappedItem, "OK");
        }
    }

}
