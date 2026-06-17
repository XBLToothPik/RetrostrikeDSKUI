using ReaLTaiizor.Controls;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
#pragma warning disable CS8601 // Possible null reference assignment.

namespace RetrostrikeDSKUI.Core
{
    public static class PoisonControlExtensions
    {
        private static MethodInfo showMethod = typeof(PoisonDropDownButton)
                                                .GetMethod("ShowContextMenuStrip", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void OpenDropDown(this PoisonDropDownButton button)
        {
            showMethod?.Invoke(button, null);
        }
    }
}
