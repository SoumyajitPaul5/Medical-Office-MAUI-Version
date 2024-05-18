using Medical_Office_MAUI.Data;
using Medical_Office_MAUI.Models;
using Medical_Office_MAUI.Utilities;
using System.Text;

namespace Medical_Office_MAUI;

public partial class PatientDetailPage : ContentPage
{
    private Patient patient; // The patient whose details are being displayed or edited.
    private App thisApp; // Reference to the current application instance.
    public List<Doctor> doctors; // List of doctors available for selection.

    public PatientDetailPage()
    {
        InitializeComponent();
        thisApp = Application.Current as App;
        doctors = new List<Doctor>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Initialize patient from the binding context.
        patient = (Patient)this.BindingContext;

        // Set the title of the page based on whether the patient is new or existing.
        if (patient.ID == 0)
        {
            this.Title = "Add New Patient";
            // Add a default option for selecting a doctor.
            Doctor selectDoctor = new Doctor { ID = 0, LastName = " Select a Doctor" };
            doctors.Add(selectDoctor);
            btnDelete.IsEnabled = false;
        }
        else
        {
            this.Title = "Edit Patient Details";
            btnDelete.IsEnabled = true;
        }

        // Fill the doctor dropdown list.
        FillDoctor();
    }

    private void FillDoctor()
    {
        // Fill the doctors list and bind it to the dropdown list.
        foreach (Doctor d in thisApp.Doctors.OrderBy(d => d.FormalName))
        {
            doctors.Add(d);
        }
        ddlDoctors.ItemsSource = doctors;

        // Select the patient's primary care physician if it is set.
        if (patient.DoctorID >= 0)
        {
            ddlDoctors.SelectedItem = thisApp.Doctors.FirstOrDefault(d => d.ID == patient.DoctorID);
        }
    }

    private async void SaveClicked(object sender, EventArgs e)
    {
        try
        {
            // Set the selected doctor for the patient.
            patient.Doctor = (Doctor)ddlDoctors.SelectedItem;
            patient.DoctorID = (patient.Doctor?.ID).GetValueOrDefault();

            // Check if a doctor is selected before saving.
            if (patient.DoctorID > 0)
            {
                // Add or update the patient based on whether it is new or existing.
                if (patient.ID == 0)
                {
                    await thisApp.patientRepository.AddPatient(patient);
                }
                else
                {
                    await thisApp.patientRepository.UpdatePatient(patient);
                }
                // Navigate back to the previous page after saving.
                await Navigation.PopAsync();
            }
            else
            {
                // Display an alert if no doctor is selected.
                await DisplayAlert("Doctor Not Selected:", "You must select the Primary Care Physician for the Patient.", "Ok");
            }

        }
        catch (ApiException apiEx)
        {
            // Handle API exceptions and display the errors.
            var sb = new StringBuilder();
            sb.AppendLine("Errors:");
            foreach (var error in apiEx.Errors)
            {
                sb.AppendLine("-" + error);
            }
            await DisplayAlert("Problem Saving the Patient:", sb.ToString(), "Ok");
        }
        catch (Exception ex)
        {
            // Handle general exceptions and display an appropriate message.
            if (ex.GetBaseException().Message.Contains("connection with the server"))
            {
                await DisplayAlert("Error", "No connection with the server.", "Ok");
            }
            else
            {
                await DisplayAlert("Error", ex.GetBaseException().Message, "Ok");
            }
        }
    }


    private async void DeleteClicked(object sender, EventArgs e)
    {
        // Display a confirmation dialog before deleting the patient.
        var answer = await DisplayAlert("Confirm Delete", "Are you certain that you want to delete " + patient.Summary + "?", "Yes", "No");
        if (answer == true)
        {
            try
            {
                // Attempt to delete the patient using the repository.
                await thisApp.patientRepository.DeletePatient(patient);
                // Navigate back to the previous page after successful deletion.
                await Navigation.PopAsync();
            }
            catch (ApiException apiEx)
            {
                // Handle API exceptions and display the errors.
                var sb = new StringBuilder();
                sb.AppendLine("Errors:");
                foreach (var error in apiEx.Errors)
                {
                    sb.AppendLine("-" + error);
                }
                await DisplayAlert("Problem Deleting the Patient:", sb.ToString(), "Ok");
            }
            catch (Exception ex)
            {
                // Handle general exceptions and display an appropriate message.
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    await DisplayAlert("Error", "No connection with the server.", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", ex.GetBaseException().Message, "Ok");
                }
            }
        }
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        // Navigate back to the previous page without performing any action.
        Navigation.PopAsync();
    }


}