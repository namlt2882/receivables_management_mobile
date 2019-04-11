using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RCM.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivableTaskListPage : ContentPage
    {
        //private double swipeDetectionThreshold = 100;
        //private double totalTranslatedX;

        public ReceivableTaskListPage()
        {
            InitializeComponent();
        }

        //private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        //{
        //    if (e.StatusType == GestureStatus.Running)
        //    {
        //        // Keep track of the total X translation while gesture is active
        //        totalTranslatedX = e.TotalX;
        //    }
        //    else if (e.StatusType == GestureStatus.Completed)
        //    {
        //        // When the gesture is complete, check the totalTranslatedX value and determine if the gesture was a left ro right swipe

        //        var selectedIndex = tabView.Items.IndexOf(tabView.SelectedItem as TabViewItem);

        //        if (totalTranslatedX < -swipeDetectionThreshold) // Swiped left
        //        {
        //            if (tabView.SelectedItem == tabView.Items.LastOrDefault())
        //            {
        //                tabView.SelectedItem = tabView.Items.FirstOrDefault();
        //            }
        //            else
        //            {
        //                tabView.SelectedItem = tabView.Items[selectedIndex + 1];
        //            }
        //        }
        //        else if (totalTranslatedX > swipeDetectionThreshold) // Swiped right
        //        {
        //            // Swiped right
        //            if (tabView.SelectedItem == tabView.Items.FirstOrDefault())
        //            {
        //                tabView.SelectedItem = tabView.Items.LastOrDefault();
        //            }
        //            else
        //            {
        //                tabView.SelectedItem = tabView.Items[selectedIndex - 1];
        //            }
        //        }
        //    }
        //}

    }
}
