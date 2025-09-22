using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeAutomation.Client.Models;

namespace OfficeAutomation.Client.Interfaces
{
    public interface ICartableClient
    {
        /// <summary>
        /// دریافت همه آیتم‌های کارتابل کاربر فعلی
        /// </summary>
        Task<List<CartableItemDto>> GetAllAsync();
    }
}
