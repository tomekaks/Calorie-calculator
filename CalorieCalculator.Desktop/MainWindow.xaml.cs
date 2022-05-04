using CalorieCalculator.Core.DataBase.Models;
using CalorieCalculator.Desktop.ViewModels;
using System;
using System.Windows;



namespace CalorieCalculator.Desktop
{
 
    public partial class MainWindow : Window
    {
        private ProductViewModel product;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                InitializeTodaysMacros();
                DataContext = product;
            }
            catch (Exception)
            {
                MessageBox.Show("An unhandled exception has occured.");
            }
        }

        private void setDailyGoalsButton_Click(object sender, RoutedEventArgs e)
        {
            double dailyCaloriesGoal = InputDataParsing(dailyCaloriesGoalTextBox.Text);
            double dailyProteinsGoal = InputDataParsing(dailyProteinsGoalTextBox.Text);
            double dailyCarbsGoal = InputDataParsing(dailyCarbsGoalTextBox.Text);
            double dailyFatsGoal = InputDataParsing(dailyFatsGoalTextBox.Text);
            dailyCaloriesGoalTextBox.Text = "";
            dailyProteinsGoalTextBox.Text = "";
            dailyCarbsGoalTextBox.Text = "";
            dailyFatsGoalTextBox.Text = "";
            product.UpdateDailyGoals(dailyCaloriesGoal, dailyProteinsGoal, dailyCarbsGoal, dailyFatsGoal);
            product.SetDailyGoals();
        }
        private void addToDairyButton_Click(object sender, RoutedEventArgs e)
        {
            product.AddFoodToTodaysMacros(GetWeight());
            weightTextBox.Text = "";
        }
        private void newProductButton_Click(object sender, RoutedEventArgs e)
        {
            string newProductName = newProductNameTextBox.Text;
            var newProductCalories = InputDataParsing(newProductCaloriesTextBox.Text);
            var newProductProteins = InputDataParsing(newProductProteinsTextBox.Text);
            var newProductCarbs = InputDataParsing(newProductCarbsTextBox.Text);
            var newProductFats = InputDataParsing(newProductFatsTextBox.Text);
            newProductNameTextBox.Text = "";
            newProductCaloriesTextBox.Text = "";
            newProductProteinsTextBox.Text = "";
            newProductCarbsTextBox.Text = "";
            newProductFatsTextBox.Text = "";
            var exists = product.CheckIfProductIsOnList(newProductName);
            if (exists)
            {
                MessageBox.Show("This product already exists on the list.");
            }
            else
            {
                product.AddNewProductToList(newProductName, newProductCalories, newProductProteins, newProductCarbs, newProductFats);
            }
            
        }
        private void deleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            product.DeleteProductFromList();

        }

        private void InitializeTodaysMacros()
        {
            product = new ProductViewModel(GetTodaysDate());
        }
        private string GetTodaysDate()
        {
            DateTime now = DateTime.Today;
            string todaysDate = now.ToString("d");
            return todaysDate;
        }
        private double GetWeight()
        {
            double weight;
            if (double.TryParse(weightTextBox.Text, out weight))
            {
                weight = weight / 100;
                return weight;
            }
            MessageBox.Show("Put in correct weight.");
            return 0;
        }
        private double InputDataParsing(string textBox)
        {
            if (double.TryParse(textBox, out double result))
                return result;
            else
                MessageBox.Show("Put in correct data.");
            return 0;
        }


    }

}
