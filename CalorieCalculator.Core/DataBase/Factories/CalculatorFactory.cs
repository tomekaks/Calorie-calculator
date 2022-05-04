using CalorieCalculator.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace CalorieCalculator.Core.DataBase.Factories
{
    public class CalculatorFactory
    {
        public Product CreateProduct(DataRow row)
        {
            return new Product()
            {
                Id = int.Parse(row["Id"].ToString()),
                Name = row["Name"].ToString(),
                Calories = double.Parse(row["Calories"].ToString()),
                Proteins = double.Parse(row["Proteins"].ToString()),
                Carbs = double.Parse(row["Carbs"].ToString()),
                Fats = double.Parse(row["Fats"].ToString())

            };
        }
        public Product CreateProduct(ProductDto productDto)
        {
            return new Product()
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Calories = productDto.Calories,
                Proteins = productDto.Proteins,
                Carbs = productDto.Carbs,
                Fats = productDto.Fats,
            };
        }
        public ProductDto CreateProductDto(Product product)
        {
            return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Calories = product.Calories,
                Proteins = product.Proteins,
                Carbs = product.Carbs,
                Fats = product.Fats,
            };
        }
        public ProductDto CreateProductDto(string name, double cal, double pro, double carb, double fat)
        {
            return new ProductDto()
            {
                Name = name,
                Calories = cal,
                Proteins = pro,
                Carbs = carb,
                Fats = fat
            };
        }
        public List<ProductDto> CreateProductDtoList(List<Product> productList)
        {
            return productList.Select(p => CreateProductDto(p)).ToList();
        }

        public DailySum CreateTodaysMacros(DataRow row)
        {
            return new DailySum()
            {                
                Date = row["Date"].ToString(),
                Calories = double.Parse(row["Calories"].ToString()),
                Proteins = double.Parse(row["Proteins"].ToString()),
                Carbs = double.Parse(row["Carbs"].ToString()),
                Fats = double.Parse(row["Fats"].ToString())               
            };
        }
        public DailySum CreateTodaysMacros(DailySumDto today)
        {
            return new DailySum()
            {
                Date = today.Date,
                Calories = today.Calories,
                Proteins = today.Proteins,
                Carbs = today.Carbs,
                Fats = today.Fats
            };
        }
        public DailySumDto CreateTodaysMacrosDto(DailySum today)
        {
            return new DailySumDto()
            {
                Date = today.Date,
                Calories = today.Calories,
                Proteins = today.Proteins,
                Carbs = today.Carbs,
                Fats = today.Fats
            };
        }
        public DailySumDto CreateTodaysMacrosDto(string today)
        {
            return new DailySumDto()
            {
                Date = today,
                Calories = 0,
                Proteins = 0,
                Carbs = 0,
                Fats = 0
            };
        }
        public DailySumDto CreateTodaysMacrosDto(string date, double cal, double pro, double carb, double fat)
        {
            return new DailySumDto()
            {
                Date = date,
                Calories = cal,
                Proteins = pro,
                Carbs = carb,
                Fats = fat
            };
        }

        public Setting CreateSetting(DataRow row)
        {
            return new Setting()
            {
                Id = int.Parse(row["Id"].ToString()),
                Key = row["Key"].ToString(),
                Value = row["Value"].ToString()
            };
        }
        public MacrosDto CreateMacrosDto(Setting cal, Setting pro, Setting carb, Setting fat)
        {
            return new MacrosDto()
            {
                Calories = double.Parse(cal.Value),
                Proteins = double.Parse(pro.Value),
                Carbs = double.Parse(carb.Value),
                Fats = double.Parse(fat.Value),
            };
        }
 
    }
}
