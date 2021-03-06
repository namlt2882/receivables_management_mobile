﻿using Plugin.Media;
using Plugin.Permissions.Abstractions;
using Plugin.ShareFile;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using ScanbotSDK.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.XamarinForms.DataControls.ListView.Commands;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{
    public class ReceivableTaskListPageViewModel : BaseAuthenticatedViewModel
    {
        private ITaskService _taskService;

        public ReceivableTaskListPageViewModel(ISettingsService settingsService, IPageDialogService dialogService, INavigationService navigationService, ITaskService taskService) : base(settingsService, dialogService, navigationService)
        {
            _taskService = taskService;
            InitAsync();
        }
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

        private ObservableCollection<SourceItem> _Source;
        public ObservableCollection<SourceItem> Source
        {
            get { return _Source; }
            set { SetProperty(ref _Source, value); RaisePropertyChanged("Source"); }
        }
        private ObservableCollection<SourceItem> _Source2;
        public ObservableCollection<SourceItem> Source2
        {
            get { return _Source2; }
            set { SetProperty(ref _Source2, value); RaisePropertyChanged("Source2"); }
        }
        //public ObservableCollection<SourceItem> Source { get; set; }
        //public ObservableCollection<SourceItem> Source2 { get; set; }
        private ObservableCollection<Task> _completedTasks;
        public ObservableCollection<Task> CompletedTasks
        {
            get { return _completedTasks; }
            set { SetProperty(ref _completedTasks, value); RaisePropertyChanged("CompletedTasks"); }
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await InitAsync();
            base.OnNavigatedTo(parameters);
        }

        private ObservableCollection<Task> _toDoTasks;
        public ObservableCollection<Task> ToDoTasks
        {
            get { return _toDoTasks; }
            set { SetProperty(ref _toDoTasks, value); RaisePropertyChanged("ToDoTasks"); }
        }
        private async System.Threading.Tasks.Task InitAsync()
        {
            TodayTasks = new ObservableCollection<Task>();
            foreach (var item in await _taskService.GetAssignedTaskByReceivableAndDay(_settingsService.AuthAccessToken, 0, _settingsService.ReceivableId))
            {
                TodayTasks.Add(item);
            }
            ToDoTasks = new ObservableCollection<Task>();
            CompletedTasks = new ObservableCollection<Task>();
            foreach (var task in await _taskService.GetTaskByReceivableId(_settingsService.AuthAccessToken, _settingsService.ReceivableId))
            {
                if (task.Status == Constant.COLLECTION_STATUS_DONE_CODE || task.Status == Constant.COLLECTION_STATUS_CANCEL_CODE)
                    CompletedTasks.Add(task);
                else ToDoTasks.Add(task);
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
                            File = new Models.File();
                            File.FileName = Path.GetFileName(fileUri.LocalPath);
                            File.StreamSource = System.IO.File.OpenRead(fileUri.AbsolutePath);
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
                            File = File
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
                        await _taskService.Cancel(_settingsService.AuthAccessToken, SelectedTask.Id);
                        await _dialogService.DisplayAlertAsync("Mesage", "Cancel Success", "OK");
                        TodayTasks.Remove(SelectedTask);
                        IsImageAvailable = false;
                        ImageSource = null;
                        await InitAsync();
                        IsOpened = false;
                    }
                });
            }
        }
    }
    public class SourceItem
    {
        public SourceItem(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
