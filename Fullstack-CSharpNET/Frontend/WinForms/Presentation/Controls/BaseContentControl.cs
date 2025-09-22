using OfficeAutomation.WinForms.Presentation.Forms;
using OfficeAutomation.WinForms.Presentation.Helpers;
using System.Windows.Forms;

namespace OfficeAutomation.WinForms.Presentation.Controls
{
    public partial class BaseContentControl : UserControl
    {
        public BaseContentControl()
        {
            // Fill the parent panel by default
            this.Dock = DockStyle.Fill;
        }

        // Show an error message on the footer status label
        public void ShowError(string message)
        {
            if (MainForm.StatusLabel != null)
                MessageHelper.ShowError(MainForm.StatusLabel, message);
        }

        // Show a success message on the footer status label
        public void ShowSuccess(string message)
        {
            if (MainForm.StatusLabel != null)
                MessageHelper.ShowSuccess(MainForm.StatusLabel, message);
        }

        // Hide any feedback messages on the footer
        public void ClearFeedback()
        {
            if (MainForm.StatusLabel != null)
                MessageHelper.Hide(MainForm.StatusLabel);
        }

        // Method for loading data or initializing the control
        // Should be overridden in derived controls
        public virtual void OnLoadData()
        {
            // override in child controls
        }
    }
}
