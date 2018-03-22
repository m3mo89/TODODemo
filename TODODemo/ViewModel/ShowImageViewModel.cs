using System;
using System.Threading.Tasks;
using TODODemo.Data.Managers;
using Xamarin.Forms;

namespace TODODemo.ViewModel
{
    public class ShowImageViewModel:BaseViewModel
    {
        TodoItemManager _manager;

        private string imageItem;

        public string ImageItem 
        { 
            get => imageItem; 
            set
            { 
                imageItem = value;
                OnPropertyChanged("ImageItem");
            } 
        }

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

                CloseWindowCommand.ChangeCanExecute();
            }
        }

        public Command CloseWindowCommand { get; set; }


        public ShowImageViewModel()
        {
            _manager = new TodoItemManager();

            CloseWindowCommand = new Command(async () => await CloseWindow(), () => !IsBusy);
        }

        public async void LoadData(int id)
        {
            Exception error = null;

            try
            {
                var item = await _manager.GetTaskById(id);

                ImageItem = item.Image;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error", error.Message, "OK");
        }

        private async Task CloseWindow()
        {
            if (IsBusy)
                return;

            Exception error = null;

            try
            {
                IsBusy = true;

                await Application.Current.MainPage.Navigation.PopModalAsync();

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
