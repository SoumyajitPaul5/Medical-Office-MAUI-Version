using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Office_MAUI.Models
{
    public class Patient
    {
        public int ID { get; set; } // Unique identifier for the patient

        // Property to get the patient's summary name in the format "FirstName M. LastName"
        public string Summary
        {
            get
            {
                return FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }

        // Property to get the patient's age and associated doctor's summary
        public string AgeDoctor
        {
            get
            {
                if (DOB == DateTime.MinValue)
                {
                    return "Age: Unknown" + (string.IsNullOrEmpty(Doctor?.Summary)
                        ? "" : " - " + Doctor?.Summary);
                }
                DateTime today = DateTime.Today;
                int age = today.Year - DOB.Year
                    - (today.Month < DOB.Month || (today.Month == DOB.Month && today.Day < DOB.Day) ? 1 : 0);
                return "Age: " + age.ToString().PadLeft(3) + (string.IsNullOrEmpty(Doctor?.Summary)
                        ? "" : " - " + Doctor?.Summary);
            }
        }

        public string FirstName { get; set; } // First name of the patient

        public string MiddleName { get; set; } // Middle name of the patient

        public string LastName { get; set; } // Last name of the patient

        public string OHIP { get; set; } // Ontario Health Insurance Plan number of the patient

        public DateTime DOB { get; set; } // Date of birth of the patient

        public byte ExpYrVisits { get; set; } // Expected yearly visits for the patient

        public byte[] RowVersion { get; set; } // For concurrency control in database operations

        public int DoctorID { get; set; } // Foreign key to associate the patient with a doctor
        public Doctor Doctor { get; set; } // Navigation property to the associated doctor
    }
}
