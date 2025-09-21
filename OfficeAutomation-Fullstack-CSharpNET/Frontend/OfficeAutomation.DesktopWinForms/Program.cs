using System;
using System.Windows.Forms;
using OfficeAutomation.WinForms.Presentation.Forms;
using OfficeAutomation.Login; // Namespace of the WPF Login window
using System.Windows;
using Application = System.Windows.Forms.Application;

namespace OfficeAutomation.WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Initialize application configuration (high DPI, default font, etc.)
            ApplicationConfiguration.Initialize();

            // Launch the WPF Login window (modal)
            var loginWindow = new MainWindow();
            loginWindow.ShowDialog();

            // Run the WinForms MainForm only if the user is authenticated.
            // The "IsAuthenticated" property should be exposed by the WPF Login window.
            if (loginWindow.IsAuthenticated)
            {
                Application.Run(new MainForm());
            }
            else
            {
                // Optional: exit gracefully if authentication fails
                Environment.Exit(0);
            }
        }
    }
}
