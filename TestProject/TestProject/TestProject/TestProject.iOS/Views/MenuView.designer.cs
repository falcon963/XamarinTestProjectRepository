﻿// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TestProject.iOS.Views
{
    [Register ("MenuView")]
    partial class MenuView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BurgerMenu { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BurgerMenu != null) {
                BurgerMenu.Dispose ();
                BurgerMenu = null;
            }
        }
    }
}