using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows;

namespace Lab02Rudnyk.Models
{
    internal class Person
    {
        private enum Month { January = 1, February, March, April, May, June, July, August, September, October, November, December }
        #region Fields
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private DateTime _dateOfBirth;

        private bool _isAdult;
        private string _sunSign;
        private string _chineseSign;
        private bool _isBirthday;
        #endregion

        #region Properties
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }
        //additional:
        public bool IsAdult => _isAdult;
        public string SunSign => _sunSign;
        public string ChineseSign => _chineseSign;
        public bool IsBirthday => _isBirthday;
        #endregion

        #region Constructors

        

        public Person(string firstName, string lastName, string emailAddress, DateTime dateOfBirth)
        {
            _firstName = firstName;
            _lastName = lastName;
            _emailAddress = emailAddress;
            _dateOfBirth = dateOfBirth;

        }

        public Person(string firstName, string lastName, string emailAddress)
            : this(firstName, lastName, emailAddress, DateTime.MinValue) { }

        public Person(string firstName, string lastName, DateTime dateOfBirth)
            : this(firstName, lastName, string.Empty, dateOfBirth) { }

        public Person() { }

        [JsonConstructor]
        public Person(bool isAdult, string sunSign, string chineseSign, bool isBirthday)
        {
            _isAdult = isAdult;
            _sunSign = sunSign;
            _chineseSign = chineseSign;
            _isBirthday = isBirthday;
        }

        #endregion



        public async Task CalculateAdditionalFieldsAsync()
        {
            PersonBirthDateValidation();
            PersonEmailValidation();
            

            var isAdultTask = Task.Run(() => CalculateIsAdult());
            var sunSignTask = Task.Run(() => CalculateSunSign());
            var chineseSignTask = Task.Run(() => CalculateChineseSign());
            var isBirthdayTask = Task.Run(() => CalculateIsBirthday());

            _isAdult = await isAdultTask;
            _sunSign = await sunSignTask;
            _chineseSign = await chineseSignTask;
            _isBirthday = await isBirthdayTask;
        }


        private async Task<bool> CalculateIsAdult()
        {
            //Thread.Sleep(3000);
            DateTime valueToCheck = DateOfBirth.AddYears(18);
            return valueToCheck <= DateTime.Today;
        }

        private async Task<string> CalculateSunSign()
        {
            //Thread.Sleep(3000);
            var day = DateOfBirth.Day;
            Month month = (Month)DateOfBirth.Month;
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

        private async Task<string> CalculateChineseSign()
        {
            //Thread.Sleep(3000);
            string chineseZodiacSign;
            switch (DateOfBirth.Year % 12)
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
                    chineseZodiacSign = "Goat";
                    break;
            }
            return chineseZodiacSign;
        }

        private async Task<bool> CalculateIsBirthday()
        {
            //Thread.Sleep(3000);
            return _dateOfBirth.Month == DateTime.Today.Month && _dateOfBirth.Day == DateTime.Today.Day;
        }
        private void PersonEmailValidation()
        {
            string pattern = @"^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})$";
            if (!Regex.IsMatch(EmailAddress, pattern))
            {
                throw new InvalidEmailException();
            }
            
        }
        private void PersonBirthDateValidation()
        {
            if (DateTime.Now < DateOfBirth)
            {
                throw new FutureBirthDateException();
            }

            var today = DateTime.Today;

            var convertedToday = (today.Year * 100 + today.Month) * 100 + today.Day;
            var convertedBD = (DateOfBirth.Year * 100 + DateOfBirth.Month) * 100 + DateOfBirth.Day;

            int age = (convertedToday - convertedBD) / 10000;

            if (age > 135)
            {
                throw new OldBirthDateException();
            }
        }
    }
}
