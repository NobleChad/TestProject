using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class Item
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        
    }
}
