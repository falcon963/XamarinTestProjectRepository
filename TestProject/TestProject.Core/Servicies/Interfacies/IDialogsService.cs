﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Core.Servicies.Interfacies
{
    public interface IDialogsService
    {
        void ShowAlert(string message);
        Task<bool> ShowConfirmDialogAsync(string message, string title);
    }
}
