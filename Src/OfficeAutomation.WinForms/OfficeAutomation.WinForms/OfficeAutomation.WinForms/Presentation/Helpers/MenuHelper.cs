using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OfficeAutomation.WinForms.Presentation.Helpers
{
    public static class MenuHelper
    {
        // Default background color for inactive menu buttons
        private static readonly Color DefaultBackColor = Color.FromArgb(41, 128, 185);

        // Background color for the active (selected) menu button
        private static readonly Color ActiveBackColor = Color.FromArgb(41, 180, 220);

        // Sets the active state for the clicked menu button
        // Resets other buttons to default state
        public static void SetActiveMenuButton(List<Button> menuButtons, Button activeButton)
        {
            // Reset all buttons to default style
            foreach (var btn in menuButtons)
            {
                btn.BackColor = DefaultBackColor;
                btn.Font = new Font(btn.Font, FontStyle.Bold);
            }

            // Highlight the active button
            activeButton.BackColor = ActiveBackColor;
            activeButton.Font = new Font(activeButton.Font, FontStyle.Bold);
        }
    }
}
