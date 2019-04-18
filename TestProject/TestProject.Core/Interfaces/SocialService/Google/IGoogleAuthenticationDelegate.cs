﻿using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;

namespace TestProject.Core.Interfaces.SocialService.Google
{
    public interface IGoogleAuthenticationDelegate
    {
        void OnAuthenticationCompleted(string token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
    }
}
