// BackEnd/Controllers/SuggestionsController.cs
using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace BackEnd.Controllers
{
    // DTO para criar uma SUGESTÃO (vindo do usuário)
    public class CreateSuggestionDto
    {
        [Required]
        public string Barcode { get; set; } = string.Empty;
        [Required]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        public string Material { get; set; } = string.Empty; // Material selecionado no ComboBox
        // Não há mais campos de dicas aqui
    }

    // DTO para APROVAR uma sugestão (enviado pelo "admin" após revisão)
    // Este DTO ainda pode ser útil se o admin quiser editar o nome/material antes de aprovar.
    // As dicas agora virão do MaterialRecyclingGuide.
    public class ApproveSuggestionDto
    {
        [Required]
        public string Barcode { get; set; } = string.Empty;
        [Required]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        public string Material { get; set; } = string.Empty; // Material confirmado/editado pelo admin
    }


    [ApiController]
    [Route("api/[controller]")]
    public class SuggestionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuggestionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSuggestion([FromBody] CreateSuggestionDto suggestionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newSuggestion = new Suggestion
            {
                Barcode = suggestionDto.Barcode,
                ProductName = suggestionDto.ProductName,
                Material = suggestionDto.Material, // Material do ComboBox
                // Os campos de dicas não são mais definidos aqui como "Aguardando análise"
                // porque não existem mais no CreateSuggestionDto.
                // Se você manteve os campos no modelo Suggestion, eles serão nulos por padrão
                // ou você pode atribuir "Aguardando análise" diretamente aqui no backend.
                // Vamos assumir que o modelo Suggestion foi simplificado.
                DateSubmitted = DateTime.UtcNow,
                Status = "Pendente"
            };

            _context.Suggestions.Add(newSuggestion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Obrigado pela sua colaboração! Sua sugestão será analisada." });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Suggestion>>> GetAllSuggestions()
        {
            var suggestions = await _context.Suggestions
                                        .Where(s => s.Status == "Pendente")
                                        .OrderByDescending(s => s.DateSubmitted)
                                        .ToListAsync();
            return Ok(suggestions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Suggestion>> GetSuggestionById(int id)
        {
            var suggestion = await _context.Suggestions.FindAsync(id);
            if (suggestion == null)
            {
                return NotFound(new { message = $"Sugestão com ID {id} não encontrada." });
            }
            return Ok(suggestion);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveSuggestion(int id, [FromBody] ApproveSuggestionDto approvalData) // approvalData agora só tem barcode, productName, material
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var suggestion = await _context.Suggestions.FindAsync(id);
            if (suggestion == null)
            {
                return NotFound(new { message = $"Sugestão com ID {id} não encontrada." });
            }
            if (suggestion.Status != "Pendente")
            {
                return BadRequest(new { message = $"Sugestão com ID {id} não está mais pendente." });
            }

            // Usar o material do approvalData (que pode ter sido confirmado/editado pelo admin)
            // para buscar as informações de reciclagem.
            var materialGuide = await _context.MaterialRecyclingGuides
                                      .FirstOrDefaultAsync(g => g.MaterialNameKey == approvalData.Material); // Usa o Material do DTO de aprovação

            if (materialGuide == null)
            {
                return BadRequest(new { message = $"Guia de reciclagem não encontrado para o material '{approvalData.Material}'. Cadastre o guia primeiro." });
            }

            var existingProduct = await _context.Products.FindAsync(approvalData.Barcode);
            if (existingProduct != null)
            {
                existingProduct.ProductName = approvalData.ProductName;
                existingProduct.Material = approvalData.Material; // Material confirmado/editado
                existingProduct.DisposalTips = materialGuide.DisposalTips; // VEM DO GUIA
                existingProduct.RecyclingInfo = materialGuide.RecyclingInfo; // VEM DO GUIA
                existingProduct.SustainabilityImpact = materialGuide.SustainabilityImpact; // VEM DO GUIA
                existingProduct.LastUpdated = DateTime.UtcNow;
                _context.Products.Update(existingProduct);
            }
            else
            {
                var newProduct = new Product
                {
                    Barcode = approvalData.Barcode,
                    ProductName = approvalData.ProductName,
                    Material = approvalData.Material, // Material confirmado/editado
                    DisposalTips = materialGuide.DisposalTips, // VEM DO GUIA
                    RecyclingInfo = materialGuide.RecyclingInfo, // VEM DO GUIA
                    SustainabilityImpact = materialGuide.SustainabilityImpact, // VEM DO GUIA
                    DateAdded = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                };
                _context.Products.Add(newProduct);
            }

            suggestion.Status = "Aprovada";
            // Atualiza a sugestão com os dados que foram efetivamente usados para criar/atualizar o produto
            suggestion.ProductName = approvalData.ProductName;
            suggestion.Material = approvalData.Material;
            // Se você removeu os campos de dicas da entidade Suggestion, remova as linhas abaixo:
            // suggestion.DisposalTips = materialGuide.DisposalTips;
            // suggestion.RecyclingInfo = materialGuide.RecyclingInfo;
            // suggestion.SustainabilityImpact = materialGuide.SustainabilityImpact;

            _context.Suggestions.Update(suggestion);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = $"Sugestão ID {id} aprovada. Produto '{approvalData.ProductName}' ({approvalData.Barcode}) cadastrado/atualizado com informações de '{approvalData.Material}'." });
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DB Error on approving suggestion: {ex.InnerException?.Message ?? ex.Message}");
                return StatusCode(500, new { message = "Erro ao salvar as alterações no banco de dados." });
            }
        }

        [HttpPost("{id}/decline")]
        public async Task<IActionResult> DeclineSuggestion(int id)
        {
            // ... (lógica de decline permanece a mesma)
            var suggestion = await _context.Suggestions.FindAsync(id);
            if (suggestion == null)
            {
                return NotFound(new { message = $"Sugestão com ID {id} não encontrada." });
            }
            if (suggestion.Status != "Pendente")
            {
                return BadRequest(new { message = $"Sugestão com ID {id} não está mais pendente (Status atual: {suggestion.Status})." });
            }
            suggestion.Status = "Rejeitada";
            _context.Suggestions.Update(suggestion);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Sugestão ID {id} rejeitada." });
        }
    }
}