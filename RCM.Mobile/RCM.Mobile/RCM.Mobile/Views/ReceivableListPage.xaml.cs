using RCM.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RCM.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivableListPage : ContentPage
    {
        public ReceivableListPage()
        {
            InitializeComponent();
        }
        private string currentText;
        private bool isRemoteSearchRunning;


        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {

            var autoCompleteView = (RadAutoCompleteView)sender;
            this.currentText = e.NewTextValue ?? "";

            if (this.currentText.Length >= autoCompleteView.SearchThreshold && !this.isRemoteSearchRunning)
            {
                this.isRemoteSearchRunning = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(1500), () =>
                {
                    Task.Factory.StartNew(async () =>
                                {
                                    this.isRemoteSearchRunning = false;
                                    string searchText = this.currentText.ToLower();
                                    var context = ((ReceivableListPageViewModel)this.BindingContext);
                                    await context.InitDebtorsAsync(currentText);
                                    autoCompleteView.ItemsSource = context.Debtors.Where(i => i.Name.ToLower().Contains(searchText)).ToList();
                                });
                    return false;
                });
            }

            //if (searchText.Length >= autoCompleteView.SearchThreshold && autoCompleteView.ShowSuggestionView)
            //{
            //    if (string.IsNullOrEmpty(e.OldTextValue) && autoCompleteView.ItemsSource == null)
            //    {
            //        Device.StartTimer(TimeSpan.FromMilliseconds(2000), () =>
            //        {
            //            Task.Factory.StartNew(async () =>
            //            {
            //                var context = ((ReceivableListPageViewModel)this.BindingContext);
            //                await context.InitDebtorsAsync(searchText);
            //            });
            //            return false;
            //        });
            //    }
            //}
            //else
            //{
            //    autoCompleteView.ItemsSource = null;
            //}

        }

        private void OnSuggestionItemSelected(object sender, Telerik.XamarinForms.Input.AutoComplete.SuggestionItemSelectedEventArgs e)
        {
            //this.viewModel.ShowDetails((Movie)e.DataItem);
        }

    }
}