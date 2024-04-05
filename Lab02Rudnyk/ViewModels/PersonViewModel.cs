using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Lab02Rudnyk.Models;
using Lab02Rudnyk.Repositories;
using Lab02Rudnyk.Tools;
namespace Lab02Rudnyk.ViewModels
{
    internal class PersonViewModel : INotifyPropertyChanged
    {
        private static PersonManager PersonManager = new PersonManager();
        private Person _person = new Person("","","");
        private RelayCommand _proceed;
        private RelayCommand _deleteCommand;
        private RelayCommand _editCommand;
        private ObservableCollection<Person> _users = new ObservableCollection<Person>();

        public ObservableCollection<Person> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }


        private bool _isEnabled = true;
        private Visibility _loaderVisibility = Visibility.Collapsed;

        public bool IsEnabled { 
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public Visibility LoaderVisibility
        {
            get => _loaderVisibility;
            set
            {
                _loaderVisibility = value;
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

        public RelayCommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand(Edit, CanExecute);
                }
                return _editCommand;
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(Delete, CanExecuteDelete);
                }
                return _deleteCommand;
            }
        }



        public PersonViewModel()
        {
            LoadUsers();
        }

        private async void Proceed()
        {
            try
            {
                IsEnabled = false;
                LoaderVisibility = Visibility.Visible;

                if (!IsEmailUnique(EmailAddress))
                {
                    MessageBox.Show("Користувач з такою поштою вже існує!");
                    return;
                }

                var tempPerson = new Person(FirstName, LastName, EmailAddress, DateOfBirth);
                await tempPerson.CalculateAdditionalFieldsAsync();
                /*string isAdult = tempPerson.IsAdult ? "Повнолітна людина" : "Не повнолітна людина";
                string isBDToday = tempPerson.IsBirthday ? "Вітаю з сьогоднішнім ДР" : "Не сьогодні ДР";
                MessageBox.Show($"Ім'я: {tempPerson.FirstName}\nПрізвище: {tempPerson.LastName}\nЕлектронна пошта: {tempPerson.EmailAddress}\nДата народження: {tempPerson.DateOfBirth.ToShortDateString()}\n" +
                                $"\n{isBDToday}\n{isAdult}\nЗнак гороскопу: {tempPerson.SunSign}\nКитайський знак: {tempPerson.ChineseSign}");*/

                await PersonManager.AddOrUpdatePerson(tempPerson);

                Users.Add(tempPerson);
                OnPropertyChanged(nameof(Users));
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
                LoaderVisibility = Visibility.Collapsed;
            }
        }

        private async void Edit()
        {
            try
            {
                IsEnabled = false;
                LoaderVisibility = Visibility.Visible;

                if (IsEmailUnique(EmailAddress))
                {
                    MessageBox.Show("Користувача з такою поштою не існує!");
                    return;
                }

                var user = Users.FirstOrDefault(usr => usr.EmailAddress == EmailAddress);
                var index = Users.IndexOf(user);

                var tempPerson = new Person(FirstName, LastName, EmailAddress, DateOfBirth);
                await tempPerson.CalculateAdditionalFieldsAsync();


                if (index != -1)
                {
                    Users[index] = tempPerson;
                    OnPropertyChanged(nameof(Users));
                }

                await PersonManager.AddOrUpdatePerson(tempPerson);
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
                LoaderVisibility = Visibility.Collapsed;
            }
        }

        private async void Delete()
        {
            try
            {
                IsEnabled = false;
                LoaderVisibility = Visibility.Visible;

                if (IsEmailUnique(EmailAddress))
                {
                    MessageBox.Show("Користувача з такою поштою не існує!");
                    return;
                }

                PersonManager.DeletePerson(EmailAddress);

                var personToRemove = Users.FirstOrDefault(person => person.EmailAddress == EmailAddress);

                if (personToRemove != null)
                {
                    Users.Remove(personToRemove);
                    OnPropertyChanged(nameof(Users));
                }

            }
            finally
            {
                IsEnabled = true;
                LoaderVisibility = Visibility.Collapsed;
            }
        }

        private bool IsEmailUnique(string email)
        {
            foreach (var user in Users)
            {
                if (user.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        private async void LoadUsers()
        {
            try
            {
                var users = PersonManager.GetAllPersons();
                Users = new ObservableCollection<Person>(users);

                OnPropertyChanged(nameof(Users));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні користувачів: {ex.Message}");
            }
        }


        private bool CanExecute()
        {
            return (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) &&
                               !string.IsNullOrEmpty(EmailAddress) && DateOfBirth != DateTime.MinValue) ;
        }

        private bool CanExecuteDelete()
        {
            return !string.IsNullOrEmpty(EmailAddress);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
