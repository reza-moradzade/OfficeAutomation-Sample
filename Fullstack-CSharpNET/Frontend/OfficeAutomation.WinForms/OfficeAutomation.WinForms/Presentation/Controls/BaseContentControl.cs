using OfficeAutomation.WinForms.Presentation.Forms;
using OfficeAutomation.WinForms.Presentation.Helpers;
using System.Windows.Forms;

namespace OfficeAutomation.WinForms.Presentation.Controls
{
    public partial class BaseContentControl : UserControl
    {
        public BaseContentControl()
        {
            this.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// نمایش پیام خطا روی فوتر
        /// </summary>
        public void ShowError(string message)
        {
            if (MainForm.StatusLabel != null)
                MessageHelper.ShowError(MainForm.StatusLabel, message);
        }

        /// <summary>
        /// نمایش پیام موفقیت روی فوتر
        /// </summary>
        public void ShowSuccess(string message)
        {
            if (MainForm.StatusLabel != null)
                MessageHelper.ShowSuccess(MainForm.StatusLabel, message);
        }

        /// <summary>
        /// مخفی کردن پیام
        /// </summary>
        public void ClearFeedback()
        {
            if (MainForm.StatusLabel != null)
                MessageHelper.Hide(MainForm.StatusLabel);
        }

        /// <summary>
        /// متد عمومی برای بارگذاری داده یا initialize کردن کنترل
        /// </summary>
        public virtual void OnLoadData()
        {
            // override شود در کنترل‌های فرزند
        }
    }
}
