using System;
using System.Collections.Generic;
using TODODemo.ViewModel;
using Xamarin.Forms;

namespace TODODemo.View
{
    public partial class CompletedTaskPage : ContentPage
    {
        TasksViewModel tasksViewModel;

        public CompletedTaskPage()
        {
            InitializeComponent();

            tasksViewModel = new TasksViewModel();
            BindingContext = tasksViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            tasksViewModel.Title = "Completed";
            tasksViewModel.LoadData();
        }
    }
}
