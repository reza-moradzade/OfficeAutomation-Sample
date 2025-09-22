using OfficeAutomation.Client.Exceptions;
using OfficeAutomation.Client.Implementations;
using OfficeAutomation.Client.Interfaces;
using OfficeAutomation.Client.Models;
using OfficeAutomation.Client.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace OfficeAutomation.Login
{
    public partial class MainWindow : Window
    {
        private readonly IAuthClient _authClient;

        public bool IsAuthenticated { get; private set; } = false;

        public MainWindow()
        {
            InitializeComponent();

            // تستی: نام کاربری و رمز پیش‌فرض
            txtUsername.Text = "reza";
            txtPassword.Password = "123456";

            // ساخت HttpClient و AuthClient
            var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7169/") };
            _authClient = new AuthClient(httpClient);
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            lblError.Content = "";
            progressLogin.Visibility = Visibility.Visible;
            btnLogin.IsEnabled = false;

            try
            {
                var loginDto = new LoginDto
                {
                    UserName = username,
                    Password = password
                };

                var authResult = await _authClient.LoginAsync(
                    loginDto,
                    captchaToken: "testCaptcha",
                    clientType: "WPF",
                    ipAddress: "127.0.0.1",
                    deviceInfo: Environment.MachineName
                );

                // بررسی وضعیت لاگین و توکن
                if (string.IsNullOrWhiteSpace(authResult.Token) || authResult.IsLocked)
                {
                    string msg = authResult.IsLocked
                        ? authResult.Message ?? "Account is locked."
                        : "Invalid username or password.";

                    lblError.Content = msg;
                    return; // فرم بسته نشود
                }

                // ذخیره اطلاعات نشست
                SessionManager.Instance.SetSession(authResult);

                IsAuthenticated = true;
                this.Close();
            }
            catch (ApiException ex)
            {
                lblError.Content = $"Login failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                lblError.Content = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                progressLogin.Visibility = Visibility.Collapsed;
                btnLogin.IsEnabled = true;
            }
        }
    }
}
