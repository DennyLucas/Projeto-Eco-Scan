
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class MaterialRecyclingGuide
    {
        [Key]
        // Chave para busca: pode ser "en:plastic" da API ou "Plástico" do ComboBox do usuário
        public string MaterialNameKey { get; set; } = string.Empty;

        [Required]
        public string DisplayMaterialName { get; set; } = string.Empty; // Nome amigável para exibição

        public string? DisposalTips { get; set; }
        public string? RecyclingInfo { get; set; }
        public string? SustainabilityImpact { get; set; }
    }
}