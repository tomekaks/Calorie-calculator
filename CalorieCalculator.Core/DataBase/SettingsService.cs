using CalorieCalculator.Core.DataBase.Factories;
using CalorieCalculator.Core.DataBase.Models;

namespace CalorieCalculator.Core.DataBase
{
    public class SettingsService
    {

        private readonly IDbManager _dbManager;
        private readonly CalculatorFactory calculatorFactory;
        public readonly string calKey = "Calories";
        public readonly string protsKey = "Proteins";
        public readonly string carbsKey = "Carbs";
        public readonly string fatsKey = "Fats";

        public SettingsService(IDbManager dbManager)
        {
            _dbManager = dbManager;
            calculatorFactory = new CalculatorFactory();
        }

        public MacrosDto GetDailyGoals()
        {
            var calories = _dbManager.GetSettingFromDb(calKey);
            var proteins = _dbManager.GetSettingFromDb(protsKey);
            var carbs = _dbManager.GetSettingFromDb(carbsKey);
            var fats = _dbManager.GetSettingFromDb(fatsKey);
            return calculatorFactory.CreateMacrosDto(calories, proteins, carbs, fats);
            
        }
        public void UpdateDailyGoals(double cal, double pro, double carb, double fat)
        {
            _dbManager.UpdateSettingInDb(calKey, cal.ToString());
            _dbManager.UpdateSettingInDb(protsKey, pro.ToString());
            _dbManager.UpdateSettingInDb(carbsKey, carb.ToString());
            _dbManager.UpdateSettingInDb(fatsKey, fat.ToString());
        }
    }
}
