using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Plugin.CurrentActivity;
using TODODemo.Droid.Renderers;
using TODODemo.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using SearchView = Android.Support.V7.Widget.SearchView;

[assembly: ExportRenderer(typeof(SearchPage), typeof(SearchPageRenderer))]
namespace TODODemo.Droid.Renderers
{
    public class SearchPageRenderer : TabbedPageRenderer
    {
        private SearchView _searchView;
        //private Toolbar maintoolbar;
        public SearchPageRenderer(Context context)
            : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e?.NewElement == null || e.OldElement != null)
            {
                return;
            }
            AddSearchToToolBar();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(TabbedPage.CurrentPage))
            {

                AddSearchToToolBar();

            }

        }

        protected override void Dispose(bool disposing)
        {
            if (_searchView != null)
            {
                _searchView.QueryTextChange += searchView_QueryTextChange;
                _searchView.QueryTextSubmit += searchView_QueryTextSubmit;
            }
            var maintoolbar = (CrossCurrentActivity.Current?.Activity as MainActivity)?.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            maintoolbar.Menu?.RemoveItem(Resource.Menu.mainmenu);
            //MainActivity.ToolBar?.Menu?.RemoveItem(Resource.Menu.mainmenu);
            base.Dispose(disposing);
        }

        private void AddSearchToToolBar()
        {
            var search = Element as SearchPage;
            var searchTextTemp = string.Empty;
            if (search.SearchText != null)
            {
                searchTextTemp = search.SearchText;
            }

            var maintoolbar = (CrossCurrentActivity.Current?.Activity as MainActivity)?.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (maintoolbar == null || Element == null)
            {
                System.Diagnostics.Debug.WriteLine("SearchPageRenderer ToolBar or Element is null");
                return;
            }
            maintoolbar.Title = Element.Title;
            maintoolbar.InflateMenu(Resource.Menu.mainmenu);
            //if (MainActivity.ToolBar == null || Element == null)
            //{
            //    System.Diagnostics.Debug.WriteLine("SearchPageRenderer ToolBar or Element is null");
            //    return;
            //}

            //MainActivity.ToolBar.Title = Element.Title;
            //MainActivity.ToolBar.InflateMenu(Resource.Menu.mainmenu);

            var actionSearch = Resource.Id.action_search;
            System.Diagnostics.Debug.WriteLine("SearchPageRenderer AddSearchToToolBar actionSearch " + actionSearch);
            _searchView = maintoolbar.Menu?.FindItem(Resource.Id.action_search)?.ActionView?.JavaCast<SearchView>();

            var test = maintoolbar.Menu.FindItem(Resource.Id.addTodo).SetIcon(Resource.Drawable.ic_playlist_add_white); // 'heart' image border only

            maintoolbar.MenuItemClick += OnMenuItemClick;
           
            // _searchView = MainActivity.ToolBar.Menu?.FindItem(Resource.Id.action_search)?.ActionView?.JavaCast<SearchView>();

            if (_searchView == null)
            {
                return;
            }

            //default open but has a debug make searchview hasnot cursor
            // _searchView.OnActionViewExpanded();
            //_searchView.SetBackgroundColor(Android.Graphics.Color.Green);
            //_searchView.SetIconifiedByDefault(false);

            _searchView.OnActionViewExpanded();
            _searchView.QueryTextChange += searchView_QueryTextChange;
            _searchView.QueryTextSubmit += searchView_QueryTextSubmit;

            //_searchView.QueryHint = (Element as SearchPage)?.SearchPlaceHolderText;
            //_searchView.ImeOptions = (int)Android.Views.InputMethods.ImeAction.Search;
            // donn't use this code it make the cursor miss
            //_searchView.InputType = (int)Android.Text.InputTypes.TextVariationNormal;
            //_searchView.MaxWidth = int.MaxValue;
            _searchView.SetQuery(searchTextTemp, false);


        }


        private void OnMenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {

            if (e == null)
            {
                return;
            }

            var searchPage = Element as SearchPage;
            if (searchPage == null)
            {
                return;
            }

            var item = e.Item;

            switch (item.ItemId)
            {
                case Resource.Id.addTodo:
                    searchPage.AddCommand?.Execute(null);
                    break;
            }

            e.Handled = true;
        }

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
            base.OnLayout(changed, left, top, right, bottom);

            //AddSearchToToolBar();
		}

		private void searchView_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            var searchPage = Element as SearchPage;
            if (searchPage == null)
            {
                return;
            }
            searchPage.SearchText = e.Query;
            searchPage.SearchCommand?.Execute(e.Query);
            e.Handled = true;
        }

        private void searchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            var searchPage = Element as SearchPage;
            if (searchPage == null)
            {
                return;
            }

            searchPage.SearchText = e?.NewText;

        }
    }
}
