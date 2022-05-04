using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCalculator.Core.DataBase.Models
{
    public interface IDbManager
    {
        void AddProductToDb(Product product);
        void DeleteProductFromDb(Product product);
        List<Product> ReadProductListFromDb();
        Setting GetSettingFromDb(string key);
        void UpdateSettingInDb(string key, string value);
        DailySum ReadTodaysMacrosFromDb(string today);
        void InsertNewDayToDb(DailySum newDay);
        void UpdateTodaysMacros(DailySum today);
    }
}
