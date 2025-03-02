//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VeterinaryСlinic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    public partial class Reception
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reception()
        {
            this.ReceptionServices = new HashSet<ReceptionServices>();
            this.Treatment = new HashSet<Treatment>();
            Date = new DateTime(2024, 1, 1);
        }
        private TimeSpan _time;
        public int ReceptionId { get; set; }
        public System.DateTime Date { get; set; }
        public string FormattedDate
        {
            get
            {
                return Date.ToString("dd.MM.yyyy");
            }
        }

        public TimeSpan Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
                OnPropertyChanged("FormattedTime");
            }
        }

        public string FormattedTime
        {
            get
            {
                return Time.ToString(@"hh\:mm");
            }
            set
            {
                TimeSpan time;
                if (TimeSpan.TryParseExact(value, @"hh\:mm", CultureInfo.InvariantCulture, out time))
                {
                    Time = time;
                }
            }
        }
        public int VeterinarianId { get; set; }
        public int PatientId { get; set; }
        public string Complaints { get; set; }
        public Nullable<int> DiagnosisId { get; set; }
    
        public virtual Diagnosis Diagnosis { get; set; }
        public virtual Patients Patients { get; set; }
        public virtual Veterinarians Veterinarians { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceptionServices> ReceptionServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Treatment> Treatment { get; set; }
        public virtual ICollection<Services> Services { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
