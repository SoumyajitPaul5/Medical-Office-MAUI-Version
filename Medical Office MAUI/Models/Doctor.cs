using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Office_MAUI.Models
{
    public class Doctor
    {
        public int ID { get; set; } // Unique identifier for the doctor

        // Property to get the doctor's summary name in the format "Dr. FirstName M. LastName"
        public string Summary
        {
            get
            {
                return "Dr. " + FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }

        // Property to get the doctor's formal name in the format "LastName, FirstName M."
        public string FormalName
        {
            get
            {
                return LastName + ", " + FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? "" :
                        (" " + (char?)MiddleName[0] + ".").ToUpper());
            }
        }

        public string FirstName { get; set; } // First name of the doctor

        public string MiddleName { get; set; } // Middle name of the doctor

        public string LastName { get; set; } // Last name of the doctor

        public byte[] RowVersion { get; set; } // For concurrency control in database operations

        public ICollection<Patient> Patients { get; set; } // Collection of patients associated with the doctor
    }
}
