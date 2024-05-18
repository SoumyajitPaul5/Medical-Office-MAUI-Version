using Medical_Office_MAUI.Models; // Importing the Models namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Office_MAUI.Data
{
    // Interface defining the contract for a repository that handles Doctor data operations
    public interface IDoctorRepository
    {
        // Method to retrieve a list of all doctors asynchronously
        Task<List<Doctor>> GetDoctors();

        // Method to retrieve a specific doctor by their ID asynchronously
        Task<Doctor> GetDoctor(int ID);

        // Method to add a new doctor asynchronously
        Task AddDoctor(Doctor doctorToAdd);

        // Method to update an existing doctor asynchronously
        Task UpdateDoctor(Doctor doctorToUpdate);

        // Method to delete an existing doctor asynchronously
        Task DeleteDoctor(Doctor doctorToDelete);
    }
}
