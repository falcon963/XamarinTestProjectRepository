﻿using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Interfaces;
using TestProject.Core.Models;
using MvvmCross;
using Acr.UserDialogs;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using TestProject.Core.Constant;

namespace TestProject.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private readonly ILoginService _loginService;

        private readonly ITaskService _taskService;

        private Boolean _rememberMe;

        private Boolean _checkBoxStatus;

        private String _login;

        private String _password;

        User _user;

        public LoginViewModel(IMvxNavigationService navigationService, ILoginService loginService, ITaskService taskService)
        {
            _loginService = loginService;
            _navigationService = navigationService;
            _taskService = taskService;
            _user = new User();
            if (CrossSecureStorage.Current.GetValue(SecureConstant.status) == "True")
            {
                Login = CrossSecureStorage.Current.GetValue(SecureConstant.login);
                Password = CrossSecureStorage.Current.GetValue(SecureConstant.password);
                _rememberMe = Boolean.Parse(CrossSecureStorage.Current.GetValue(SecureConstant.status));
            }
        }

        public String Login
        {
            get
            {
                return _login;
            }
            set
            {
                SetProperty(ref _login, value);
                User.Login = _login;
                if (String.IsNullOrEmpty(User.Login) && String.IsNullOrEmpty(User.Password))
                {
                    EnableStatus = false;
                }
                if (!String.IsNullOrEmpty(User.Login) && !String.IsNullOrEmpty(User.Password))
                {
                    EnableStatus = true;
                }
            }
        }

        public String Password
        {
            get
            {
                return _password;
            }
            set
            {
                SetProperty(ref _password, value);
                User.Password = _password;
                if (String.IsNullOrEmpty(User.Login) && String.IsNullOrEmpty(User.Password))
                {
                    EnableStatus = false;
                }
                if (!String.IsNullOrEmpty(User.Login) && !String.IsNullOrEmpty(User.Password))
                {
                    EnableStatus = true;
                }
            }
        }

        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        public Boolean RememberMeStatus
        {
            get
            {
                return _rememberMe;
            }
            set
            {
                _rememberMe = value;
                if (_rememberMe == true)
                {
                    CrossSecureStorage.Current.SetValue(SecureConstant.status, "True");
                    _loginService.SetLoginAndPassword(Login, Password);
                }
                if (_rememberMe == false)
                {
                    CrossSecureStorage.Current.DeleteKey(SecureConstant.status);
                }
            }
        }

        public Boolean EnableStatus
        {
            get
            {
                return _checkBoxStatus;
            }
            set
            {
                SetProperty(ref _checkBoxStatus, value);
            }
        }

        public Task<Boolean> CheckLogin
        {
            get
            {
                return _taskService.CheckValidLogin(User.Login);
            }
        }

        #region Commands

        public IMvxAsyncCommand LoginCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    if (await _taskService.CheckAccountAccess(User.Login, User.Password) != null)
                    {
                        User = await _taskService.CheckAccountAccess(User.Login, User.Password);
                        var taskToNavigate = new ResultModel {Changes = new UserTask { UserId = User.Id } };
                        await _navigationService.Navigate<TaskListViewModel, ResultModel>(taskToNavigate);
                        await _navigationService.Close(this);
                    }
                    if((await _taskService.CheckAccountAccess(User.Login, User.Password) == null))
                    {
                        var alert = UserDialogs.Instance.Alert(new AlertConfig { Message = "Wrong data, account not found!", OkText = "Ok", Title = "Account not found" });
                        return;
                    }
                });
            }
        }

        public IMvxAsyncCommand RegistrationCommand
        {
            get
            {
                return new MvxAsyncCommand(async () => 
                {
                    if (!(await _taskService.CheckValidLogin(User.Login)))
                    {
                        var alert = UserDialogs.Instance.Alert(new AlertConfig { Message = "Login already use!", OkText = "Ok", Title = "Login use" });
                        return;
                    }
                    if ((await _taskService.CheckValidLogin(User.Login)))
                    {
                        await _taskService.CreateUser(User);
                        var alert = UserDialogs.Instance.Alert(new AlertConfig { Message = "Registrate successful!", OkText = "Ok", Title = "Success!" });
                    }
                });
            }
        }

        #endregion

    }
}