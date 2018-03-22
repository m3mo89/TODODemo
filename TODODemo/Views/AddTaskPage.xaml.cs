using System;
using System.Collections.Generic;
using TODODemo.Data.Models;
using TODODemo.ViewModel;
using Xamarin.Forms;

namespace TODODemo.Views
{
    public partial class AddTaskPage : ContentPage
    {
        AddTaskViewModel addTaskViewModel;
        TodoItem item;

        public AddTaskPage(TodoItem item=null)
        {
            InitializeComponent();

            this.item = item;
            addTaskViewModel = new AddTaskViewModel();
            BindingContext = addTaskViewModel;
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();
            addTaskViewModel.TodoItem = item;
            addTaskViewModel.LoadData();
		}
	}
}
