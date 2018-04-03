using System.ComponentModel;
using CoreGraphics;
using Foundation;
using IOS.Renderer;
using TODODemo.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchPage), typeof(SearchPageRenderer))]

namespace IOS.Renderer
{
    public class SearchPageRenderer : TabbedRenderer
    {
        
        private UIButton _searchButton;
        private UIButton _addButton;
        private UIButton _cancelButton;
        private UIBarButtonItem _searchbarButtonItem;
        private UIView _defaultTitleView;
        private UITextField _searchTextField;
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
            base.OnElementChanged(e);

            if (e.NewElement != e.OldElement)
            {
                if (e.OldElement != null)
                {
                    e.OldElement.PropertyChanged -= OnElementPropertyChanged;
                }
                if (e.NewElement != null)
                {
                    e.NewElement.PropertyChanged += OnElementPropertyChanged;
                }
            }
		}

        void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        { 
            if (e.PropertyName == nameof(TabbedPage.CurrentPage))
            {
                InvokeOnMainThread(()=>{
                    ShowSearchButton();
                });

            }
        }

		public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _defaultTitleView = NavigationItem?.TitleView;
            CreateSearchButton();
            CreateAddButton();
            CreateSearchToolbar();
            CreateCancelButton();

            UIBarButtonItem[] buttons = new UIBarButtonItem[]
            {
                new UIBarButtonItem(_searchButton),
                new UIBarButtonItem(_addButton)
            };

            DisplayLeftBarButton(buttons);
        }

        private void CreateSearchButton()
        {
            if (NavigationController?.NavigationBar == null)
            {
                return;
            }
            var height = NavigationController.NavigationBar.Frame.Height;
            var searchButtonView = new UIView(new CGRect(0, 0, 50, height));
            _searchButton = new UIButton(UIButtonType.System)
            {
                BackgroundColor = UIColor.Clear,
                Frame = searchButtonView.Frame,
                AutosizesSubviews = true,
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleMargins
            };
            _searchButton.SetTitle("\uD83D\uDD0D", UIControlState.Normal);
            _searchButton.TouchUpInside -= SearchButton_TouchUpInside;
            _searchButton.TouchUpInside += SearchButton_TouchUpInside;
        }

        private void CreateAddButton()
        {
            if (NavigationController?.NavigationBar == null)
            {
                return;
            }
            var height = NavigationController.NavigationBar.Frame.Height;
            var searchButtonView = new UIView(new CGRect(0, 0, 50, height));
            _addButton = new UIButton(UIButtonType.System)
            {
                BackgroundColor = UIColor.Clear,
                Frame = searchButtonView.Frame,
                AutosizesSubviews = true,
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleMargins
            };
            _addButton.SetTitle("\u002b", UIControlState.Normal);
            _addButton.TouchUpInside -= AddButton_TouchUpInside;
            _addButton.TouchUpInside += AddButton_TouchUpInside;
        }

        private void CreateSearchToolbar()
        {
            var element = Element as SearchPage;

            if (element == null || NavigationController?.NavigationBar == null)
            {
                return;
            }

            var width = NavigationController.NavigationBar.Frame.Width;
            var height = NavigationController.NavigationBar.Frame.Height;
            var searchBar = new UIStackView(new CGRect(0, 0, width * 0.75, height))
            {
                Alignment = UIStackViewAlignment.Center,
                Axis = UILayoutConstraintAxis.Horizontal,
                Spacing = 3
            };

            _searchTextField = new UITextField
            {
                BackgroundColor = UIColor.White,
                AttributedPlaceholder = new NSAttributedString(element.SearchPlaceHolderText, foregroundColor: UIColor.Gray),
                Placeholder = element.SearchPlaceHolderText,
            };
            _searchTextField.SizeToFit();

            // Delete button
            var textDeleteButton = new UIButton(new CGRect(0, 0, _searchTextField.Frame.Size.Height + 5, _searchTextField.Frame.Height)) { BackgroundColor = UIColor.Clear };
            textDeleteButton.SetTitleColor(UIColor.FromRGB(146, 146, 146), UIControlState.Normal);
            textDeleteButton.SetTitle("\u24CD", UIControlState.Normal);

            textDeleteButton.TouchUpInside += (sender, e) =>
            {
                _searchTextField.Text = string.Empty;
                _searchTextField.ResignFirstResponder();
                element.SetValue(SearchPage.SearchTextProperty, _searchTextField.Text);
            };

            _searchTextField.RightView = textDeleteButton;
            _searchTextField.RightViewMode = UITextFieldViewMode.Always;

            // Border
            _searchTextField.BorderStyle = UITextBorderStyle.RoundedRect;
            _searchTextField.Layer.BorderColor = UIColor.FromRGB(239, 239, 239).CGColor;
            _searchTextField.Layer.BorderWidth = 1;
            _searchTextField.Layer.CornerRadius = 5;
            _searchTextField.EditingChanged += (sender, e) => element.SetValue(SearchPage.SearchTextProperty, _searchTextField.Text);
            _searchTextField.KeyboardType = UIKeyboardType.Default;
            _searchTextField.EditingDidEndOnExit += (sender, e) => element.SearchCommand?.Execute(_searchTextField.Text);
            searchBar.AddArrangedSubview(_searchTextField);

            _searchbarButtonItem = new UIBarButtonItem(searchBar);
        }

        private void CreateCancelButton()
        {
            var element = Element as SearchPage;
            if (element == null)
            {
                return;
            }
            if (NavigationController?.NavigationBar != null)
            {
                var searchButtonView = new UIView(new CGRect(0, 0, 60, NavigationController.NavigationBar.Frame.Height));
                _cancelButton = new UIButton(UIButtonType.System)
                {
                    BackgroundColor = UIColor.Clear,
                    Frame = searchButtonView.Frame,
                    AutosizesSubviews = true,
                    AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleMargins,
                    HorizontalAlignment = UIControlContentHorizontalAlignment.Right
                };
            }
            _cancelButton.SetTitle("Cancel", UIControlState.Normal);
            _cancelButton.TouchUpInside -= CancelButton_TouchUpInside;
            _cancelButton.TouchUpInside += CancelButton_TouchUpInside;
        }

        private void DisplayRightBarButton(UIView button)
        {
            if (ParentViewController?.NavigationItem != null)
            {
                ParentViewController.NavigationItem.RightBarButtonItem = new UIBarButtonItem(button);
            }
        }

        private void DisplayLeftBarButton(UIBarButtonItem[] buttons)
        {
            if (ParentViewController?.NavigationItem != null)
            {
                ParentViewController.NavigationItem.LeftBarButtonItems = buttons;
            }
        }

        private void ClearRightToolbarButton()
        {
            if (ParentViewController?.NavigationItem != null)
            {
                ParentViewController.NavigationItem.RightBarButtonItem = null;
            }
        }


        private void ClearLeftToolbarButtons()
        {
            if (ParentViewController?.NavigationItem != null)
            {
                ParentViewController.NavigationItem.LeftBarButtonItems = null;
            }
        }

        private void SearchButton_TouchUpInside(object sender, System.EventArgs e) => ShowSearchToolbar();

        private void AddButton_TouchUpInside(object sender, System.EventArgs e)
        {
            var element = Element as SearchPage;
            if (element == null)
            {
                return;
            }
            element.AddCommand?.Execute(null);
        }

        private void CancelButton_TouchUpInside(object sender, System.EventArgs e)
        {
            var element = Element as SearchPage;

            _searchTextField.Text = string.Empty;
            _searchTextField.ResignFirstResponder();
            element.SearchCommand?.Execute(_searchTextField.Text);

            ClearRightToolbarButton();
            HideSearchToolbar();

            UIBarButtonItem[] buttons = new UIBarButtonItem[]
            {
                new UIBarButtonItem(_searchButton),
                new UIBarButtonItem(_addButton)
            };
            DisplayLeftBarButton(buttons);
            //DisplayLeftBarButton(_addButton);
        }

        private void ShowSearchButton()
        {
            ClearLeftToolbarButtons();
            HideSearchToolbar();
            CreateSearchButton();

            UIBarButtonItem[] buttons = new UIBarButtonItem[] 
            { 
                new UIBarButtonItem(_searchButton)
            };

            NavigationItem.SetLeftBarButtonItems(buttons, true);

            NavigationItem.TitleView = new UIView();

            if (ParentViewController?.NavigationItem == null)
            {
                return;
            }


            ParentViewController.NavigationItem.LeftBarButtonItems = this.NavigationItem.LeftBarButtonItems;
           
            ParentViewController.NavigationItem.TitleView = this.NavigationItem.TitleView;
        }

        private void ShowSearchToolbar()
        {
            ClearRightToolbarButton();
            DisplayRightBarButton(_cancelButton);

            NavigationItem.SetLeftBarButtonItem(_searchbarButtonItem, true);

            NavigationItem.TitleView = new UIView();

            if (ParentViewController?.NavigationItem == null)
            {
                return;
            }
            ParentViewController.NavigationItem.LeftBarButtonItems = NavigationItem.LeftBarButtonItems;
            ParentViewController.NavigationItem.TitleView = NavigationItem.TitleView;
        }

        private void HideSearchToolbar()
        {
            NavigationItem.TitleView = _defaultTitleView;
            if (ParentViewController?.NavigationItem == null)
            {
                return;
            }
            ParentViewController.NavigationItem.LeftBarButtonItems = null;
            ParentViewController.NavigationItem.TitleView = NavigationItem.TitleView;
        }
    }
}