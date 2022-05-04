using CalorieCalculator.Core.DataBase.Models;
using CalorieCalculator.Core.DataBase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace CalorieCalculator.Desktop.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly DbServiceEf _db;
        private readonly CalorieCalculatorService _service;
        private List<ProductDto> _productList;
        private ObservableCollection<ProductDto> _products;
        private double _dailyCaloriesGoal;
        private double _dailyProteinsGoal;
        private double _dailyCarbsGoal;
        private double _dailyFatsGoal;
        private ProductDto _selectedProduct;
        private double _calories;
        private double _proteins;
        private double _carbs;
        private double _fats;

        public ProductViewModel(string today)
        {
            _db = new DbServiceEf();
            _service = new CalorieCalculatorService(_db);
            GetTodaysMacros(today);
            _productList = _service.GetProductList();
            SetDailyGoals();
            _products = new ObservableCollection<ProductDto>(_productList);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ProductDto SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            } 
        }
        public ObservableCollection<ProductDto> Products
        {
            get { return _products; }
            set { _products = value; }
        }

        public string Date { get; set; }
        public double Calories
        {
            get { return _calories; }
            set 
            {
                _calories = value;
                OnPropertyChanged("Calories");
                OnPropertyChanged("RemainingDailyCalories");
            }
        }
        public double Proteins
        {
            get { return _proteins; }
            set 
            {
                _proteins = value;
                OnPropertyChanged("Proteins");
                OnPropertyChanged("RemainingDailyProteins");
            }
        }
        public double Carbs 
        { 
            get { return _carbs; }
            set 
            {
                _carbs = value;
                OnPropertyChanged("Carbs");
                OnPropertyChanged("RemainingDailyCarbs");
            } 
        }
        public double Fats
        {
            get { return _fats; }
            set 
            { 
                _fats = value;
                OnPropertyChanged("Fats");
                OnPropertyChanged("RemainingDailyFats");
            }
        }
        public double RemainingDailyCalories
        {
            get { return _dailyCaloriesGoal - Calories; }
            set { }
        }
        public double RemainingDailyProteins
        {
            get { return _dailyProteinsGoal - Proteins; }
            set { }
        }
        public double RemainingDailyCarbs
        {
            get { return _dailyCarbsGoal - Carbs; }
            set { }
        }
        public double RemainingDailyFats
        {
            get { return _dailyFatsGoal - Fats; }
            set { }
        }
        public double DailyCaloriesGoal
        {
            get { return _dailyCaloriesGoal; }
            set
            {
                _dailyCaloriesGoal = value;
                OnPropertyChanged("DailyCaloriesGoal");
                OnPropertyChanged("RemainingDailyCalories");
            }
        }
        public double DailyProteinsGoal
        {
            get { return _dailyProteinsGoal; }
            set 
            { 
                _dailyProteinsGoal = value;
                OnPropertyChanged("DailyProteinsGoal");
                OnPropertyChanged("RemainingDailyProteins");
            }
        }
        public double DailyCarbsGoal
        {
            get { return _dailyCarbsGoal; }
            set 
            {
                _dailyCarbsGoal = value;
                OnPropertyChanged("DailyCarbsGoal");
                OnPropertyChanged("RemainingDailyCarbs");
            }
        }
        public double DailyFatsGoal
        {
            get { return _dailyFatsGoal; }
            set
            {
                _dailyFatsGoal = value;
                OnPropertyChanged("DailyFatsGoal");
                OnPropertyChanged("RemainingDailyFats");
            }
        }

        public void AddFoodToTodaysMacros(double weight)
        {            
            Calories += SelectedProduct.Calories * weight;
            Proteins += SelectedProduct.Proteins * weight;
            Carbs += SelectedProduct.Carbs * weight;
            Fats += SelectedProduct.Fats * weight;
            BeginNewOrUpdateTodaysMacros();
        }
        public void BeginNewOrUpdateTodaysMacros()
        {
            var todaysMacros = _service.CreateNewDailySumDto(Date, Calories, Proteins, Carbs, Fats);
            _service.BeginNewOrUpdateTodaysMacros(todaysMacros);
        }
        public void AddNewProductToList(string name, double cal, double pro, double carb, double fat)
        {           
 
            Products.Add(_service.CreateNewProduct(name, cal, pro, carb, fat));
            _service.AddAProduct(name, cal, pro, carb, fat);
            _productList = _service.GetProductList();
        }
        public void DeleteProductFromList()
        {           
            _service.DeleteAProduct(SelectedProduct);
            Products.Remove(SelectedProduct);
        }
        public bool CheckIfProductIsOnList(string name)
        {
            var chosenProduct = Products.SingleOrDefault(x => x.Name == name);
            if (chosenProduct != null)
            {
                return true;
            }
            else
                return false;

        }
        public void SetDailyGoals()
        {
            var goals = _service.GetDailyGoals();
            DailyCaloriesGoal = goals.Calories;
            DailyProteinsGoal = goals.Proteins;
            DailyCarbsGoal = goals.Carbs;
            DailyFatsGoal = goals.Fats;
        }
        public void UpdateDailyGoals(double cal, double pro, double carb, double fat)
        {
            _service.UpdateDailyGoals(cal, pro, carb, fat);
        }
        public void GetTodaysMacros(string today)
        {
           var todaysMacros = _service.GetTodaysMacros(today);
            Date = todaysMacros.Date;
            Calories = todaysMacros.Calories;
            Proteins = todaysMacros.Proteins;
            Carbs = todaysMacros.Carbs;
            Fats = todaysMacros.Fats;
        }
    }
}
