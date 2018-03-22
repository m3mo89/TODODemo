using System;
using System.Collections.Generic;
using TODODemo.ViewModel;
using Xamarin.Forms;

namespace TODODemo.Views
{
    public partial class PendingTaskPage : ContentPage
    {
        TasksViewModel tasksViewModel;
        public PendingTaskPage()
        {
            InitializeComponent();
            tasksViewModel = new TasksViewModel();
            BindingContext = tasksViewModel;
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();

            tasksViewModel.Title = "Pending";
            tasksViewModel.LoadData();
		}
	}
}
