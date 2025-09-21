using OfficeAutomation.WinForms.Presentation.Controls;
using OfficeAutomation.Client.Interfaces;
using OfficeAutomation.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OfficeAutomation.WinForms.Presentation.Views
{
    public partial class CartableView : BaseContentControl
    {
        private readonly ICartableClient _cartableClient;

        public CartableView(ICartableClient cartableClient)
        {
            InitializeComponent();
            _cartableClient = cartableClient ?? throw new ArgumentNullException(nameof(cartableClient));
        }

        // Load data when the control is displayed
        public override async void OnLoadData()
        {
            await LoadCartableAsync();
        }

        // Fetches cartable items from API and populates the DataGridView
        private async Task LoadCartableAsync()
        {
            try
            {
                // Clear previous data
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                // Define columns
                dataGridView1.Columns.Add("Id", "ID");
                dataGridView1.Columns.Add("Title", "Title");
                dataGridView1.Columns.Add("Status", "Status");
                dataGridView1.Columns.Add("Description", "Description");
                dataGridView1.Columns.Add("ReceivedAt", "Received At");
                dataGridView1.Columns.Add("AssignedTo", "Assigned To");

                // Fetch data from API
                List<CartableItemDto> items = await _cartableClient.GetAllAsync();

                if (items == null || items.Count == 0)
                {
                    ShowError("No cartable items found.");
                    return;
                }

                // Populate DataGridView
                foreach (var item in items)
                {
                    try
                    {
                        dataGridView1.Rows.Add(
                            item.CartableId,
                            item.Title ?? "-",
                            item.Status ?? "-",
                            item.Description ?? "-",
                            item.ReceivedAt == DateTime.MinValue ? "-" : item.ReceivedAt.ToString("yyyy-MM-dd HH:mm"),
                            item.AssignedToFullName ?? "-"
                        );
                    }
                    catch (Exception rowEx)
                    {
                        // Show error for this row only and continue
                        ShowError($"Failed to add row ID {item.CartableId}: {rowEx.Message}");
                    }
                }

                ShowSuccess("Cartable loaded successfully!");
            }
            catch (HttpRequestException httpEx)
            {
                ShowError($"Failed to load cartable due to network error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                ShowError($"Failed to load cartable: {ex.Message}");
            }
        }
    }
}
