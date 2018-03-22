using System;
using System.Collections.Generic;
using TODODemo.ViewModel;
using Xamarin.Forms;

namespace TODODemo.View
{
    public partial class AddTaskPage : ContentPage
    {
        public AddTaskPage()
        {
            InitializeComponent();
            BindingContext = new AddTaskViewModel();
        }
    }
}
