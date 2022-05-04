using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CalorieCalculator.Core.DataBase.Models;
using CalorieCalculator.Core.DataBase;

namespace Calorie_calculator
{
    class Program
    { 
        static void Main()
        {
            try
            {
                DbManagementService db = new DbManagementService();
                CalorieCalculatorService service = new CalorieCalculatorService(db);
                var productList = service.GetProductList();
                string todaysDate = GetTodaysDate();
                Console.WriteLine(todaysDate);
                DailySumDto todaysMacros = service.GetTodaysMacros(todaysDate);
                bool endOfProgram = false;

                while (!endOfProgram)
                {

                    Menu();
                    switch (Console.ReadLine())
                    {
                        case "1":
                            {

                                string yesOrNo = "y";
                                while (yesOrNo == "y")
                                {
                                    ProductDto selectedProduct = FindProductOnList(productList);
                                    if (todaysMacros == null)
                                    {
                                        todaysMacros = new DailySumDto();
                                        todaysMacros.Date = todaysDate;
                                        todaysMacros = AddWhatYouAteToday(todaysMacros, selectedProduct);
                                        service.BeginNewDay(todaysMacros);
                                    }
                                    else
                                    {
                                        todaysMacros = AddWhatYouAteToday(todaysMacros, selectedProduct);
                                    }
                                    Console.WriteLine(todaysMacros.Date + ": calories " + todaysMacros.Calories + ", proteins " + todaysMacros.Proteins + "g, carbohydrates " + todaysMacros.Carbs +
                                   "g, fats " + todaysMacros.Fats + "g");
                                    Console.WriteLine("Add another product? y or n");
                                    yesOrNo = Console.ReadLine().ToLower();

                                }
                                service.UpdateTodaysMacros(todaysMacros);
                                break;
                            }
                        case "2":
                            {
                                ProductDto productDto = FindProductOnList(productList);
                                if (productDto != null)
                                {
                                    Console.WriteLine("This product already exists");
                                }
                                else
                                {
                                    service.AddAProduct(CreateProductDto());
                                }
                                break;
                            }
                        case "3":
                            {
                                ProductDto productDto = FindProductOnList(productList);
                                if (productDto == null)
                                {
                                    Console.WriteLine("Product does not exist");
                                }
                                else
                                {
                                    service.DeleteAProduct(productDto);
                                }
                                break;
                            }
                        case "4":
                            {
                                PrintProductList(productList);
                                break;
                            }

                        case "0":
                            {
                                endOfProgram = true;
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Choose a correct option");
                                break;
                            }

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }            
        }

        static void Menu()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Choose an option");
            Console.WriteLine("1) Calculator");
            Console.WriteLine("2) Add a product");
            Console.WriteLine("3) Delete a product");
            Console.WriteLine("4) View the list of products");
            Console.WriteLine("0) Quit");
            Console.WriteLine("-------------------------------");
        }
        static void PrintProductList(List<ProductDto> productList)
        {
            foreach (ProductDto item in productList)
            {
                Console.WriteLine("{0}: calories - {1}g, proteins - {2}g, carbohydrates - {3}g, fats - {4}g (for 100g of product)",
                                    item.Name,
                                    item.Calories,
                                    item.Proteins,
                                    item.Carbs,
                                    item.Fats);
            }
        }
        static ProductDto FindProductOnList(List<ProductDto> productList)
        {
            Console.WriteLine("Put in the name of the product");
            string usersChoice = Console.ReadLine();
            var chosenProduct = productList.SingleOrDefault(x => x.Name == usersChoice);
            if (chosenProduct != null)
            {
                Console.WriteLine(chosenProduct.Name + ": calories " + chosenProduct.Calories + ", proteins " + chosenProduct.Proteins + "g, carbohydrates " + chosenProduct.Carbs +
                    "g, fats " + chosenProduct.Fats + "g (for 100g of product)");
                return chosenProduct;
            }
            else
            {
                ProductNotFound(productList);
                return FindProductOnList(productList);
            }

        }
        public static double GetCalories()
        {
            double calories;
            if (double.TryParse(Console.ReadLine(), out calories))
            {
                return calories;
            }
            Console.WriteLine("Put in the correct data");
            return GetCalories();
        }
        public static double GetProteins()
        {
            double proteins;
            if (double.TryParse(Console.ReadLine(), out proteins))
            {
                return proteins;
            }
            Console.WriteLine("Put in the correct data");
            return GetProteins();
        }
        public static double GetCarbs()
        {
            double carbs;
            if (double.TryParse(Console.ReadLine(), out carbs))
            {
                return carbs;
            }
            Console.WriteLine("Put in the correct data");
            return GetCarbs();
        }
        public static double GetFats()
        {
            double fats;
            if (double.TryParse(Console.ReadLine(), out fats))
            {
                return fats;
            }
            Console.WriteLine("Put in the correct data");
            return GetFats();
        }
        static double GetWeight()
        {
            double weight;
            if (double.TryParse(Console.ReadLine(), out weight))
            {
                weight = weight / 100;
                return weight;
            }
            Console.WriteLine("Put in the correct data");
            return GetWeight();
        }
        static string GetTodaysDate()
        {
            DateTime now = DateTime.Today;
            string todaysDate = now.ToString("d");
            return todaysDate;
        }                
        static DailySumDto AddWhatYouAteToday(DailySumDto todaysMacros, ProductDto selectedProduct)
        {
            Console.WriteLine("How many grams?");
            double weight = GetWeight();
            todaysMacros.Calories += Math.Round(selectedProduct.Calories * weight, 1);
            todaysMacros.Proteins += Math.Round(selectedProduct.Proteins * weight, 1);
            todaysMacros.Carbs += Math.Round(selectedProduct.Carbs * weight, 1);
            todaysMacros.Fats += Math.Round(selectedProduct.Fats * weight, 1);
            return todaysMacros;
        }
        static void ProductNotFound(List<ProductDto> productList)
        {
            Console.WriteLine("Product does not exist, choose a different one");
            Console.WriteLine("If you want to view the list of products type \"y\"");
            Console.WriteLine("Otherwise type anything");
            string choice = Console.ReadLine().ToLower();
            if (choice == "y")
            {
                PrintProductList(productList);
            }
        }
        public static ProductDto CreateProductDto()
        {
            ProductDto productDto = new ProductDto();
            Console.WriteLine("Put in the name of the product");
            productDto.Name = Console.ReadLine();
            Console.WriteLine("Put in the amount of calories for 100 gram");
            productDto.Calories = GetCalories();
            Console.WriteLine("Put in the amount of proteins for 100 gram");
            productDto.Proteins = GetProteins();
            Console.WriteLine("Put in the amount of carbohydrates for 100 gram");
            productDto.Carbs = GetCarbs();
            Console.WriteLine("Put in the amount of fats for 100 gram");
            productDto.Fats = GetFats();
            return productDto;
        }
        [Obsolete]
        static void AddWhatYouAteTodayOld(Product selectedProduct, Product todaysMacros)
        {
            string name = GetTodaysDate();
            double calories = selectedProduct.Calories + todaysMacros.Calories;
            double proteins = selectedProduct.Proteins + todaysMacros.Proteins;
            double carbs = selectedProduct.Carbs + todaysMacros.Carbs;
            double fats = selectedProduct.Fats + todaysMacros.Fats;

            try
            {
                StreamWriter outputFile = File.CreateText("today.txt");
                outputFile.WriteLine($"{name};{calories};{proteins};{carbs};{fats}");
                outputFile.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            
            return;
        }
        [Obsolete] 
        static Product ReadTodaysMacros()
        {
            Product todaysMacros = new Product();
            try
            {
                if (File.Exists("today.txt"))
                {
                    StreamReader inputFile = File.OpenText("today.txt");
                    string data = inputFile.ReadLine();
                    char[] delim = { ';' };
                    string[] tokens = data.Split(delim);
                    todaysMacros.Name = tokens[0];
                    todaysMacros.Calories = int.Parse(tokens[1]);
                    todaysMacros.Proteins = int.Parse(tokens[2]);
                    todaysMacros.Carbs = int.Parse(tokens[3]);
                    todaysMacros.Fats = int.Parse(tokens[4]);
                    inputFile.Close();
                }
                else
                {
                    return todaysMacros;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return todaysMacros;
        }
        [Obsolete]
        static void CreateProduct()
        {
            string name = "";
            double calories = 0;
            double proteins = 0;
            double carbs = 0;
            double fats = 0;
            StreamWriter outputFile;

            Console.WriteLine("Podaj nazwę produktu");
            name = Console.ReadLine();

            Console.WriteLine("Podaj ilość kalorii na 100 gram");
            calories = GetCalories();
            Console.WriteLine("Podaj ilość białka na 100 gram");
            proteins = GetProteins();
            Console.WriteLine("Podaj ilość węglowodanów na 100 gram");
            carbs = GetCarbs();
            Console.WriteLine("Podaj ilość tłuszczy na 100 gram");
            fats = GetFats();
            Console.WriteLine(name + ": kalorie " + calories + ", białko " + proteins + "g, węglowodany " + carbs +
                                            "g, tłuszcze " + fats + "g (na 100g produktu)");

            try
            {
                outputFile = File.AppendText("dane.txt");
                outputFile.WriteLine($"{name};{calories};{proteins};{carbs};{fats}");
                outputFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }

            return;
        }
        [Obsolete]
        static List<Product> ReadList()
        {
            List<Product> productList = new List<Product>();

            try
            {
                StreamReader inputFile = File.OpenText("dane.txt");
                while (!inputFile.EndOfStream)
                {
                    string data = inputFile.ReadLine();
                    char[] delim = { ';' };
                    string[] tokens = data.Split(delim);

                    Product product = new Product();
                    product.Name = tokens[0];
                    product.Calories = double.Parse(tokens[1]);
                    product.Proteins = double.Parse(tokens[2]);
                    product.Carbs = double.Parse(tokens[3]);
                    product.Fats = double.Parse(tokens[4]);
                    productList.Add(product);

                }
                inputFile.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }

            return productList;
        }



    }   
}
