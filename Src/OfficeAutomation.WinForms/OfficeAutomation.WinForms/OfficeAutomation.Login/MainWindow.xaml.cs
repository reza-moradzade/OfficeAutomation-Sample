using OfficeAutomation.Client.Exceptions;
using OfficeAutomation.Client.Implementations;
using OfficeAutomation.Client.Interfaces;
using OfficeAutomation.Client.Models;
using OfficeAutomation.Client.Services;
using System;
using System.Net.Http;
using System.Windows;

namespace OfficeAutomation.Login
{
    public partial class MainWindow : Window
    {
        private readonly IAuthClient _authClient;

        // Indicates whether login was successful
        public bool IsAuthenticated { get; private set; } = false;

        public MainWindow()
        {
            InitializeComponent();

            // Test credentials (for demo purposes)
            txtUsername.Text = "reza";
            txtPassword.Password = "123456";

            // Initialize HttpClient and AuthClient
            var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7169/") };
            _authClient = new AuthClient(httpClient);
        }

        // Handles login button click
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

                // Call API to authenticate user
                var authResult = await _authClient.LoginAsync(
                    loginDto,
                    captchaToken: "testCaptcha",
                    clientType: "WPF",
                    ipAddress: "127.0.0.1",
                    deviceInfo: Environment.MachineName
                );

                // Check login status and token
                if (string.IsNullOrWhiteSpace(authResult.Token) || authResult.IsLocked)
                {
                    string msg = authResult.IsLocked
                        ? authResult.Message ?? "Account is locked."
                        : "Invalid username or password.";

                    lblError.Content = msg;
                    return; // Do not close the window
                }

                // Save session info
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
