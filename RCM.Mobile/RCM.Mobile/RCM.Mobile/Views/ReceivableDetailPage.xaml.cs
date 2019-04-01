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
            InitDataForm();

        }
        void InitDataForm()
        {
            var dataForm = new RadDataForm
            {
                Source = new Receivable()
            };
            //this.dataForm.PropertyDataSourceProvider = new UserPropertyDataSourceProvider();
            //dataForm.GroupLayoutDefinition = new DataFormGroupGridLayoutDefinition();
            //var style = new DataFormEditorStyle
            //{
            //    FeedbackFontSize = 10,
            //    PositiveFeedbackBackground = Color.Transparent,
            //};

            //foreach (var property in typeof(Receivable).GetTypeInfo().DeclaredProperties)
            //{
            //    this.dataForm.RegisterEditor(property.Name, EditorType.Custom);
            //}

            //dataForm.RegisterEditor("", EditorType.IntegerEditor);
            dataForm.RegisterEditor("DebtAmount", EditorType.DecimalEditor);
            dataForm.RegisterEditor("PayableDay", EditorType.DateEditor);
            dataForm.RegisterEditor("PrepaidAmount", EditorType.DecimalEditor);
            dataForm.RegisterEditor("ClosedDay", EditorType.DateEditor);
            dataForm.RegisterEditor("CustomerName", EditorType.TextEditor);
            dataForm.RegisterEditor("DebtorName", EditorType.TextEditor);
            dataForm.RegisterEditor("AssignDate", EditorType.TextEditor);
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