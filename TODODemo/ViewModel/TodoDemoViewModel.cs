using System;
using System.Threading.Tasks;
using TODODemo.View;
using Xamarin.Forms;

namespace TODODemo.ViewModel
{
    public class TodoDemoViewModel: BaseViewModel
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged();

                AddTaskCommand.ChangeCanExecute();
            }
        }

        public Command AddTaskCommand { get; set; }

        public TodoDemoViewModel()
        {
            AddTaskCommand = new Command(async () => await AddTask(), () => !IsBusy);
        }

        private async Task AddTask()
        {
            if (IsBusy)
                return;

            Exception error = null;

            try
            {
                IsBusy = true;

                var addTask = new AddTaskPage();
                await Application.Current.MainPage.Navigation.PushAsync(addTask);
            }
            catch(Exception ex)
            {
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error", error.Message, "OK");
        }
    }
}
