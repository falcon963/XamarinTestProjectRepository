﻿using MvvmCross;
using MvvmCross.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Helpers;
using TestProject.Core.Helpers.Interfaces;
using TestProject.Core.Models;
using TestProject.Core.Services.Interfaces.SocialService.Google;

namespace TestProject.Core.Services
{
    public class GoogleService
        : IGoogleService
    {
        private readonly IHttpHelper _httpHelper;

        private readonly IMvxLog _mvxLog;

        public GoogleService(IHttpHelper httpHelper, IMvxLog mvxLog)
        {
            _httpHelper = httpHelper;
            _mvxLog = mvxLog;
        }

        public async Task<User> GetUserAsync(string accessToken)
        {
            User account = new User();

            try
            {
                var requestUrl = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accessToken;
                var model = await _httpHelper.Get<GoogleProfileModel>(requestUrl);
                var pictureUri = model?.PictureUri;

                if (pictureUri != null)
                {
                    var image = await _httpHelper.GetByte(pictureUri);
                    account.ImagePath = Convert.ToBase64String(image);
                }

                account.Login = model?.Name;
                account.GoogleId = model?.Id;
            }
            catch (Exception ex)
            {
                _mvxLog.Trace(ex.Message);
            }

            return account;
        }
    }
}
