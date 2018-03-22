using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using TODODemo.Data.Managers;
using TODODemo.Data.Models;
using Xamarin.Forms;

namespace TODODemo.ViewModel
{
    public class AddTaskViewModel : BaseViewModel
    {
        private TodoItemManager _manager;

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");

                SaveTaskCommand.ChangeCanExecute();
                AddImageCommand.ChangeCanExecute();
            }
        }

        private string _content;
        public string Content 
        { 
            get => _content; 
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        public Command SaveTaskCommand { get; set; }

        public Command AddImageCommand { get; set; }

        public AddTaskViewModel()
        {
            ImagePath = "ImgProfile.png";

            SaveTaskCommand = new Command(async () => await SaveTask(), () => !IsBusy);

            AddImageCommand = new Command(async () => await AddImageTask(), () => !IsBusy);
        }

        private async Task AddImageTask()
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("Select an option", "Cancel", null, "Take Photo", "Gallery");

            MediaFile file = null;

            await CrossMedia.Current.Initialize();

            if (action == "Take Photo")
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }


                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    await Application.Current.MainPage.DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");

                    return;
                }

                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Test",
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    DefaultCamera = CameraDevice.Rear
                });

                if (file == null)
                    return;
                 
            }
            else if(action =="Gallery")
            {
                if(!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }

                file = await CrossMedia.Current.PickPhotoAsync();

                if (file == null)
                    return;
            }

            if (file != null)
                ImagePath = file.Path;
        }

        private async Task SaveTask()
        {
            if (IsBusy)
                return;

            Exception error = null;

            try
            {
                IsBusy = true;

                _manager = new TodoItemManager();

                TodoItem item = new TodoItem();
                item.Content = Content;
                item.Status = StatusType.Pending;
                item.Image = ImagePath;
                item.LastModified = DateTime.Now;

                var result = await _manager.SaveOrUpdateInDBAsync(item);

                if (result)
                    await Application.Current.MainPage.Navigation.PopAsync();
                else
                    throw new Exception("An error ocurred while saving the data");
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error", error.Message, "OK");
        }
    }
}
