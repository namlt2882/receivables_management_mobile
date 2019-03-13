using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Telerik.XamarinForms.DataControls.ListView;

namespace RCM.Mobile.Style
{
    public class NotificationListViewStyleSelector : ListViewStyleSelector
    {

        protected override void OnSelectStyle(object item, ListViewStyleContext styleContext)
        {
            var sourceItem = item as Notification;
            styleContext.SelectedItemStyle = new ListViewItemStyle
            {
                BackgroundColor = sourceItem.IsSeen? Color.White: Color.AliceBlue,
                BorderColor = Color.Black,
                BorderWidth = 1
            };
            styleContext.PressedItemStyle = new ListViewItemStyle
            {
                BackgroundColor = sourceItem.IsSeen ? Color.White : Color.AliceBlue,
                BorderColor = Color.Black,
                BorderWidth = 1
            };
            styleContext.ItemStyle = new ListViewItemStyle
            {
                BackgroundColor = sourceItem.IsSeen ? Color.White : Color.AliceBlue,
                BorderColor = Color.Black,
                BorderWidth = 1

            };
        }
    }
}
