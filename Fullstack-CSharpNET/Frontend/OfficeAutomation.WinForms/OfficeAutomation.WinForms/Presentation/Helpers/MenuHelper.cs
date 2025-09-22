using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OfficeAutomation.WinForms.Presentation.Helpers
{
    public static class MenuHelper
    {
        private static readonly Color DefaultBackColor = Color.FromArgb(41, 128, 185); // دکمه‌های معمولی
        private static readonly Color ActiveBackColor = Color.FromArgb(41, 180, 220); // دکمه فعال

        public static void SetActiveMenuButton(List<Button> menuButtons, Button activeButton)
        {
            foreach (var btn in menuButtons)
            {
                btn.BackColor = DefaultBackColor;
                btn.Font = new Font(btn.Font, FontStyle.Bold);
            }

            activeButton.BackColor = ActiveBackColor;
            activeButton.Font = new Font(activeButton.Font, FontStyle.Bold);
        }
    }
}
