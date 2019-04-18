﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.IO;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using TestProject.Core.Interfaces;
using Android.Support.V7.Widget;
using Android;
using TestProject.Droid.Views;
using Android.Support.V4.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Uri = Android.Net.Uri;
using Path = System.IO.Path;
using File = Java.IO.File;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame,
        true)]
    [Register("testproject.droid.fragments.TaskFragment")]
    public class TaskFragment 
        : BaseFragment<TaskViewModel>
    {
       
        private LinearLayout _linearLayout;
        private Toolbar _toolbar;
        private ImageView _imageView;
        private static readonly Int32 REQUEST_CAMERA = 0;
        private static readonly Int32 SELECT_FILE = 1;
        private Bitmap _bitmap;

        protected override int FragmentId => Resource.Layout.TaskFragment;

        public Uri ImageUri { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _linearLayout = view.FindViewById<LinearLayout>(Resource.Id.task_linearlayout);
            _toolbar = view.FindViewById<Toolbar>(Resource.Id.fragment_toolbar);
            _imageView = view.FindViewById<ImageView>(Resource.Id.image_view);

            _imageView.Click += OnAddPhotoClicked;
            _linearLayout.Click += delegate { HideSoftKeyboard(); };
            _toolbar.Click += delegate { HideSoftKeyboard(); };

            if (ViewModel.UserTask.Changes.ImagePath == null)
            {
                try
                {
                    _bitmap = BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.placeholder);
                    _imageView.SetImageBitmap(_bitmap);
                }
                catch (Java.Lang.OutOfMemoryError)
                {
                    GC.Collect();
                }
            }

            if (ViewModel.UserTask.Changes.ImagePath != null)
            {
                BitmapFactory.Options options = new BitmapFactory.Options();
                options.InSampleSize = 3;
                try
                {
                    _bitmap = BitmapFactory.DecodeFile(ViewModel.UserTask.Changes.ImagePath, options);
                    _imageView.SetImageBitmap(_bitmap);
                }
                catch (Java.Lang.OutOfMemoryError)
                {
                    GC.Collect();
                }
            }

            ((MainActivity)ParentActivity).DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);



            if (resultCode == -1 && requestCode == 0)
            {
                Bitmap bitmap = BitmapFactory.DecodeFile(ImageUri.Path);

                using (MemoryStream writer = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 40, writer);
                    Java.IO.File resizeFile = new Java.IO.File(ImageUri.Path);
                    resizeFile.CreateNewFile();
                    FileOutputStream fos = new FileOutputStream(resizeFile);
                    fos.Write(writer.ToArray());
                    fos.Close();
                    fos.Dispose();
                    BitmapFactory.Options options = new BitmapFactory.Options();
                    options.InSampleSize = 3;
                    Bitmap _bitmap = BitmapFactory.DecodeFile(ImageUri.Path, options);
                    _imageView.SetImageBitmap(_bitmap);
                    ViewModel.UserTask.Changes.ImagePath = ImageUri.Path;
                }
            }
            if (resultCode == -1 
                && requestCode == 1)
            {
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                string name = "Test_Project_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                var filePath = Path.Combine(sdCardPath, name);
                var stream = new FileStream(filePath, FileMode.Create);

                Bitmap bitmap = BitmapFactory.DecodeFile(GetRealPathFromURI(data.Data));
                try
                {
                    using (MemoryStream writer = new MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Jpeg, 40, writer);
                        File resizeUri = GetPhotoFileUri("Test_Project_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_resize" + ".jpg");
                        File resizeFile = new File(resizeUri.Path);
                        resizeFile.CreateNewFile();
                        FileOutputStream fos = new FileOutputStream(resizeFile);
                        fos.Write(writer.ToArray());
                        fos.Close();
                        fos.Dispose();
                        BitmapFactory.Options options = new BitmapFactory.Options();
                        options.InSampleSize = 3;
                        Bitmap _bitmap = BitmapFactory.DecodeFile(resizeUri.Path, options);
                        _imageView.SetImageBitmap(_bitmap);
                        ViewModel.UserTask.Changes.ImagePath = resizeUri.Path;
                    }
                }
                catch (Java.Lang.OutOfMemoryError) {
                    GC.Collect();
                }

            }
            if (resultCode == 0)
            {

            }
        }

        private Uri GetImageUri(Context context, Bitmap inImage)
        {
            String path = MediaStore.Images.Media.InsertImage(context.ContentResolver, inImage, "Title", null);
            return Uri.Parse(path);
        }


        public File GetPhotoFileUri(String fileName)
        {
            File mediaStorageDir = new File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);

            File file = new File(mediaStorageDir.Path + File.Separator + fileName);

            return file;
        }

        public String GetRealPathFromURI(Uri contentUri)
        {
            String[] proj = { MediaStore.Images.Media.InterfaceConsts.Data };
            var cursor = this.Activity.ContentResolver.Query(contentUri, proj, null, null, null);
            int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);

            cursor.MoveToFirst();

            return cursor.GetString(column_index);
        }

        public void OnAddPhotoClicked(object sender, EventArgs e)
        {
            var popup = new Android.Support.V7.Widget.PopupMenu(Activity, _imageView);
            popup.Menu.Add("Camera");
            popup.Menu.Add("Gallery");
            popup.Menu.Add("Cancel");
            popup.MenuItemClick += OnMenuItemClicked;
            popup.Show();
        }

        private void OnMenuItemClicked(object sender, Android.Support.V7.Widget.PopupMenu.MenuItemClickEventArgs e)
        {
            var label = e.Item.TitleFormatted.ToString();

            if(label == "Camera")
            {
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                string name = "Test_Project_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                var filePath = Path.Combine(sdCardPath, name);
                File image = new File(filePath);
                ImageUri = Uri.FromFile(image);
                var intent = new Intent(MediaStore.ActionImageCapture);
                intent.PutExtra(MediaStore.ExtraOutput, ImageUri);
                this.StartActivityForResult(intent, REQUEST_CAMERA);
            }
            if(label == "Gallery")
            {
                var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                intent.SetType("image/*");
                this.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), SELECT_FILE);
            }
            if(label == "Cancel")
            {
                
            }
        }

        public void HideSoftKeyboard()
        {
            InputMethodManager close = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            close.HideSoftInputFromWindow(_linearLayout.WindowToken, 0);
        }

        private void UnbindDrawables(View view)
        {
            if (view.Background != null)
            {
                view.Background.SetCallback(null);
            }
            if (view is ViewGroup)
            {
                for (int i = 0; i < ((ViewGroup)view).ChildCount; i++)
                {
                    UnbindDrawables(((ViewGroup)view).GetChildAt(i));
                }

                ((ViewGroup)view).RemoveAllViews();
            }
        }

        public void ShowInputMethod()
        {
            InputMethodManager methodManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            methodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        public override void OnDestroyView()
        {
            InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            var currentFocus = Activity.CurrentFocus;
            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, 0);
            base.OnDestroyView();
            UnbindDrawables(this.View);
            GC.SuppressFinalize(this);
        }
    }
}