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
    
    public partial class ReceptionServices
    {
        public int ReceptionServicesId { get; set; }
        public int ReceptionId { get; set; }
        public int ServiceId { get; set; }
    
        public virtual Reception Reception { get; set; }
        public virtual Services Services { get; set; }
    }
}
