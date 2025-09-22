namespace OfficeAutomation.WinForms.Presentation.Helpers
{
    public static class MessageHelper
    {
        /// <summary>
        /// نمایش پیام خطا روی یک کنترل Label
        /// </summary>
        public static void ShowError(Label lbl, string message)
        {
            if (lbl == null) return;
            lbl.ForeColor = Color.Red;
            lbl.Text = message;
            lbl.Visible = true;
        }

        /// <summary>
        /// نمایش پیام موفقیت روی یک کنترل Label
        /// </summary>
        public static void ShowSuccess(Label lbl, string message)
        {
            if (lbl == null) return;
            lbl.ForeColor = Color.Green;
            lbl.Text = message;
            lbl.Visible = true;
        }

        /// <summary>
        /// مخفی کردن پیام
        /// </summary>
        public static void Hide(Label lbl)
        {
            if (lbl == null) return;
            lbl.Visible = false;
            lbl.Text = string.Empty;
        }
    }
}
