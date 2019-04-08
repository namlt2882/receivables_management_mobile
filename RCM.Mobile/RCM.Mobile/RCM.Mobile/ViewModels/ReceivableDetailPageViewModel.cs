using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Permissions.Abstractions;
using Plugin.ShareFile;
using ScanbotSDK.Xamarin.Forms;
using System.IO;
using System.Text;
using Telerik.XamarinForms.DataControls.ListView.Commands;
namespace RCM.Mobile.ViewModels
{
    public class ReceivableDetailPageViewModel : BaseAuthenticatedViewModel
    {

        public IReceivableService _receivableService;
        public ITaskService _taskService;

        public ReceivableDetailPageViewModel(
            ISettingsService settingsService,
            IPageDialogService dialogService,
            INavigationService navigationService,
            IReceivableService receivableService,
            ITaskService taskService
            ) : base(settingsService, dialogService, navigationService)
        {
            _receivableService = receivableService;
            _taskService = taskService;
            Title = "Receivable Detail";

            #region Task
            #endregion
        }
        #region Receivable

        private List<Contact> _relatives;
        public List<Contact> Relatives
        {
            get { return _relatives; }
            set { SetProperty(ref _relatives, value); RaisePropertyChanged("Relatives"); }
        }

        private Receivable _receivable;
        public Receivable Receivable
        {
            get { return _receivable; }
            set { SetProperty(ref _receivable, value); RaisePropertyChanged("Receivable"); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); RaisePropertyChanged("IsBusy"); }
        }
        //public ObservableCollection<Receivable> Receivables { get; set; }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            #region Receivable
            IsBusy = true;
            if (parameters.ContainsKey("receivableId"))
            {
                var receivableId = parameters.GetValue<int>("receivableId");
                Receivable = await _receivableService.GetAssignedReceivableAsync(_settingsService.AuthAccessToken, receivableId);
                _settingsService.ReceivableId = Receivable.Id;
            }
            #region Task
            await InitAsync();
            #endregion
            

            //Get Relatives
            Relatives = new List<Contact>();
            Receivable.Contacts.Skip(1).ToList().ForEach(c =>
            {
                Relatives.Add(c);
            });
            #endregion

            IsBusy = false;

            base.OnNavigatedTo(parameters);
        }
        private async System.Threading.Tasks.Task InitAsync()
        {
            TodayTasks = new ObservableCollection<Task>();
            foreach (var item in await _taskService.GetAssignedTaskByReceivableAndDay(_settingsService.AuthAccessToken, 0, _settingsService.ReceivableId))
            {
                TodayTasks.Add(item);
            }
            ToDoTasks = new ObservableCollection<Task>();
            foreach (var task in await _taskService.GetTodoTaskByReceivableId(_settingsService.AuthAccessToken, _settingsService.ReceivableId))
            {
                ToDoTasks.Add(task);
            }
            CompletedTasks = new ObservableCollection<Task>();
            foreach (var task in await _taskService.GetCompletedTaskByReceivableId(_settingsService.AuthAccessToken, _settingsService.ReceivableId))
            {
                CompletedTasks.Add(task);
            }
            SetTaskAvailable();
        }
        //public override void OnNavigatedFrom(INavigationParameters parameters)
        //{
        //    base.OnNavigatedFrom(parameters);
        //}

        //public Command<bool> UpdateReceivableStatus
        //{
        //    get
        //    {
        //        return new Command<bool>(async (Finish) =>
        //        {
        //            //await _receivableService.CloseReceivable(_settingsService.AuthAccessToken, new ReceivableCloseModel()
        //            //{
        //            //    Id = Receivable.Id,
        //            //    isPayed = Finish
        //            //});
        //            if (Finish)
        //            {
        //                Receivable.CollectionProgressStatus = Constant.COLLECTION_STATUS_CLOSED_CODE;
        //            }
        //            else
        //            {
        //                Receivable.CollectionProgressStatus = Constant.COLLECTION_STATUS_CANCEL_CODE;
        //            }
        //        });
        //    }
        //}

        public Command PhoneCommand
        {
            get
            {
                return new Command(() =>
                {
                    Plugin.Messaging.CrossMessaging.Current.PhoneDialer.MakePhoneCall(Receivable.Contacts[0].Phone, Receivable.Contacts[0].Name);
                });
            }
        }
        public Command MessagingCommand
        {
            get
            {
                return new Command(() =>
                {
                    Plugin.Messaging.CrossMessaging.Current.SmsMessenger.SendSms(Receivable.Contacts[0].Phone, "");
                });
            }
        }
        public Command DirectionCommand
        {
            get
            {
                return new Command(() =>
                {
                    var uri = new Uri($"http://maps.google.com/maps?daddr={Receivable.Contacts[0].Address}&directionsmode=transit");
                    Device.OpenUri(uri);

                });
            }
        }

        public Command<string> PhoneRelativeCommand
        {
            get
            {
                return new Command<string>((phone) =>
                {
                    Plugin.Messaging.CrossMessaging.Current.PhoneDialer.MakePhoneCall(phone, "");
                });
            }
        }
        public Command<string> MessagingRelativeCommand
        {
            get
            {
                return new Command<string>((phone) =>
                {
                    Plugin.Messaging.CrossMessaging.Current.SmsMessenger.SendSms(phone, "");
                });
            }
        }
        public Command<string> DirectionRelativeCommand
        {
            get
            {
                return new Command<string>((address) =>
                {
                    var uri = new Uri($"http://maps.google.com/maps?daddr={address}&directionsmode=transit");
                    Device.OpenUri(uri);

                });
            }
        }
        public Command Edit
        {
            get
            {
                return new Command(async () =>
                {
                    if (Receivable.CollectionProgressStatus != Constant.COLLECTION_STATUS_COLLECTION_CODE)
                    {
                        await _dialogService.DisplayActionSheetAsync("Update Receivable Status", "OK", null, null);
                    }
                    else
                    {
                        var answer = await _dialogService.DisplayActionSheetAsync("Update Receivable Status", "Cancel", null, "Finish", "Stop");
                        switch (answer)
                        {
                            case "Finish":
                                await _receivableService.CloseReceivable(_settingsService.AuthAccessToken, new ReceivableCloseModel()
                                {
                                    Id = Receivable.Id,
                                    isPayed = true
                                });
                                Receivable.CollectionProgressStatus = Constant.COLLECTION_STATUS_CLOSED_CODE;
                                break;
                            case "Stop":
                                await _receivableService.CloseReceivable(_settingsService.AuthAccessToken, new ReceivableCloseModel()
                                {
                                    Id = Receivable.Id,
                                    isPayed = false
                                });
                                Receivable.CollectionProgressStatus = Constant.COLLECTION_STATUS_CANCEL_CODE;
                                break;
                        }
                    }
                });
            }
        }
        public Command TapToDayTasksCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await NavigationService.NavigateAsync("ReceivableTaskListPage");
                });
            }
        }
        #endregion


        #region Task
        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); RaisePropertyChanged("ImageSource"); }
        }
        private ImageSource _completedTaskImageSource;
        public ImageSource CompletedTaskImageSource
        {
            get { return _completedTaskImageSource; }
            set { SetProperty(ref _completedTaskImageSource, value); RaisePropertyChanged("CompletedTaskImageSource"); }
        }
        private Models.File _file;
        public Models.File File
        {
            get { return _file; }
            set { SetProperty(ref _file, value); RaisePropertyChanged("File"); }
        }
        private Task _selectedTask;
        public Task SelectedTask
        {
            get { return _selectedTask; }
            set { SetProperty(ref _selectedTask, value); RaisePropertyChanged("SelectedTask"); }
        }

        private bool _isImageAvailable;
        public bool IsImageAvailable
        {
            get { return _isImageAvailable; }
            set { SetProperty(ref _isImageAvailable, value); RaisePropertyChanged("IsImageAvailable"); }
        }
        private bool _isOpened;
        public bool IsOpened
        {
            get { return _isOpened; }
            set { SetProperty(ref _isOpened, value); RaisePropertyChanged("IsOpened"); }
        }
        private bool _isCompltedOpened;
        public bool IsCompletedOpened
        {
            get { return _isCompltedOpened; }
            set { SetProperty(ref _isCompltedOpened, value); RaisePropertyChanged("IsCompletedOpened"); }
        }
        private ObservableCollection<Task> _todayTasks;
        public ObservableCollection<Task> TodayTasks
        {
            get { return _todayTasks; }
            set { SetProperty(ref _todayTasks, value); RaisePropertyChanged("TodayTasks"); }
        }

        private bool _hasToDayTasks;
        public bool HasToDayTasks
        {
            get { return _hasToDayTasks; }
            set { SetProperty(ref _hasToDayTasks, value); RaisePropertyChanged("HasToDayTasks"); }
        }
        private ObservableCollection<SourceItem> _Source;
        public ObservableCollection<SourceItem> Source
        {
            get { return _Source; }
            set { SetProperty(ref _Source, value); RaisePropertyChanged("Source"); }
        }

        private ObservableCollection<Task> _completedTasks;
        public ObservableCollection<Task> CompletedTasks
        {
            get { return _completedTasks; }
            set { SetProperty(ref _completedTasks, value); RaisePropertyChanged("CompletedTasks"); }
        }
        private bool _hasCompletedTasks;
        public bool HasCompletedTasks
        {
            get { return _hasCompletedTasks; }
            set { SetProperty(ref _hasCompletedTasks, value); RaisePropertyChanged("HasCompletedTasks"); }
        }

        private ObservableCollection<Task> _toDoTasks;
        public ObservableCollection<Task> ToDoTasks
        {
            get { return _toDoTasks; }
            set { SetProperty(ref _toDoTasks, value); RaisePropertyChanged("ToDoTasks"); }
        }
        private bool _hasToDoTasks;
        public bool HasToDoTasks
        {
            get { return _hasToDoTasks; }
            set { SetProperty(ref _hasToDoTasks, value); RaisePropertyChanged("HasToDoTasks"); }
        }


        private void SetTaskAvailable()
        {
            if (TodayTasks.Count > 0)
            {
                HasToDayTasks = true;
            }
            else
            {
                HasToDayTasks = false;
            }
            if (ToDoTasks.Count > 0)
            {
                HasToDoTasks = true;
            }
            else
            {
                HasToDoTasks = false;
            }
            if (CompletedTasks.Count > 0)
            {
                HasCompletedTasks = true;
            }
            else
            {
                HasCompletedTasks = false;

            }
        }
        public Command ClosePopup
        {
            get
            {
                return new Command(() =>
                {
                    IsOpened = false;
                    IsCompletedOpened = false;
                });
            }
        }
        public Command OpenPopup
        {
            get
            {
                return new Command<ItemTapCommandContext>((_) =>
                {
                    var task = _.Item as Models.Task;
                    SelectedTask = TodayTasks.First(t => t.Id == task.Id);
                    IsOpened = true;
                    //await NavigationService.NavigateAsync("TaskDetailPage", new NavigationParameters() { { "task", task } });
                });
            }
        }
        public Command ViewEvidence
        {
            get
            {
                return new Command((_) =>
                {
                    Device.OpenUri(new Uri($"{_settingsService.Url()}{SelectedTask.Evidence}"));
                    //await NavigationService.NavigateAsync("TaskDetailPage", new NavigationParameters() { { "task", task } });
                });
            }
        }
        public Command OpenCompletedTaskPopup
        {
            get
            {
                return new Command<ItemTapCommandContext>((_) =>
                {
                    var task = _.Item as Models.Task;
                    SelectedTask = CompletedTasks.First(t => t.Id == task.Id);
                    IsCompletedOpened = true;
                    //await NavigationService.NavigateAsync("TaskDetailPage", new NavigationParameters() { { "task", task } });
                });
            }
        }
        public Command ScanPhoto
        {
            get
            {
                return new Command(async () =>
                {
                    //await CrossMedia.Current.Initialize();
                    await Utility.GrantPermisionAsync(Permission.Photos, _dialogService);
                    await Utility.GrantPermisionAsync(Permission.Storage, _dialogService);
                    if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
                    {
                        await _dialogService.DisplayAlertAsync("No camera", "No camera available", "OK");
                        return;
                    }
                    var config = new DocumentScannerConfiguration();
                    var result = await SBSDK.UI.LaunchDocumentScannerAsync(config);
                    if (result.Status == OperationResult.Ok)
                    {
                        var croppongConfig = new CroppingScreenConfiguration();
                        var croppingResult = await SBSDK.UI.LaunchCroppingScreenAsync(result.Pages[0], croppongConfig);
                        if (croppingResult.Status == OperationResult.Ok)
                        {
                            // `page` has been updated
                            IEnumerable<ImageSource> DocumentSources = result.Pages.Select(p => p.Document).Where(image => image != null);
                            var fileUri = await SBSDK.Operations.CreatePdfAsync(DocumentSources, ScanbotSDK.Xamarin.PDFPageSize.Auto);
                            //Move file to device
                            CrossShareFile.Current.ShareLocalFile(fileUri.AbsolutePath);
                            File = new Models.File
                            {
                                FileName = Path.GetFileName(fileUri.LocalPath),
                                StreamSource = System.IO.File.OpenRead(fileUri.AbsolutePath)
                            };
                            ImageSource = result.Pages[0].Document;
                            IsImageAvailable = true;
                        }
                    }
                });
            }
        }
        public Command Update
        {
            get
            {
                return new Command(async () =>
                {
                    if (await _dialogService.DisplayAlertAsync("Mesage", "Are you sure to Update this task?", "OK", "Cancel"))
                    {
                        if (await _taskService.Update(_settingsService.AuthAccessToken, new UpdateTaskModel()
                        {
                            Id = SelectedTask.Id,
                            Note = SelectedTask.Note,
                            File = File,
                            Status = Constant.COLLECTION_STATUS_DONE_CODE
                        }))
                        {
                            await _dialogService.DisplayAlertAsync("Mesage", "Update Success", "Ok");
                            TodayTasks.Remove(SelectedTask);
                            IsImageAvailable = false;
                            ImageSource = null;
                            await InitAsync();
                        }
                        else
                        {
                            await _dialogService.DisplayAlertAsync("Mesage", "Update Fail", "Ok");
                        }
                        IsOpened = false;
                    }

                });
            }
        }
        public Command Cancel
        {
            get
            {
                return new Command(async () =>
                {
                    if (await _dialogService.DisplayAlertAsync("Mesage", "Are you sure to cancel this task?", "OK", "Cancel"))
                    {


                        if (await _taskService.Update(_settingsService.AuthAccessToken, new UpdateTaskModel()
                        {
                            Id = SelectedTask.Id,
                            Note = SelectedTask.Note,
                            File = File,
                            Status = Constant.COLLECTION_STATUS_CANCEL_CODE
                        }))
                        {
                            await _dialogService.DisplayAlertAsync("Mesage", "Cancel Success", "Ok");
                            TodayTasks.Remove(SelectedTask);
                            IsImageAvailable = false;
                            ImageSource = null;
                            await InitAsync();
                        }
                        else
                        {
                            await _dialogService.DisplayAlertAsync("Mesage", "Fail", "Ok");
                        }
                    }
                });
            }
        }
        #endregion
    }
}
