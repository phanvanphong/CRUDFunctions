using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDotNet5.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string NameImage { get; set; }
        public IFormFile Image { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }

    }
}
