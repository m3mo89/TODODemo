using System;
using System.Collections.Generic;
using TODODemo.ViewModel;
using Xamarin.Forms;

namespace TODODemo.View
{
    public partial class ShowImagePage : ContentPage
    {
        ShowImageViewModel showImageViewModel;

        public ShowImagePage(int id)
        {
            InitializeComponent();

            showImageViewModel = new ShowImageViewModel();
            showImageViewModel.LoadData(id);

            BindingContext = showImageViewModel;
        }
    }
}
