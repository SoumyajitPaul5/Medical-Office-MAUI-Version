using Medical_Office_MAUI.Models;
using Medical_Office_MAUI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Office_MAUI.Data
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HttpClient client = new HttpClient(); // Instance of HttpClient to make API requests

        public DoctorRepository()
        {
            client.BaseAddress = Jeeves.DBUri; // Set the base address for HTTP requests
            client.DefaultRequestHeaders.Accept.Clear(); // Clear any existing request headers
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // Set the header to accept JSON responses
        }

        public async Task<List<Doctor>> GetDoctors()
        {
            HttpResponseMessage response = await client.GetAsync("api/doctor"); // Send a GET request to retrieve doctors
            if (response.IsSuccessStatusCode)
            {
                List<Doctor> doctors = await response.Content.ReadAsAsync<List<Doctor>>(); // Parse response content as a list of Doctor objects
                return doctors; // Return the list of doctors
            }
            else
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }

        public async Task<Doctor> GetDoctor(int DoctorID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/doctor/{DoctorID}"); // Send a GET request to retrieve a specific doctor by ID
            if (response.IsSuccessStatusCode)
            {
                Doctor doctor = await response.Content.ReadAsAsync<Doctor>(); // Parse response content as a Doctor object
                return doctor; // Return the doctor object
            }
            else
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }

        public async Task AddDoctor(Doctor doctorToAdd)
        {
            var response = await client.PostAsJsonAsync("/api/doctor", doctorToAdd); // Send a POST request to add a new doctor
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }

        public async Task UpdateDoctor(Doctor doctorToUpdate)
        {
            var response = await client.PutAsJsonAsync($"/api/doctor/{doctorToUpdate.ID}", doctorToUpdate); // Send a PUT request to update an existing doctor
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }

        public async Task DeleteDoctor(Doctor doctorToDelete)
        {
            var response = await client.DeleteAsync($"/api/doctor/{doctorToDelete.ID}"); // Send a DELETE request to delete an existing doctor
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }
    }
}
