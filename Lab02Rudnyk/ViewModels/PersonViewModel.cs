using System.Windows;
using Lab02Rudnyk.Models;
using Lab02Rudnyk.Tools;
namespace Lab02Rudnyk.ViewModels
{
    internal class PersonViewModel
    {
        private Person _person = new Person("","","");
        private RelayCommand _proceed;

        public string FirstName
        {
            get
            {
                return _person.FirstName;
            }
            set
            {
                _person.FirstName = value;
            }
        }
        public string LastName
        {
            get => _person.LastName;
            set
            {
                _person.LastName = value;
            }
        }
        public string EmailAddress
        {
            get => _person.EmailAddress;
            set
            {
                _person.EmailAddress = value;
            }
        }

        public DateTime DateOfBirth
        {
            get { return _person.DateOfBirth; }
            set
            {
                _person.DateOfBirth = value;
            }
        }
        public RelayCommand ProceedCommand
        {
            get { 
                if (_proceed == null)
                {
                    _proceed = new RelayCommand(Proceed, CanExecute);
                }
                return _proceed; 
            }
        }
        private void Proceed()
        {
            var today = DateTime.Today;

            var convertedToday = (today.Year * 100 + today.Month) * 100 + today.Day;
            var convertedBD = (DateOfBirth.Year * 100 + DateOfBirth.Month) * 100 + DateOfBirth.Day;

            int age = (convertedToday - convertedBD) / 10000;

            if (DateTime.Now < DateOfBirth)
            {
                MessageBox.Show("Ще не народився!");
                return;
            }

            if (age > 135)
            {
                MessageBox.Show("Вік користувача > 135р!");
                return;
            }

            if (DateOfBirth.Month == DateTime.Today.Month && DateOfBirth.Day == DateTime.Today.Day)
            {
                MessageBox.Show("З Днем Народження!");
            }

            var tempPerson = new Person(FirstName, LastName, EmailAddress, DateOfBirth);
            string isAdult = tempPerson.IsAdult ? "Повнолітна людина" : "Не повнолітна людина";
            MessageBox.Show($"Ім'я: {tempPerson.FirstName}\nПрізвище: {tempPerson.LastName}\nЕлектронна пошта: {tempPerson.EmailAddress}\nДата народження: {tempPerson.DateOfBirth.ToShortDateString()}\n" +
                            $"Вік: {age}\n{isAdult}\nЗнак гороскопу: {tempPerson.SunSign}\nКитайський знак: {tempPerson.ChineseSign}");
        }

        private bool CanExecute()
        {
            return (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) &&
                               !string.IsNullOrEmpty(EmailAddress) && DateOfBirth != DateTime.MinValue) ;
        }
    }
}
