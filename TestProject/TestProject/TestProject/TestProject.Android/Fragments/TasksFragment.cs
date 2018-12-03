﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using TestProject.Core.ViewModels;
using TestProject.Droid.Views;
using Android.Support.V7.Widget;
using System.ComponentModel;
using Android.Support.V7.Widget.Helper;
using TestProject.Droid.Adapter;

namespace TestProject.Droid.Fragments
{
    [MvxFragmentPresentation(
        typeof(MainViewModel),
        Resource.Id.content_frame,
        false)]
    [Register("testProject.droid.fragments.TasksFragment")]
    public class TasksFragment
        : BaseFragment<TaskListViewModel>
    {
        protected override int FragmentId => Resource.Layout.TasksFragmentLayout;

        private MvxListView _listView;
     
        Android.Support.V7.Widget.Toolbar _toolbar;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewModel.ListOfTasks.CollectionChanged += ViewModel_CollectionChanged;

            var view = base.OnCreateView(inflater, container, savedInstanceState);
            var itemTouchHelper = new ItemTouchHelper(new SwipeItemDelete());

            _listView = view.FindViewById<MvxListView>(Resource.Id.task_recycler_view);
            _toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

            Activity.FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer_layout).CloseDrawers();
            var adapter = new ImageAdapter(this.Activity, (MvxAndroidBindingContext)BindingContext, _listView);

            _listView.Adapter = adapter;

            ViewModel.ShowMenuCommand.Execute(null);

            return view;
        }

        private void ViewModel_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ImageAdapter adapter = new ImageAdapter(this.Activity, (MvxAndroidBindingContext)BindingContext, _listView);
                _listView.Adapter = adapter;
            }
        }

        public override void OnDestroyView()
        {
            InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            var currentFocus = Activity.CurrentFocus;
            inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, 0);
            base.OnDestroyView();
        }

    }
}