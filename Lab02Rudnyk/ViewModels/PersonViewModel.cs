using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Lab02Rudnyk.Models;
using Lab02Rudnyk.Tools;
namespace Lab02Rudnyk.ViewModels
{
    internal class PersonViewModel : INotifyPropertyChanged
    {
        private Person _person = new Person("","","");
        private RelayCommand _proceed;

        private bool _isEnabled = true;

        public bool IsEnabled { 
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

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



        private async void Proceed()
        {


            try
            {
                IsEnabled = false;
                var tempPerson = new Person(FirstName, LastName, EmailAddress, DateOfBirth);
                await tempPerson.CalculateAdditionalFieldsAsync();
                string isAdult = tempPerson.IsAdult ? "Повнолітна людина" : "Не повнолітна людина";
                string isBDToday = tempPerson.IsBirthday ? "Вітаю з сьогоднішнім ДР" : "Не сьогодні ДР";
                MessageBox.Show($"Ім'я: {tempPerson.FirstName}\nПрізвище: {tempPerson.LastName}\nЕлектронна пошта: {tempPerson.EmailAddress}\nДата народження: {tempPerson.DateOfBirth.ToShortDateString()}\n" +
                                $"\n{isBDToday}\n{isAdult}\nЗнак гороскопу: {tempPerson.SunSign}\nКитайський знак: {tempPerson.ChineseSign}");
            }
            catch (FutureBirthDateException)
            {
                MessageBox.Show("Помилка. День народження в майбутньому");
            }
            catch (OldBirthDateException)
            {
                MessageBox.Show("Помилка. Людині більше 135р.");
            }
            catch (InvalidEmailException)
            {
                MessageBox.Show("Помилка. Адреса електронної пошти не правильна");
            }
            finally
            {
                IsEnabled = true;
            }
        }


            

        private bool CanExecute()
        {
            return (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) &&
                               !string.IsNullOrEmpty(EmailAddress) && DateOfBirth != DateTime.MinValue) ;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
