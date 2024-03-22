using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02Rudnyk.Models
{
    internal class FutureBirthDateException:Exception
    {
        public FutureBirthDateException() : base("ERROR: Дата народження в майбутньому.") { }
    }
}
