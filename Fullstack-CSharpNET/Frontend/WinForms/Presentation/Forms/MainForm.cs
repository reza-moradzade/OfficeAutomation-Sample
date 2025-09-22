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
        // Global status label (used by other components to update status messages)
        public static Label StatusLabel { get; private set; }

        // Holds references to all menu buttons for active state management
        private readonly List<Button> _menuButtons;

        // API clients
        private readonly HttpClient _httpClient;
        private readonly CartableClient _cartableClient;

        public MainForm()
        {
            InitializeComponent();

            // Initialize HttpClient with base API address
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7169/") };
            _cartableClient = new CartableClient(_httpClient);

            StatusLabel = lblStatus;

            // Register menu buttons for active state handling
            _menuButtons = new List<Button> { btnHome, btnCartable };

            // Load the default view (Home)
            LoadContent(new HomeView());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Set default active menu
            MenuHelper.SetActiveMenuButton(_menuButtons, btnHome);

            // Display user info in header if authentication is successful
            if (SessionManager.Instance.IsAuthenticated)
            {
                lblUserName.Text = SessionManager.Instance.FullName
                                   ?? SessionManager.Instance.UserName;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            // Switch to Home view
            MenuHelper.SetActiveMenuButton(_menuButtons, btnHome);
            LoadContent(new HomeView());
        }

        private void btnCartable_Click(object sender, EventArgs e)
        {
            // Switch to Cartable view
            MenuHelper.SetActiveMenuButton(_menuButtons, btnCartable);

            // Pass CartableClient dependency to the CartableView
            var cartableView = new CartableView(_cartableClient);
            LoadContent(cartableView);
        }

        // Dynamically loads the given UserControl into the main content panel
        private void LoadContent(BaseContentControl control)
        {
            // Clear feedback from the currently loaded control (if any)
            if (contentPanel.Controls.Count > 0 &&
                contentPanel.Controls[0] is BaseContentControl currentControl)
            {
                currentControl.ClearFeedback();
            }

            // Replace current control with the new one
            contentPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(control);

            // Trigger data loading logic of the control
            control.OnLoadData();
        }
    }
}
