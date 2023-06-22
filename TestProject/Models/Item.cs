using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class Item
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "*Required Field")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Required Field")]
        public int Price { get; set; }
        [Required(ErrorMessage = "*Required Field")]
        public int Amount { get; set; }
        
    }
}
