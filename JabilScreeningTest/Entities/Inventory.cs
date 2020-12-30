using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JabilScreeningTest.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatesDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
