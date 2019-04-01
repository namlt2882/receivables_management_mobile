using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.ShareFile;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using ScanbotSDK.Xamarin;
using ScanbotSDK.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{
    public class TaskDetailPageViewModel : BaseAuthenticatedViewModel
    {
        private ITaskService _taskService;
        private MediaFile _mediaFile;
        public TaskDetailPageViewModel(ISettingsService settingsService, IPageDialogService dialogService, INavigationService navigationService, ITaskService taskService) : base(settingsService, dialogService, navigationService)
        {
            _taskService = taskService;
        }
        private bool _updateIsAvailable;
        public bool UpdateIsAvailable
        {
            get { return _updateIsAvailable; }
            set { SetProperty(ref _updateIsAvailable, value); RaisePropertyChanged("UpdateIsAvailable"); }
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); RaisePropertyChanged("ImageSource"); }
        }
        private Models.File _file;
        public Models.File File
        {
            get { return _file; }
            set { SetProperty(ref _file, value); RaisePropertyChanged("File"); }
        }
        private Task _task;
        public Task Task
        {
            get { return _task; }
            set { SetProperty(ref _task, value); RaisePropertyChanged("Task"); }
        }
        //public ObservableCollection<Task> Tasks { get; set; }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var task = parameters.GetValue<Task>("task");
            if (task == null) return;
            Task = task;
            UpdateIsAvailable = false;
            base.OnNavigatedTo(parameters);
        }
        public Command PickPhoto
        {
            get
            {
                return new Command(async () =>
                {
                    await CrossMedia.Current.Initialize();

                    await Utility.GrantPermisionAsync(Permission.Photos, _dialogService);
                    await Utility.GrantPermisionAsync(Permission.Storage, _dialogService);

                    _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                    if (_mediaFile == null)
                    {
                        return;
                    }
                    File = new Models.File();
                    File.FileName = _mediaFile.Path;
                    File.StreamSource = _mediaFile.GetStream();
                    ImageSource = ImageSource.FromStream(() => { return _mediaFile.GetStream(); });
                    UpdateIsAvailable = true;
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
                            UpdateIsAvailable = true;
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
                    if (await _taskService.Update(_settingsService.AuthAccessToken, new UpdateTaskModel()
                    {
                        Id = Task.Id,
                        Note = Task.Note,
                        File = File
                    }))
                    {
                        await _dialogService.DisplayAlertAsync("Mesage", "Update Success", "Ok");
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("Mesage", "Update Fail", "Ok");
                    }
                });
            }
        }
        public Command ViewReceivable
        {
            get
            {
                return new Command<int>(async (receivableId) =>
                {
                    var navigationParams = new NavigationParameters();
                    navigationParams.Add("receivableId", receivableId);
                    await NavigationService.NavigateAsync("ReceivableDetailPage", navigationParams);
                });
            }
        }
    }
}
