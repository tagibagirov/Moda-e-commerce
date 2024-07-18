using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ModaECommerce.Models.ViewModel
{
    public class BaglamaDetails
    {
        [Required(ErrorMessage = "The Size field is required.")]
        public int SizeId { get; set; }
        [Required(ErrorMessage = "The Shipping field is required.")]
        public string ShippingType { get; set; }
        [Required]
        public float TotalPrice { get; set; }
    }
}



