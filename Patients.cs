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
    
    public partial class Patients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patients()
        {
            this.Reception = new HashSet<Reception>();
        }
    
        public int PatientId { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public int ViewId { get; set; }
        public string Breed { get; set; }
        public string Paul { get; set; }
        public Nullable<System.DateTime> DayOfBirth { get; set; }

        public string FormattedDayOfBirth
        {
            get
            {
                if (DayOfBirth.HasValue)
                {
                    return DayOfBirth.Value.ToString("dd.MM.yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public virtual Owners Owners { get; set; }
        public virtual View View { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reception> Reception { get; set; }
    }
}
