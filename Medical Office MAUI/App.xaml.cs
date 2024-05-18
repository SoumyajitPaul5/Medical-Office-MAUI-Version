using Medical_Office_MAUI.Data; // Importing the Data namespace
using Medical_Office_MAUI.Models; // Importing the Models namespace

namespace Medical_Office_MAUI
{
    public partial class App : Application
    {
        public DoctorRepository doctorRepository; // Instance of DoctorRepository for accessing doctor data
        public PatientRepository patientRepository; // Instance of PatientRepository for accessing patient data
        public List<Doctor> Doctors; // List of doctors

        public App()
        {
            InitializeComponent(); // Initializing the application components

            MainPage = new AppShell(); // Setting the main page of the application to an instance of AppShell
        }
    }
}
