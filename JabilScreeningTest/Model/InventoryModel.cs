using System.ComponentModel.DataAnnotations;

namespace JabilScreeningTest.Model
{
    public class InventoryModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }
        public string CreatedDate { get; set; }
    }
}
