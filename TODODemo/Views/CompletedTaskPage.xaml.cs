using System;
using System.Collections.Generic;
using TODODemo.ViewModel;
using Xamarin.Forms;

namespace TODODemo.Views
{
    public partial class CompletedTaskPage : ContentPage
    {
        TasksViewModel _tasksViewModel;

        public CompletedTaskPage(TasksViewModel tasksViewModel)
        {
            InitializeComponent();

            _tasksViewModel = tasksViewModel;
            BindingContext = _tasksViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _tasksViewModel.Title = "Completed";
            _tasksViewModel.LoadData();
        }
    }
}
