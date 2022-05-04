using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCalculator.Core.DataBase.Models
{
    public class ProductDto : MacrosDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
