using System;
using System.Collections.Generic;
using TODODemo.ViewModel;
using Xamarin.Forms;

namespace TODODemo.Views
{
    public partial class PendingTaskPage : ContentPage
    {
        TasksViewModel _tasksViewModel;

        public PendingTaskPage(TasksViewModel tasksViewModel)
        {
            InitializeComponent();
            _tasksViewModel = tasksViewModel;
            BindingContext = _tasksViewModel;
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();


            _tasksViewModel.Title = "Pending";
            _tasksViewModel.LoadData();
		}
	}
}
