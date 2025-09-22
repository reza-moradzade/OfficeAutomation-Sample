namespace OfficeAutomation.Blazor.Models
{
    public class CartableItemDto
    {
        public int CartableId { get; set; }       // شناسه رکورد کارتابل
        public int TaskId { get; set; }           // شناسه تسک مرتبط
        public string Title { get; set; } = "";   // عنوان تسک
        public string? Description { get; set; }  // توضیحات تسک
        public string Status { get; set; } = "";  // وضعیت (Pending, InProgress, Completed, ...)
        public DateTime? DueDate { get; set; }    // تاریخ سررسید
        public bool IsRead { get; set; }          // خوانده شده یا خیر
        public DateTime ReceivedAt { get; set; }  // تاریخ دریافت

        // 🔹 این دو پراپرتی جدید برای نمایش اطلاعات کاربر مسئول
        public string? AssignedToUserName { get; set; }
        public string? AssignedToFullName { get; set; }
    }
}
