using Microsoft.AspNetCore.Mvc;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models;
using System.Text.Json;
using System.Text;

namespace Nowadays.Web.Controllers
{
    public class IssueController : Controller
    {
        public async Task<IActionResult> IssueEmployee(Issue issue)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Issue/");

            var response = await client.GetAsync("GetIssue");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            return View();
        }

        public async Task<IActionResult> AddIssue(AddIssueViewModel issue)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Issue/");

            var jsonIssue = JsonSerializer.Serialize(issue);
            var content = new StringContent(jsonIssue, Encoding.UTF8, "application/json");
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
            client.BaseAddress = new Uri("https://localhost:44308/api/Issue/");

            var response = await client.DeleteAsync($"Delete?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Update(Issue issue)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Issue/");

            var jsonIssue = JsonSerializer.Serialize(issue);
            var content = new StringContent(jsonIssue, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("Update", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("List");
        }
    }
}
