using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TODODemo.Data.Managers;
using TODODemo.Data.Models;
using TODODemo.DependecyServices;
using TODODemo.Views;
using Xamarin.Forms;

namespace TODODemo.ViewModel
{
    public class TasksViewModel:BaseViewModel
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

                ShareTaskCommand.ChangeCanExecute();
                AddTaskCommand.ChangeCanExecute();
                SearchCommand.ChangeCanExecute();
                ShowImageItemCommand.ChangeCanExecute();
                EditTaskCommand.ChangeCanExecute();
            }
        }


        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");

                Search(SearchText);
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

        private string _title;
        public string Title
        {
            get => _title;
            set 
            { 
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        private ObservableCollection<TodoItem> _items;
        public ObservableCollection<TodoItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged("Items");
                OnPropertyChanged("HasItems");
                OnPropertyChanged("NoHasItems");
            }
        }

        public bool HasItems
        {
            get
            {
                if (Items != null)
                    return Items.Count > 0;
                else
                    return false;
            }
        }

        public bool NoHasItems
        {
            get
            {
                return !HasItems;
            }
        }

        public Command ShowImageItemCommand { get; set; }
        public Command EditTaskCommand { get; set; }
        public Command ShareTaskCommand { get; set; }
        public Command SearchCommand { get; set; }
        public Command AddTaskCommand { get; set; }

        public TasksViewModel()
        {
            _manager = new TodoItemManager();
            Items = new ObservableCollection<TodoItem>();

            ShowImageItemCommand = new Command(async (id) => await ShowImageItem(id), (id) => !IsBusy);
            EditTaskCommand = new Command<TodoItem>(async (item) => await EditTask(item), (item) => !IsBusy);
            ShareTaskCommand = new Command(async (item) => await ShareTask(item), (item) => !IsBusy);
            SearchCommand = new Command(async (text) => await Search(text), (text) => !IsBusy);
            AddTaskCommand = new Command(async () => await AddTask(), () => !IsBusy);
        }

        public async void LoadData()
        {
            if (IsBusy)
                return;

            Exception error = null;

            try
            {
                IsBusy = true;

                if (Title == "Pending")
                {
                    Items = new ObservableCollection<TodoItem>(await _manager.GetAllPendingTaskAsync());
                }else
                    Items = new ObservableCollection<TodoItem>(await _manager.GetAllCompletedTaskAsync());
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

        private async Task ShowImageItem(object id)
        {
            if (IsBusy)
                return;

            if (id == null)
                return;

            Exception error = null;

            try
            {
                IsBusy = true;

                int idItem = 0;

                int.TryParse(id.ToString(), out idItem);

                ShowImagePage page = new ShowImagePage(idItem);

                await Application.Current.MainPage.Navigation.PushModalAsync(page);

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


        private async Task EditTask(TodoItem item)
        {
            if (IsBusy)
                return;

            if (item == null)
                return;

            Exception error = null;

            try
            {
                IsBusy = true;

                AddTaskPage page = new AddTaskPage(item);

                await Application.Current.MainPage.Navigation.PushAsync(page);

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

        private async Task ShareTask(object item)
        {
            if (IsBusy)
                return;

            Exception error = null;

            try
            {
                IsBusy = true;

                var todoItem = (TodoItem)item;

                DependencyService.Get<IShareFile>().ShareLocalFile(todoItem);

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

        private async Task Search(object text)
        {
            if (IsBusy)
                return;

            if (text == null)
                return;


            Exception error = null;

            try
            {
                IsBusy = true;

                var busqueda = text.ToString();

                var status = string.Equals(Title, "Pending") ? StatusType.Pending : StatusType.Completed;

                if(!string.IsNullOrEmpty(SearchText) && !string.IsNullOrWhiteSpace(SearchText))
                    Items = new ObservableCollection<TodoItem>(await _manager.GetTaskByContentAndStatus(busqueda, status));
                else if(!string.IsNullOrEmpty(SearchText) && status == StatusType.Pending)
                    Items = new ObservableCollection<TodoItem>(await _manager.GetAllPendingTaskAsync());
                else if (!string.IsNullOrEmpty(SearchText) && status == StatusType.Completed)
                    Items = new ObservableCollection<TodoItem>(await _manager.GetAllCompletedTaskAsync());
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
