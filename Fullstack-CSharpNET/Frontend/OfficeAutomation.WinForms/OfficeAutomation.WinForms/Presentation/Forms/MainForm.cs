using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Forms;
using OfficeAutomation.WinForms.Presentation.Controls;
using OfficeAutomation.WinForms.Presentation.Helpers;
using OfficeAutomation.WinForms.Presentation.Views;
using OfficeAutomation.Client.Implementations;
using OfficeAutomation.Client.Services;

namespace OfficeAutomation.WinForms.Presentation.Forms
{
    public partial class MainForm : Form
    {
        public static Label StatusLabel { get; private set; }
        private List<Button> menuButtons;

        // اضافه کردن HttpClient و Client‌ها
        private readonly HttpClient _httpClient;
        private readonly CartableClient _cartableClient;

        public MainForm()
        {
            InitializeComponent();

            // HttpClient و CartableClient
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7169/") };
            _cartableClient = new CartableClient(_httpClient);

            StatusLabel = lblStatus;
            menuButtons = new List<Button> { btnHome, btnCartable };

            // نمایش HomeView به صورت پیش‌فرض
            LoadContent(new HomeView());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MenuHelper.SetActiveMenuButton(menuButtons, btnHome);

            // نمایش نام کاربر در هدر
            if (SessionManager.Instance.IsAuthenticated)
            {
                lblUserName.Text = SessionManager.Instance.FullName ?? SessionManager.Instance.UserName;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            MenuHelper.SetActiveMenuButton(menuButtons, btnHome);
            LoadContent(new HomeView());
        }

        private void btnCartable_Click(object sender, EventArgs e)
        {
            MenuHelper.SetActiveMenuButton(menuButtons, btnCartable);

            // پاس دادن CartableClient به CartableView
            var cartableView = new CartableView(_cartableClient);
            LoadContent(cartableView);
        }

        private void LoadContent(BaseContentControl control)
        {
            if (contentPanel.Controls.Count > 0 && contentPanel.Controls[0] is BaseContentControl currentControl)
            {
                currentControl.ClearFeedback();
            }

            contentPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(control);
            control.OnLoadData();
        }
    }
}
