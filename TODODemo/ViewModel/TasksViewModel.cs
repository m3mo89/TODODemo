﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TODODemo.Data.Managers;
using TODODemo.Data.Models;
using TODODemo.View;
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

        public TasksViewModel()
        {
            _manager = new TodoItemManager();
            Items = new ObservableCollection<TodoItem>();

            ShowImageItemCommand = new Command(async (id) => await ShowImageItem(id), (id) => !IsBusy);
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
    }
}
