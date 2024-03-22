using System.Net.Mail;
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
            Thread.Sleep(3000);
            DateTime valueToCheck = DateOfBirth.AddYears(18);
            return valueToCheck <= DateTime.Today;
        }

        private async Task<string> CalculateSunSign()
        {
            Thread.Sleep(3000);
            var day = DateOfBirth.Day;
            Month month = (Month)DateOfBirth.Month;
            string westernAstroSign;
            switch (month)
            {
                case Month.December:
                    westernAstroSign = (day < 22) ? "Стрілець" : "Козеріг";
                    break;
                case Month.January:
                    westernAstroSign = (day < 20) ? "Козеріг" : "Водолій";
                    break;
                case Month.February:
                    westernAstroSign = (day < 19) ? "Водолій" : "Риби";
                    break;
                case Month.March:
                    westernAstroSign = (day < 21) ? "Риби" : "Овен";
                    break;
                case Month.April:
                    westernAstroSign = (day < 20) ? "Овен" : "Телець";
                    break;
                case Month.May:
                    westernAstroSign = (day < 21) ? "Телець" : "Близнюки";
                    break;
                case Month.June:
                    westernAstroSign = (day < 21) ? "Близнюки" : "Рак";
                    break;
                case Month.July:
                    westernAstroSign = (day < 23) ? "Рак" : "Лев";
                    break;
                case Month.August:
                    westernAstroSign = (day < 23) ? "Лев" : "Діва";
                    break;
                case Month.September:
                    westernAstroSign = (day < 23) ? "Діва" : "Терези";
                    break;
                case Month.October:
                    westernAstroSign = (day < 23) ? "Терези" : "Скорпіон";
                    break;
                case Month.November:
                    westernAstroSign = (day < 22) ? "Скорпіон" : "Стрілець";
                    break;
                default:
                    westernAstroSign = "Invalid";
                    break;          
            }
            return westernAstroSign;
        }

        private async Task<string> CalculateChineseSign()
        {
            Thread.Sleep(3000);
            string chineseZodiacSign;
            switch (DateOfBirth.Year % 12)
            {
                case 0:
                    chineseZodiacSign = "Мавпа";
                    break;
                case 1:
                    chineseZodiacSign = "Півень";
                    break;
                case 2:
                    chineseZodiacSign = "Собака";
                    break;
                case 3:
                    chineseZodiacSign = "Свиня";
                    break;
                case 4:
                    chineseZodiacSign = "Пацюк";
                    break;
                case 5:
                    chineseZodiacSign = "Бик";
                    break;
                case 6:
                    chineseZodiacSign = "Тигр";
                    break;
                case 7:
                    chineseZodiacSign = "Кролик";
                    break;
                case 8:
                    chineseZodiacSign = "Дракон";
                    break;
                case 9:
                    chineseZodiacSign = "Змія";
                    break;
                case 10:
                    chineseZodiacSign = "Кінь";
                    break;
                default:
                    chineseZodiacSign = "Коза";
                    break;
            }
            return chineseZodiacSign;
        }

        private async Task<bool> CalculateIsBirthday()
        {
            Thread.Sleep(3000);
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
