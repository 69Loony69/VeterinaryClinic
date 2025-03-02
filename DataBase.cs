using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryСlinic
{
    class DataBase
    {
        public Veterinary_Clinic baza { get; set; }

        public DataBase(Veterinary_Clinic baza)
        {
            this.baza = baza;
        }
    }
}
