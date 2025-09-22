using System;
using System.Windows.Forms;
using OfficeAutomation.WinForms.Presentation.Forms;
using OfficeAutomation.Login; // Namespace فرم WPF Login
using System.Windows;
using Application = System.Windows.Forms.Application;

namespace OfficeAutomation.WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Enable WinForms visual styles
            ApplicationConfiguration.Initialize();

            // اجرای WPF Login
            var loginWindow = new MainWindow(); // WPF Login
            loginWindow.ShowDialog(); // بصورت modal

            // اگر Login موفق بود (می‌توان یک property در loginWindow تعریف کرد)
            if (loginWindow.IsAuthenticated)
            {
                Application.Run(new MainForm());
            }
        }
    }
}
