using TODODemo.Renderers;
using TODODemo.ViewModel;
using TODODemo.Views;
using Xamarin.Forms;

namespace TODODemo
{
    public partial class TODODemoPage : SearchPage
    {
        TasksViewModel tasksViewModel;

        public TODODemoPage()
        {
            InitializeComponent();

            tasksViewModel = new TasksViewModel();

            BindingContext = tasksViewModel;

            Children.Add(new PendingTaskPage(tasksViewModel){ Title="Pending", Icon = "ic_list_white"});
            Children.Add(new CompletedTaskPage(tasksViewModel){Title="Completed", Icon = "ic_playlist_add_check_white"});
        }

		protected override void OnAppearing()
		{
			base.OnAppearing(); 
		}
	}
}
