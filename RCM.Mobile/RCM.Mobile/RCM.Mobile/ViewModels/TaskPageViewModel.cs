using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Services;
using Telerik.XamarinForms.DataControls.ListView.Commands;
using Telerik.XamarinForms.Input;
using Telerik.XamarinForms.Input.Calendar.Commands;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{
    public class TaskPageViewModel : BaseAuthenticatedViewModel
    {
        private ITaskService _taskService;

        #region CurrentTask


        private DateTime _minDate;
        public DateTime MinDate
        {
            get { return _minDate; }
            set { SetProperty(ref _minDate, value); RaisePropertyChanged("MinDay"); }
        }

        private DateTime _maxDate;
        public DateTime MaxDate
        {
            get { return _maxDate; }
            set { SetProperty(ref _maxDate, value); RaisePropertyChanged("MaxDay"); }
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); RaisePropertyChanged("SelectedDate"); }
        }


        private ObservableCollection<Appointment> _currentAppointments;
        public ObservableCollection<Appointment> CurrentAppointments
        {
            get { return _currentAppointments; }
            set { SetProperty(ref _currentAppointments, value); RaisePropertyChanged("CurrentAppointments"); }
        }

        private ObservableCollection<Models.Task> _currentTasksByDay;
        public ObservableCollection<Models.Task> CurrentTasksByDay
        {
            get { return _currentTasksByDay; }
            set { SetProperty(ref _currentTasksByDay, value); RaisePropertyChanged("CurrentTasksByDay"); }
        }

        #endregion

        public TaskPageViewModel(ISettingsService settingsService, IPageDialogService dialogService, INavigationService navigationService, ITaskService taskService) : base(settingsService, dialogService, navigationService)
        {
            _taskService = taskService;

        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await Init();
            base.OnNavigatedTo(parameters);
        }
        private async System.Threading.Tasks.Task Init()
        {
            CurrentAppointments = new ObservableCollection<Appointment>();
            var appointments = await _taskService.GetCollectorCalendarTasks(_settingsService.AuthAccessToken);
            appointments.ForEach(appointment =>
            {
                CurrentAppointments.Add(new Appointment()
                {
                    StartDate = appointment,
                    EndDate = appointment,
                    Color = Color.Red,
                    Title = "",
                    Detail = "",
                });
            });
            SetMinMaxDay(appointments);
            //
            await SetCurrentTaskByDay(null);
        }

        private void SetMinMaxDay(List<DateTime> appointments)
        {
            appointments.Sort();
            if (appointments.Count > 0)
            {
                MaxDate = appointments[appointments.Count - 1];
                MinDate = appointments[0];
            }
            
        }

        public Command TapDay
        {
            get
            {
                return new Command<CalendarDateCell>(async (calendarDate) =>
                {
                    if (calendarDate.Date.CompareTo(MinDate) >= 0 || calendarDate.Date.CompareTo(MaxDate) <= 0) { }
                        await SetCurrentTaskByDay(calendarDate.Date);
                });
            }
        }
        private async Task SetCurrentTaskByDay(DateTime? date)
        {
            SelectedDate = date;
            var currentDay = Utility.ConvertDatimeToInt(date);
            var currentTasksByDay = await _taskService.GetAssignedTaskByDay(_settingsService.AuthAccessToken, currentDay);
            CurrentTasksByDay = new ObservableCollection<Models.Task>();
            foreach (var item in currentTasksByDay)
            {
                CurrentTasksByDay.Add(item);
            }
        }
        public Command TapTask
        {
            get
            {
                return new Command<ItemTapCommandContext>(async (_) =>
               {
                   var task = _.Item as Models.Task;
                   await NavigationService.NavigateAsync("ReceivableDetailPage", new NavigationParameters() { { "receivableId", task.ReceivableId} });
               });
            }
        }
    }
}
