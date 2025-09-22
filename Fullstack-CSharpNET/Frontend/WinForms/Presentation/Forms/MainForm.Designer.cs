namespace OfficeAutomation.WinForms.Presentation.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // UI Controls
        private Panel headerPanel;
        private Label lblSystemName;
        private Label lblUserName;
        private Panel footerPanel;
        private Label lblFooter;
        private Panel leftMenuPanel;
        private Button btnHome;
        private Button btnCartable;
        private Panel contentPanel;
        private Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            headerPanel = new Panel();
            lblSystemName = new Label();
            lblUserName = new Label();
            footerPanel = new Panel();
            lblStatus = new Label();
            lblFooter = new Label();
            leftMenuPanel = new Panel();
            btnCartable = new Button();
            btnHome = new Button();
            contentPanel = new Panel();
            headerPanel.SuspendLayout();
            footerPanel.SuspendLayout();
            leftMenuPanel.SuspendLayout();
            SuspendLayout();

            // Header panel (top bar with system name and user info)
            headerPanel.BackColor = Color.FromArgb(52, 152, 219);
            headerPanel.Controls.Add(lblSystemName);
            headerPanel.Controls.Add(lblUserName);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(900, 60);
            headerPanel.TabIndex = 3;

            // System name label
            lblSystemName.AutoSize = true;
            lblSystemName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblSystemName.ForeColor = Color.White;
            lblSystemName.Location = new Point(12, 14);
            lblSystemName.Name = "lblSystemName";
            lblSystemName.Size = new Size(176, 25);
            lblSystemName.TabIndex = 0;
            lblSystemName.Text = "Office Automation";

            // Username label (displays logged-in user)
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI", 10F);
            lblUserName.ForeColor = Color.White;
            lblUserName.Location = new Point(778, 20);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(110, 19);
            lblUserName.TabIndex = 0;
            lblUserName.Text = "Reza Moradzade";

            // Footer panel (bottom status bar)
            footerPanel.BackColor = Color.FromArgb(41, 128, 185);
            footerPanel.Controls.Add(lblStatus);
            footerPanel.Controls.Add(lblFooter);
            footerPanel.Dock = DockStyle.Bottom;
            footerPanel.Location = new Point(0, 520);
            footerPanel.Name = "footerPanel";
            footerPanel.Size = new Size(900, 30);
            footerPanel.TabIndex = 2;

            // Status label (dynamic messages will be shown here)
            lblStatus.AutoSize = true;
            lblStatus.BackColor = Color.White;
            lblStatus.Location = new Point(198, 6);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 1;

            // Footer static text
            lblFooter.Anchor = AnchorStyles.None;
            lblFooter.AutoSize = true;
            lblFooter.Font = new Font("Segoe UI", 9F);
            lblFooter.ForeColor = Color.White;
            lblFooter.Location = new Point(5, 5);
            lblFooter.Name = "lblFooter";
            lblFooter.Size = new Size(147, 15);
            lblFooter.TabIndex = 0;
            lblFooter.Text = "© 2025 Office Automation";

            // Left menu panel (sidebar with navigation buttons)
            leftMenuPanel.BackColor = Color.FromArgb(41, 128, 185);
            leftMenuPanel.Controls.Add(btnCartable);
            leftMenuPanel.Controls.Add(btnHome);
            leftMenuPanel.Dock = DockStyle.Left;
            leftMenuPanel.Location = new Point(0, 60);
            leftMenuPanel.Name = "leftMenuPanel";
            leftMenuPanel.Size = new Size(160, 460);
            leftMenuPanel.TabIndex = 1;

            // Cartable button
            btnCartable.BackColor = Color.FromArgb(41, 128, 185);
            btnCartable.Dock = DockStyle.Top;
            btnCartable.FlatAppearance.BorderSize = 0;
            btnCartable.FlatStyle = FlatStyle.Flat;
            btnCartable.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCartable.ForeColor = Color.White;
            btnCartable.Location = new Point(0, 40);
            btnCartable.Name = "btnCartable";
            btnCartable.Padding = new Padding(10, 0, 0, 0);
            btnCartable.Size = new Size(160, 40);
            btnCartable.TabIndex = 0;
            btnCartable.Text = "📋 Cartable";
            btnCartable.TextAlign = ContentAlignment.MiddleLeft;
            btnCartable.UseVisualStyleBackColor = false;
            btnCartable.Click += btnCartable_Click;

            // Home button
            btnHome.BackColor = Color.FromArgb(41, 128, 185);
            btnHome.Dock = DockStyle.Top;
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnHome.ForeColor = Color.White;
            btnHome.Location = new Point(0, 0);
            btnHome.Name = "btnHome";
            btnHome.Padding = new Padding(10, 0, 0, 0);
            btnHome.Size = new Size(160, 40);
            btnHome.TabIndex = 1;
            btnHome.Text = "🏠 Home";
            btnHome.TextAlign = ContentAlignment.MiddleLeft;
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnHome_Click;

            // Content panel (where dynamic views are loaded)
            contentPanel.BackColor = Color.White;
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(160, 60);
            contentPanel.Name = "contentPanel";
            contentPanel.Size = new Size(740, 460);
            contentPanel.TabIndex = 0;

            // MainForm setup
            ClientSize = new Size(900, 550);
            Controls.Add(contentPanel);
            Controls.Add(leftMenuPanel);
            Controls.Add(footerPanel);
            Controls.Add(headerPanel);
            Name = "MainForm";
            Load += MainForm_Load;
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            footerPanel.ResumeLayout(false);
            footerPanel.PerformLayout();
            leftMenuPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
