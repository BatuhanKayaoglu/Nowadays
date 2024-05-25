using Microsoft.AspNetCore.Mvc;
using Nowadays.Entity.Models;
using System.Text.Json;
using System.Text;

namespace Nowadays.Web.Controllers
{
    public class ReportController : Controller
    {
        public async Task<IActionResult> ListReport(Report report)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Report/");

            var response = await client.GetAsync("GetReport");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            return View();
        }

        public async Task<IActionResult> AddReport(Report report)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Report/");

            var jsonReport = JsonSerializer.Serialize(report);
            var content = new StringContent(jsonReport, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("Add", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("Add");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Report/");

            var response = await client.DeleteAsync($"Delete?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Update(Report report)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Report/");

            var jsonReport = JsonSerializer.Serialize(report);
            var content = new StringContent(jsonReport, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("Update", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            return RedirectToAction("List");
        }
    }
}
