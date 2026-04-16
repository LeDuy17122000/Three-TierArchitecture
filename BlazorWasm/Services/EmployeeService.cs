using Business.DTOs;
using Business.Entities;
using System.Net.Http.Json;

namespace BlazorWasm.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient httpClient;
        public EmployeeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<ServiceResponse> AddAsync(Employee employee)
        {
            var response = await httpClient.PostAsJsonAsync("api/employee", employee);
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var response = await httpClient.DeleteAsync($"api/employee/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }

        public Task<List<Employee>> GetAsync()=>
            httpClient.GetFromJsonAsync<List<Employee>>("api/employee");

        public Task<Employee?> GetByIdAsync(int id)=>
            httpClient.GetFromJsonAsync<Employee>($"api/employee/{id}");


        public async Task<ServiceResponse> UpdateAsync(Employee employee)
        {
            var response = await httpClient.PutAsJsonAsync("api/employee", employee);
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }
    }
}
