using Medical_Office_MAUI.Models; // Importing the Models namespace
using Medical_Office_MAUI.Utilities; // Importing the Utilities namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http; 
using System.Net.Http.Headers; // Importing the Http.Headers namespace for HTTP headers
using System.Text;
using System.Threading.Tasks;

namespace Medical_Office_MAUI.Data
{
    // Implementation of IPatientRepository to handle Patient data operations
    public class PatientRepository : IPatientRepository
    {
        private readonly HttpClient client = new HttpClient(); // Instance of HttpClient to make API requests

        public PatientRepository()
        {
            client.BaseAddress = Jeeves.DBUri; // Set the base address for HTTP requests
            client.DefaultRequestHeaders.Accept.Clear(); // Clear any existing request headers
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // Set the header to accept JSON responses
        }

        public async Task<List<Patient>> GetPatients()
        {
            HttpResponseMessage response = await client.GetAsync("api/patient"); // Send a GET request to retrieve patients
            if (response.IsSuccessStatusCode)
            {
                List<Patient> Patients = await response.Content.ReadAsAsync<List<Patient>>(); // Parse response content as a list of Patient objects
                return Patients; // Return the list of patients
            }
            else
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }

        public async Task<List<Patient>> GetPatientsByDoctor(int DoctorID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/patient/byDoctor/{DoctorID}"); // Send a GET request to retrieve patients by doctor ID
            if (response.IsSuccessStatusCode)
            {
                List<Patient> Patients = await response.Content.ReadAsAsync<List<Patient>>(); // Parse response content as a list of Patient objects
                return Patients; // Return the list of patients
            }
            else
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }

        public async Task<Patient> GetPatient(int ID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/patient/{ID}"); // Send a GET request to retrieve a specific patient by ID
            if (response.IsSuccessStatusCode)
            {
                Patient Patient = await response.Content.ReadAsAsync<Patient>(); // Parse response content as a Patient object
                return Patient; // Return the patient object
            }
            else
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }

        public async Task AddPatient(Patient patientToAdd)
        {
            var response = await client.PostAsJsonAsync("/api/patient", patientToAdd); // Send a POST request to add a new patient
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }

        public async Task UpdatePatient(Patient patientToUpdate)
        {
            var response = await client.PutAsJsonAsync($"/api/patient/{patientToUpdate.ID}", patientToUpdate); // Send a PUT request to update an existing patient
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }

        public async Task DeletePatient(Patient patientToDelete)
        {
            var response = await client.DeleteAsync($"/api/patient/{patientToDelete.ID}"); // Send a DELETE request to delete an existing patient
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response); // Create an exception for an unsuccessful response
                throw ex; // Throw the exception
            }
        }
    }
}
