using Microsoft.AspNetCore.Mvc;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models;
using System.Text;
using System.Text.Json;

namespace Nowadays.Web.Controllers
{
    public class CompanyController : Controller
    {
        public async Task<IActionResult> ListCompany(Company company)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Company/");

            var response = await client.GetAsync("GetCompany");

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

        public async Task<IActionResult> AddCompany(AddCompanyViewModel company)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Company/");

            var jsonCompany = JsonSerializer.Serialize(company);
            var content = new StringContent(jsonCompany, Encoding.UTF8, "application/json");
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
            client.BaseAddress = new Uri("https://localhost:44308/api/Company/");

            var response = await client.DeleteAsync($"Delete?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Update(Company company)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Company/");

            var jsonCompany = JsonSerializer.Serialize(company);
            var content = new StringContent(jsonCompany, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("Update", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("List");
        }


    }
}
