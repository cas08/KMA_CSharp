using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01Rudnyk.Models
{
    internal class UserBirthInfo
    {
        #region Fields
        private DateTime _dateOfBirth;
        private int _age;
        private string _westernZodiacSign;
        private string _chineseZodiacSign;
        #endregion

        #region Properties
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public string WesternZodiacSign
        {
            get { return _westernZodiacSign; }
            set { _westernZodiacSign = value; }
        }

        public string ChineseZodiacSign
        {
            get { return _chineseZodiacSign; }
            set { _chineseZodiacSign = value; }
        } 
        #endregion

    }
}
