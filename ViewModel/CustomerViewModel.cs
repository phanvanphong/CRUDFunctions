using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDotNet5.ViewModel
{
    public class CustomerViewModel
    {
     
        public int Id { get; set; }
     
        public string Username { get; set; }
  
        public string Password { get; set; }
    
        public string Fullname { get; set; }
      
        public string Address { get; set; }
      
        public string Email { get; set; }
        public string CreatedAt { get; set; }
        public string UserAdd { get; set; }
    }
}
