using Medical_Office_MAUI.Data;
using Medical_Office_MAUI.Models;
using Medical_Office_MAUI.Utilities;
using System.Text;

namespace Medical_Office_MAUI
{
    public partial class MainPage : ContentPage
    {
        private App thisApp; // Reference to the current application instance
        private List<Patient> patients; // List of patients displayed on the page
        private bool needRefresh; // Flag to determine if data needs to be refreshed

        public MainPage()
        {
            InitializeComponent(); // Initialize the page components
            thisApp = Application.Current as App; // Get the current application instance
            thisApp.doctorRepository = new DoctorRepository(); // Initialize the doctor repository
            thisApp.patientRepository = new PatientRepository(); // Initialize the patient repository
            thisApp.Doctors = new List<Doctor>(); // Initialize the list of doctors in the application
            needRefresh = true; // Set the needRefresh flag to true
            patients = new List<Patient>(); // Initialize the list of patients
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ShowData(); // Show data on the page when it appears
        }

        private async Task ShowData()
        {
            btnAdd.IsEnabled = false; // Disable the add button

            if (needRefresh)
            {
                await ShowDoctors(); // Show the list of doctors
                await ShowPatients(null); // Show the list of patients
                PatientList.SelectedItem = null; // Clear the selected item
                needRefresh = false; // Set needRefresh to false
            }
            else
            {
                if (ddlDoctors.SelectedIndex < 1)
                {
                    await ShowPatients(null); // Show patients for all doctors
                }
                else
                {
                    Doctor selDoctor = (Doctor)ddlDoctors.SelectedItem;
                    await ShowPatients(selDoctor.ID); // Show patients for the selected doctor
                }
            }

            btnAdd.IsEnabled = true; // Re-enable the add button
        }

        private async Task ShowDoctors()
        {
            if (thisApp.Doctors?.Count == 0)
            {
                Loading.IsRunning = true; // Show loading indicator

                try
                {
                    thisApp.Doctors = await thisApp.doctorRepository.GetDoctors(); // Get the list of doctors
                }
                catch (ApiException apiEx)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Errors:");
                    foreach (var error in apiEx.Errors)
                    {
                        sb.AppendLine("-" + error);
                    }
                    await DisplayAlert("Problem Getting List of Doctors:", sb.ToString(), "Ok"); // Display alert for API exception
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.GetBaseException().Message.Contains("connection with the server"))
                        {
                            await DisplayAlert("Error", "No connection with the server. Check that the Web Service is running and available and then click the Refresh button.", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Error", "If the problem persists, please call your system administrator.", "Ok");
                        }
                    }
                    else
                    {
                        if (ex.Message.Contains("NameResolutionFailure"))
                        {
                            await DisplayAlert("Internet Access Error ", "Cannot resolve the Uri: " + Jeeves.DBUri.ToString(), "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Error", ex.GetBaseException().Message, "Ok");
                        }
                    }
                }
                finally
                {
                    Loading.IsRunning = false; // Hide loading indicator
                }
            }

            List<Doctor> doctors = new List<Doctor>();
            doctors.Add(new Doctor { ID = 0, LastName = " All Doctors" });
            foreach (Doctor d in thisApp.Doctors.OrderBy(d => d.FormalName))
            {
                doctors.Add(d);
            }
            ddlDoctors.ItemsSource = doctors;
            ddlDoctors.SelectedIndex = 0;
        }

        private async Task ShowPatients(int? DoctorID)
        {
            Loading.IsRunning = true; // Show loading indicator
            try
            {
                if (DoctorID.GetValueOrDefault() > 0)
                {
                    patients = await thisApp.patientRepository.GetPatientsByDoctor(DoctorID.GetValueOrDefault()); // Get patients for a specific doctor
                }
                else
                {
                    patients = await thisApp.patientRepository.GetPatients(); // Get all patients
                }
                PatientList.ItemsSource = patients; // Set the patient list as the item source
                PatientList.IsVisible = true; // Show the patient list
            }
            catch (ApiException apiEx)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Errors:");
                foreach (var error in apiEx.Errors)
                {
                    sb.AppendLine("-" + error);
                }
                PatientList.IsVisible = false; // Hide the patient list
                await DisplayAlert("Error Getting Patients:", sb.ToString(), "Ok"); // Display alert for API exception
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.GetBaseException().Message.Contains("connection with the server"))
                    {
                        await DisplayAlert("Error", "No connection with the server. Check that the Web Service is running and available and then click the Refresh button.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Error", "If the problem persists, please call your system administrator.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Error", ex.GetBaseException().Message, "Ok");
                }
            }
            finally
            {
                Loading.IsRunning = false; // Hide loading indicator
            }
        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            Patient newPatient = new()
            {
                DOB = DateTime.Today.AddDays(-1),
                ExpYrVisits = 2
            };
            var PatientDetailPage = new PatientDetailPage
            {
                BindingContext = newPatient
            };
            PatientList.IsVisible = false; // Hide the patient list
            await Navigation.PushAsync(PatientDetailPage); // Navigate to the patient detail page
        }
        // Handles the event when a patient is selected from the list
        private async void PatientSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var Patient = (Patient)e.SelectedItem;
                var PatientDetailPage = new PatientDetailPage
                {
                    BindingContext = Patient
                };
                PatientList.IsVisible = false; // Hide the patient list
                await Navigation.PushAsync(PatientDetailPage); // Navigate to the patient detail page
            }
        }

        // Handles the event when a doctor is selected from the dropdown
        private async void ddlDoctors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDoctors.SelectedIndex < 1)
            {
                await ShowPatients(null); // Show patients for all doctors
            }
            else
            {
                Doctor selDoctor = (Doctor)ddlDoctors.SelectedItem;
                await ShowPatients(selDoctor.ID); // Show patients for the selected doctor
            }
        }

        // Handles the event when the refresh button is clicked
        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            needRefresh = true; // Set the needRefresh flag to true
            _ = ShowData(); // Show the data on the page
        }

    }

}
