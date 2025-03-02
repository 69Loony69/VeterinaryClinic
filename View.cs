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
    
    public partial class View
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public View()
        {
            this.Patients = new HashSet<Patients>();
        }
    
        public int ViewId { get; set; }
        public string Name { get; set; }

        public string ImageFileName
        {
            get
            {
                if (Name == "Собака")
                {
                    return @"\Image\Dog.png";
                }
                else if (Name == "Кошка/кот")
                {
                    return @"\Image\Cat.png";
                }
                else if (Name == "Грызун")
                {
                    return @"\Image\Hamster.png";
                }
                else if (Name == "Птица")
                {
                    return @"\Image\Parrot.png";
                }
                else if (Name == "Рептилия")
                {
                    return @"\Image\Turtle.png";
                }
                else
                {
                    return @"\Image\Animals.png";
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Patients> Patients { get; set; }
    }
}
