using Microsoft.AspNetCore.Mvc;
using Nowadays.Common.ViewModels;
using System.Text.Json;
using System.Text;
using Nowadays.Entity.Models;

namespace Nowadays.Web.Controllers
{
    public class ProjectController : Controller
    {
        public async Task<IActionResult> ListProject(Project project)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Project/");

            var response = await client.GetAsync("GetProjects");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            return View();
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        public async Task<IActionResult> AddProject(AddProjectViewModel project)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Project/");

            var jsonProject = JsonSerializer.Serialize(project);
            var content = new StringContent(jsonProject, Encoding.UTF8, "application/json");
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
            client.BaseAddress = new Uri("https://localhost:44308/api/Project/");

            var response = await client.DeleteAsync($"Delete?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Update(Project project)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Project/");

            var jsonProject = JsonSerializer.Serialize(project);
            var content = new StringContent(jsonProject, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("Update", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("List");
        }
    }
}
