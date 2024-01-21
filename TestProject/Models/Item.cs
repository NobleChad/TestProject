using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TestProject.Models
{
    public class Item
    {
		[Key]
		[SwaggerSchema(ReadOnly = true)]
		public int ID { get; set; }
		[Required(ErrorMessage = "*Required Field")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Required Field")]
        public int Price { get; set; }
        [Required(ErrorMessage = "*Required Field")]
        public int Amount { get; set; }

    }
}
