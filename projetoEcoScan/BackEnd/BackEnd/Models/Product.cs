using System;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class Product
    {
        [Key]
        public string Barcode { get; set; } = string.Empty;

        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public string Material { get; set; } = string.Empty; // Material principal amigável (ex: "Plástico", "Vidro")

        public string? DisposalTips { get; set; }
        public string? RecyclingInfo { get; set; }
        public string? SustainabilityImpact { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}