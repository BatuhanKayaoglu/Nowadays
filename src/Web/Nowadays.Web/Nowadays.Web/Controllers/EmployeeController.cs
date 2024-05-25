using Microsoft.AspNetCore.Mvc;
using Nowadays.Common.ViewModels;
using System.Text.Json;
using System.Text;
using Nowadays.Entity.Models;

namespace Nowadays.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public async Task<IActionResult> ListEmployee(Employee employee)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Employee/");

            var response = await client.GetAsync("GetEmployee");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            return View();
        }


        public async Task<IActionResult> AddEmployee(AddEmployeeViewModel employee)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Employee/");

            var jsonEmployee = JsonSerializer.Serialize(employee);
            var content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
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
            client.BaseAddress = new Uri("https://localhost:44308/api/Employee/");

            var response = await client.DeleteAsync($"Delete?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Update(Employee employee)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44308/api/Employee/");

            var jsonEmployee = JsonSerializer.Serialize(employee);
            var content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("Update", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("List");
        }
    }
}
