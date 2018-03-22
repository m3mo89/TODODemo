using System;
using System.Threading.Tasks;
using TODODemo.Data.Managers;
using TODODemo.Data.Models;
using Xamarin.Forms;

namespace TODODemo.ViewModel
{
    public class AddTaskViewModel : BaseViewModel
    {
        private TodoItemManager _manager;

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

                SaveTaskCommand.ChangeCanExecute();
            }
        }

        private string _content;
        public string Content 
        { 
            get => _content; 
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        public Command SaveTaskCommand { get; set; }

        public AddTaskViewModel()
        {
            SaveTaskCommand = new Command(async () => await SaveTask(), () => !IsBusy);
        }

        private async Task SaveTask()
        {
            if (IsBusy)
                return;

            Exception error = null;

            try
            {
                IsBusy = true;

                _manager = new TodoItemManager();

                TodoItem item = new TodoItem();
                item.Content = Content;
                item.Status = StatusType.Pending;
                item.Image = string.Empty;
                item.LastModified = DateTime.Now;

                var result = await _manager.SaveOrUpdateInDBAsync(item);

                if (result)
                    await Application.Current.MainPage.Navigation.PopAsync();
                else
                    throw new Exception("An error ocurred while saving the data");
            }
            catch (Exception ex)
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
