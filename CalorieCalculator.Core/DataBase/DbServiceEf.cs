using CalorieCalculator.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCalculator.Core.DataBase
{
    public class DbServiceEf : IDbManager
    {
        private CalculatorDbContext context = new CalculatorDbContext();

        public void AddProductToDb(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
        }
        public void DeleteProductFromDb(Product product)
        {
            var delete = context.Products.Find(product.Id);
            context.Products.Remove(delete);
            context.SaveChanges();
        }
        public List<Product> ReadProductListFromDb()
        {
            return context.Products.ToList();
        }
        public Setting GetSettingFromDb(string key)
        {
            return context.Settings.SingleOrDefault(s => s.Key == key);
        }
        public void UpdateSettingInDb(string key, string value)
        {
            var setting = context.Settings.SingleOrDefault(s => s.Key == key);
            setting.Value = value;
            context.SaveChanges();
        }
        public DailySum ReadTodaysMacrosFromDb(string today)
        {
            var result = context.DailySums.SingleOrDefault(d => d.Date == today);
            return result;
        }
        public void InsertNewDayToDb(DailySum newDay)
        {
            context.DailySums.Add(newDay);
            context.SaveChanges();
        }
        public void UpdateTodaysMacros(DailySum today)
        {
            var todaysMacros = context.DailySums.SingleOrDefault(d => d.Date == today.Date);
            todaysMacros.Calories = today.Calories;
            todaysMacros.Proteins = today.Proteins;
            todaysMacros.Carbs = today.Carbs;
            todaysMacros.Fats = today.Fats;
            context.SaveChanges();
        }

    }
}
