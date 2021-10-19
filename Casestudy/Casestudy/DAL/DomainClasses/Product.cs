using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Casestudy.DAL.DomainClasses
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }

        public string ProductName { get; set; }
        [Required]
        public string GraphicName { get; set; }
        [Required]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "money")]
        [Required]
        public decimal MSRP { get; set; }
        [Column(TypeName = "money")]
        [Required]
        public int QtyOnHand { get; set; }
        [Required]
        public int QtyOnBackOrder { get; set; }
        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] Timer { get; set; }
    }
}
