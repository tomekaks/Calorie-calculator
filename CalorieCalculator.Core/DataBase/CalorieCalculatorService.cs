using CalorieCalculator.Core.DataBase.Models;
using CalorieCalculator.Core.DataBase.Factories;
using System.Collections.Generic;

namespace CalorieCalculator.Core.DataBase
{
    public class CalorieCalculatorService
    {
        private readonly CalculatorFactory calculatorFactory;
        private readonly SettingsService settingsService;
        private readonly IDbManager dbManager;
        private bool newDay = true;

        public CalorieCalculatorService(IDbManager db)
        {
            dbManager = db;
            calculatorFactory = new CalculatorFactory();
            settingsService = new SettingsService(dbManager);
            
        }

        public List<ProductDto> GetProductList()
        {
            List<Product> productList = dbManager.ReadProductListFromDb();
            return calculatorFactory.CreateProductDtoList(productList);
        }
        public ProductDto CreateNewProduct(string name, double cal, double pro, double carb, double fat)
        {
            return calculatorFactory.CreateProductDto(name, cal, pro, carb, fat);
        }
        public void AddAProduct(string name, double cal, double pro, double carb, double fat)
        {
            ProductDto productDto = calculatorFactory.CreateProductDto(name, cal, pro, carb, fat);
            Product product = calculatorFactory.CreateProduct(productDto);

            dbManager.AddProductToDb(product);
        }
        public void AddAProduct(ProductDto productDto)
        {
            Product product = calculatorFactory.CreateProduct(productDto);

            dbManager.AddProductToDb(product);
        }
        public void DeleteAProduct(ProductDto productDto)
        {
            Product product = calculatorFactory.CreateProduct(productDto);
            dbManager.DeleteProductFromDb(product);
        }
        public DailySumDto GetTodaysMacros(string todaysDate)
        {
            var todaysSum = dbManager.ReadTodaysMacrosFromDb(todaysDate);
            if (todaysSum == null)
            {
                newDay = true;
                return calculatorFactory.CreateTodaysMacrosDto(todaysDate);
            }
            newDay = false;
            return calculatorFactory.CreateTodaysMacrosDto(todaysSum);
        }
        public void BeginNewOrUpdateTodaysMacros(DailySumDto todaysMacros)
        {
            if (newDay == true)
            {
                BeginNewDay(todaysMacros);
                newDay = false;
            }
            else
                UpdateTodaysMacros(todaysMacros);
        }
        public void UpdateTodaysMacros(DailySumDto todaysMacros)
        {
            DailySum today = calculatorFactory.CreateTodaysMacros(todaysMacros);
            dbManager.UpdateTodaysMacros(today);
        }
        public void BeginNewDay(DailySumDto todaysMacros)
        {
            DailySum today = calculatorFactory.CreateTodaysMacros(todaysMacros);
            dbManager.InsertNewDayToDb(today);
        }
        public DailySumDto CreateNewDailySumDto(string date, double cal, double pro, double carb, double fat)
        {
            return calculatorFactory.CreateTodaysMacrosDto(date, cal, pro, carb, fat);
        }
        public void UpdateDailyGoals(double cal, double pro, double carb, double fat)
        {
            settingsService.UpdateDailyGoals(cal, pro, carb, fat);
        }
        public MacrosDto GetDailyGoals()
        {
            var goalsDto = settingsService.GetDailyGoals();
            return goalsDto;
        }
    }
}
