using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02Rudnyk.Models
{
    internal class InvalidEmailException : Exception
    {
        public InvalidEmailException() : base("ERROR: Неправильна адреса електронної пошти.") { }
    }
}
