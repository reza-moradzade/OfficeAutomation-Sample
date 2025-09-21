using System.Drawing;
using System.Windows.Forms;

namespace OfficeAutomation.WinForms.Presentation.Helpers
{
    public static class MessageHelper
    {
        // Show an error message on a Label control
        public static void ShowError(Label lbl, string message)
        {
            if (lbl == null) return;
            lbl.ForeColor = Color.Red;
            lbl.Text = message;
            lbl.Visible = true;
        }

        // Show a success message on a Label control
        public static void ShowSuccess(Label lbl, string message)
        {
            if (lbl == null) return;
            lbl.ForeColor = Color.Green;
            lbl.Text = message;
            lbl.Visible = true;
        }

        // Hide the message and clear the Label
        public static void Hide(Label lbl)
        {
            if (lbl == null) return;
            lbl.Visible = false;
            lbl.Text = string.Empty;
        }
    }
}
