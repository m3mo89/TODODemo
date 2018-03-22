using TODODemo.ViewModel;
using Xamarin.Forms;

namespace TODODemo
{
    public partial class TODODemoPage : TabbedPage
    {
        public TODODemoPage()
        {
            InitializeComponent();

            BindingContext = new TodoDemoViewModel();
        }
    }
}
