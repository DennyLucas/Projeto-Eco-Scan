// BackEnd/Models/Suggestion.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class Suggestion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Barcode { get; set; } = string.Empty;

        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public string Material { get; set; } = string.Empty; // Este virá do ComboBox

        // Estes campos não são mais preenchidos pelo usuário na sugestão inicial
        // public string DisposalTips { get; set; } = "Aguardando análise";
        // public string RecyclingInfo { get; set; } = "Aguardando análise";
        // public string SustainabilityImpact { get; set; } = "Aguardando análise";
        // Eles serão preenchidos no Product durante a aprovação.
        // Mantê-los na Suggestion pode ser útil para ver o que o "admin" preencheu ao aprovar,
        // mas para a submissão inicial do usuário, eles não são necessários.
        // Vou mantê-los para o caso da tela de revisão querer mostrá-los, mas eles
        // não virão do formulário de sugestão inicial do usuário.

        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pendente"; // Pendente, Aprovada, Rejeitada
    }
}