using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using RCM.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Input;
using Telerik.XamarinForms.Input.DataForm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RCM.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivableDetailPage : ContentPage
    {
        ISettingsService _settingsService;
        public ReceivableDetailPage(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            InitializeComponent();
        }
        private async void FinishPopup(object sender, EventArgs e)
        {
            var context = ((ReceivableDetailPageViewModel)this.BindingContext);
            context.Receivable.CollectionProgressStatus = 5;
            await context._receivableService.CloseReceivable(_settingsService.AuthAccessToken, new ReceivableCloseModel()
            {
                Id = context.Receivable.Id,
                isPayed = true
            });
            popup.IsOpen = false;
        }

        private async void ClosePopup(object sender, EventArgs e)
        {
            var context = ((ReceivableDetailPageViewModel)this.BindingContext);
            context.Receivable.CollectionProgressStatus = 0;

            await context._receivableService.CloseReceivable(_settingsService.AuthAccessToken, new ReceivableCloseModel()
            {
                Id = context.Receivable.Id,
                isPayed = false
            });
            popup.IsOpen = false;
        }
        private void Close(object sender, EventArgs e)
        {
            popup.IsOpen = false;
        }
        private void ShowPopup(object sender, EventArgs e)
        {
            var context = (ReceivableDetailPageViewModel)this.BindingContext;
            if (context.Receivable.CollectionProgressStatus == 1)
            {
                popup.IsOpen = true;
            }
        }
    }
}