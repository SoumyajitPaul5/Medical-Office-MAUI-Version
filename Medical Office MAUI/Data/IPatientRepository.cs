using Medical_Office_MAUI.Models; // Importing the Models namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Office_MAUI.Data
{
    // Interface defining the contract for a repository that handles Patient data operations
    public interface IPatientRepository
    {
        // Method to retrieve a list of all patients asynchronously
        Task<List<Patient>> GetPatients();

        // Method to retrieve a specific patient by their ID asynchronously
        Task<Patient> GetPatient(int ID);

        // Method to retrieve a list of patients associated with a specific doctor asynchronously
        Task<List<Patient>> GetPatientsByDoctor(int DoctorID);

        // Method to add a new patient asynchronously
        Task AddPatient(Patient patientToAdd);

        // Method to update an existing patient asynchronously
        Task UpdatePatient(Patient patientToUpdate);

        // Method to delete an existing patient asynchronously
        Task DeletePatient(Patient patientToDelete);
    }
}
