using Lab01Rudnyk.ViewModels;
using System.Windows;
using System.Windows.Controls;


namespace Lab01Rudnyk.View
{
    /// <summary>
    /// Interaction logic for DateOfBirthPickerControl.xaml
    /// </summary>
    public partial class DateOfBirthPickerView : UserControl
    {
        private UserBirthViewModel _viewModel;
        public DateOfBirthPickerView()
        {
            InitializeComponent();
            DataContext = _viewModel = new UserBirthViewModel();
        }

        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            
            if (DPBirthDate.SelectedDate == null)
            {
                MessageBox.Show("Please, enter ur date of birth");
                ClearTextBoxes();
                return;
            }

            _viewModel.DateOfBirth = (DateTime)DPBirthDate.SelectedDate;

            if(!_viewModel.IsDateOfBirthCorrect) { 
                ClearTextBoxes(); 
                return; 
            }

            if(_viewModel.DateOfBirth == DateTime.Today)
            {
                MessageBox.Show("Today is your birthday, wish u good luck!");
            }
            TBAge.Text = $"You are {_viewModel.Age} y.o";
            TBWesternZodiacSign.Text = $"Your western zodiac sign is {_viewModel.WesternZodiacSign}";
            TBChineseZodiacSign.Text = $"Your chinese zodiac sign is {_viewModel.ChineseZodiacSign}";
        }
        private void ClearTextBoxes()
        {
            TBAge.Text = "";
            TBWesternZodiacSign.Text = "";
            TBChineseZodiacSign.Text = "";
        }
    }
}
