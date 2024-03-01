using Lab01Rudnyk.Models;
using System.Windows;

namespace Lab01Rudnyk.ViewModels
{
    internal class UserBirthViewModel
    {
        private UserBirthInfo _user = new UserBirthInfo();
        private bool _isDateOfBirthCorrect = false;
        private enum Month{ January = 1, February, March, April, May, June, July, August, September, October, November, December }
        public int Age
        {
            get { return _user.Age; }
        }

        public bool IsDateOfBirthCorrect
        {
            get { return _isDateOfBirthCorrect; }
        }
        public string WesternZodiacSign
        {
            get { return _user.WesternZodiacSign; }
        }

        public string ChineseZodiacSign
        {
            get { return _user.ChineseZodiacSign; }
        }

        public DateTime DateOfBirth
        {
            get { return _user.DateOfBirth; }
            set {

                if (value > DateTime.Now)
                {
                    MessageBox.Show("User hasn't been born yet");
                    _isDateOfBirthCorrect = false;
                    return;
                }
                int age = CalculateAge(value);
                
                if (age > 135)
                {
                    MessageBox.Show("User is older than 135");
                    _isDateOfBirthCorrect = false;
                    return;
                }
                _user.DateOfBirth = value;
                _user.Age = age;
                _isDateOfBirthCorrect = true;
                FindZodiacs();
            }
        }

        private void FindZodiacs()
        {
            var dayOfBirth = _user.DateOfBirth.Day;
            Month monthOfBirth = (Month)_user.DateOfBirth.Month;   
            var yearOfBirth = _user.DateOfBirth.Year;
                      
            _user.WesternZodiacSign = FindWesternZodiac(dayOfBirth, monthOfBirth);
            _user.ChineseZodiacSign = FindChineseZodiac(yearOfBirth);
        }

        private string FindWesternZodiac(int day, Month month)
        {
            string westernAstroSign;
            switch (month)
            {
                case Month.December:
                    westernAstroSign = (day < 22) ? "Sagittarius" : "Capricorn";
                    break;
                case Month.January:
                    westernAstroSign = (day < 20) ? "Capricorn" : "Aquarius";
                    break;
                case Month.February:
                    westernAstroSign = (day < 19) ? "Aquarius" : "Pisces";
                    break;
                case Month.March:
                    westernAstroSign = (day < 21) ? "Pisces" : "Aries";
                    break;
                case Month.April:
                    westernAstroSign = (day < 20) ? "Aries" : "Taurus";
                    break;
                case Month.May:
                    westernAstroSign = (day < 21) ? "Taurus" : "Gemini";
                    break;
                case Month.June:
                    westernAstroSign = (day < 21) ? "Gemini" : "Cancer";
                    break;
                case Month.July:
                    westernAstroSign = (day < 23) ? "Cancer" : "Leo";
                    break;
                case Month.August:
                    westernAstroSign = (day < 23) ? "Leo" : "Virgo";
                    break;
                case Month.September:
                    westernAstroSign = (day < 23) ? "Virgo" : "Libra";
                    break;
                case Month.October:
                    westernAstroSign = (day < 23) ? "Libra" : "Scorpio";
                    break;
                case Month.November:
                    westernAstroSign = (day < 22) ? "Scorpio" : "Sagittarius";
                    break;
                default:
                    westernAstroSign = "Invalid";
                    break;
            }
            return westernAstroSign;
        }
        private string FindChineseZodiac(int year)
        {
            string chineseZodiacSign;
            switch (year % 12)
            {
                case 0:
                    chineseZodiacSign = "Monkey";
                    break;
                case 1:
                    chineseZodiacSign = "Rooster";
                    break;
                case 2:
                    chineseZodiacSign = "Dog";
                    break;
                case 3:
                    chineseZodiacSign = "Pig";
                    break;
                case 4:
                    chineseZodiacSign = "Rat";
                    break;
                case 5:
                    chineseZodiacSign = "Ox";
                    break;
                case 6:
                    chineseZodiacSign = "Tiger";
                    break;
                case 7:
                    chineseZodiacSign = "Rabbit";
                    break;
                case 8:
                    chineseZodiacSign = "Dragon";
                    break;
                case 9:
                    chineseZodiacSign = "Snake";
                    break;
                case 10:
                    chineseZodiacSign = "Horse";
                    break;
                default:
                    chineseZodiacSign = "Sheep";
                    break;
            }
            return chineseZodiacSign;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var convertedToday = (today.Year * 100 + today.Month) * 100 + today.Day;
            var convertedBD = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (convertedToday - convertedBD) / 10000;
        }
    }
}
