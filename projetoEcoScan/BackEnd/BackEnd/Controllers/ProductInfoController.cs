// BackEnd/Controllers/ProductInfoController.cs
using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization; // Para JsonPropertyName
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic; // Para List

namespace BackEnd.Controllers
{
    // --- Estruturas para desserializar a resposta da API OpenFoodFacts ---
    public class OpenFoodFactsPackagingDetail
    {
        [JsonPropertyName("material")]
        public string? Material { get; set; } // Ex: "en:plastic"

        [JsonPropertyName("shape")]
        public string? Shape { get; set; } // Ex: "pt:Squeeze"
    }

    public class OpenFoodFactsProductDetail
    {
        [JsonPropertyName("product_name")]
        public string? ProductName { get; set; }

        [JsonPropertyName("product_name_pt")]
        public string? ProductNamePt { get; set; }

        [JsonPropertyName("packaging_tags")]
        public List<string>? PackagingTags { get; set; }

        [JsonPropertyName("packaging")]
        public string? Packaging { get; set; }

        [JsonPropertyName("packagings")]
        public List<OpenFoodFactsPackagingDetail>? Packagings { get; set; }
    }

    public class OpenFoodFactsApiResponse
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("status_verbose")]
        public string? StatusVerbose { get; set; }

        [JsonPropertyName("product")]
        public OpenFoodFactsProductDetail? Product { get; set; }
    }

    // --- DTO para a resposta do seu backend ao frontend ---
    public class EcoProductInfoResponse
    {
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public string? DisposalTips { get; set; }
        public string? RecyclingInfo { get; set; }
        public string? SustainabilityImpact { get; set; }
        public string DataSource { get; set; } = string.Empty;
        public bool SuggestionNeeded { get; set; } = false;
    }

    [ApiController]
    [Route("api/productinfo")]
    public class ProductInfoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductInfoController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<EcoProductInfoResponse>> GetProductInfoByBarcode([FromQuery] string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return BadRequest("Barcode cannot be empty.");
            }

            // 1. PRIORIDADE: Verificar na base de dados local (Products)
            var dbProduct = await _context.Products.FindAsync(barcode);
            if (dbProduct != null)
            {
                Console.WriteLine($"Product {barcode} found in local DB (Products table) first.");
                return Ok(new EcoProductInfoResponse
                {
                    Barcode = dbProduct.Barcode,
                    ProductName = dbProduct.ProductName,
                    Material = dbProduct.Material,
                    DisposalTips = dbProduct.DisposalTips,
                    RecyclingInfo = dbProduct.RecyclingInfo,
                    SustainabilityImpact = dbProduct.SustainabilityImpact,
                    DataSource = "DB_Product", // Indica que veio do nosso banco de produtos curados
                    SuggestionNeeded = false
                });
            }

            // 2. Se não encontrou no DB local, tentar API OpenFoodFacts
            Console.WriteLine($"Product {barcode} not found in local DB. Trying OpenFoodFacts API...");
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("EcoScanApp/1.0 (seu.email@example.com)"); // Substitua pelo seu email ou URL do projeto
            var apiUrl = $"https://world.openfoodfacts.org/api/v2/product/{barcode}.json?fields=code,product_name,product_name_pt,packaging_tags,packaging,packagings,status_verbose";

            OpenFoodFactsApiResponse? apiResponse = null;
            try
            {
                var httpResponse = await httpClient.GetAsync(apiUrl);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                    apiResponse = JsonSerializer.Deserialize<OpenFoodFactsApiResponse>(jsonResponse);
                }
                else
                {
                    Console.WriteLine($"OpenFoodFacts API error for barcode {barcode}: {httpResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling OpenFoodFacts API for barcode {barcode}: {ex.Message}");
            }

            if (apiResponse != null && apiResponse.StatusVerbose == "product found" && apiResponse.Product != null)
            {
                Console.WriteLine($"Product {barcode} found in OpenFoodFacts API.");
                var productName = !string.IsNullOrWhiteSpace(apiResponse.Product.ProductNamePt) ? apiResponse.Product.ProductNamePt : apiResponse.Product.ProductName;
                productName ??= "Nome não fornecido pela API";

                string identifiedMaterialKey = "unknown";
                string displayMaterial = "Desconhecido (API)";

                if (apiResponse.Product.Packagings != null && apiResponse.Product.Packagings.Any())
                {
                    var firstPackaging = apiResponse.Product.Packagings.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Material));
                    if (firstPackaging != null)
                    {
                        identifiedMaterialKey = firstPackaging.Material!;
                        displayMaterial = identifiedMaterialKey.Contains(':') ? identifiedMaterialKey.Split(':')[1] : identifiedMaterialKey;
                        if(!string.IsNullOrEmpty(displayMaterial))
                            displayMaterial = char.ToUpper(displayMaterial[0]) + displayMaterial.Substring(1);

                        if (!string.IsNullOrWhiteSpace(firstPackaging.Shape))
                        {
                            var shapeClean = firstPackaging.Shape.Contains(':') ? firstPackaging.Shape.Split(':')[1] : firstPackaging.Shape;
                             if(!string.IsNullOrEmpty(shapeClean))
                                shapeClean = char.ToUpper(shapeClean[0]) + shapeClean.Substring(1);
                            displayMaterial += $" ({shapeClean})";
                        }
                    }
                }
                else if (apiResponse.Product.PackagingTags != null && apiResponse.Product.PackagingTags.Any())
                {
                    identifiedMaterialKey = apiResponse.Product.PackagingTags
                        .FirstOrDefault(tag => tag.StartsWith("en:") || tag.StartsWith("pt:")) ?? apiResponse.Product.PackagingTags.First();
                    displayMaterial = identifiedMaterialKey.Contains(':') ? identifiedMaterialKey.Split(':')[1] : identifiedMaterialKey;
                     if(!string.IsNullOrEmpty(displayMaterial))
                        displayMaterial = char.ToUpper(displayMaterial[0]) + displayMaterial.Substring(1);
                }
                else if (!string.IsNullOrWhiteSpace(apiResponse.Product.Packaging))
                {
                    displayMaterial = apiResponse.Product.Packaging;
                    // Mapeamento simples de texto livre para identifiedMaterialKey
                    if (displayMaterial.ToLower().Contains("plástico") || displayMaterial.ToLower().Contains("plastic")) identifiedMaterialKey = "en:plastic";
                    else if (displayMaterial.ToLower().Contains("vidro") || displayMaterial.ToLower().Contains("glass")) identifiedMaterialKey = "en:glass";
                    else if (displayMaterial.ToLower().Contains("alumínio") || displayMaterial.ToLower().Contains("aluminium")) identifiedMaterialKey = "en:aluminium";
                    else if (displayMaterial.ToLower().Contains("papelão") || displayMaterial.ToLower().Contains("cardboard")) identifiedMaterialKey = "en:cardboard";
                    else if (displayMaterial.ToLower().Contains("papel") || displayMaterial.ToLower().Contains("paper")) identifiedMaterialKey = "en:paper";
                    // Adicionar mais mapeamentos conforme necessário
                }

                var materialGuide = await _context.MaterialRecyclingGuides
                                        .FirstOrDefaultAsync(g => g.MaterialNameKey.ToLower() == identifiedMaterialKey.ToLower());

                if (materialGuide != null)
                {
                    // Produto encontrado na API E temos um guia para o material
                    Console.WriteLine($"Product {barcode} (API) matched with local MaterialRecyclingGuide for '{identifiedMaterialKey}'.");
                    return Ok(new EcoProductInfoResponse
                    {
                        Barcode = apiResponse.Code ?? barcode,
                        ProductName = productName,
                        Material = materialGuide.DisplayMaterialName,
                        DisposalTips = materialGuide.DisposalTips,
                        RecyclingInfo = materialGuide.RecyclingInfo,
                        SustainabilityImpact = materialGuide.SustainabilityImpact,
                        DataSource = "API_DB_Guide",
                        SuggestionNeeded = false
                    });
                }
                else
                {
                    // Produto encontrado na API, mas não temos um guia para o material que ela retornou.
                    // Como já verificamos a tabela Products no início e não estava lá,
                    // precisamos de uma sugestão para que o usuário possa nos dar o material correto (do Picker)
                    // e o sistema (via aprovação) possa então associar a um guia ou criar um novo produto.
                    Console.WriteLine($"Product {barcode} (API) found, but no local guide for material '{identifiedMaterialKey}'. Suggestion needed.");
                    return Ok(new EcoProductInfoResponse
                    {
                        Barcode = apiResponse.Code ?? barcode,
                        ProductName = productName, // Nome da API
                        Material = displayMaterial, // Material da API (o usuário poderá corrigir/selecionar no formulário)
                        DisposalTips = "Guia de descarte não encontrado para este material.",
                        RecyclingInfo = "Guia de reciclagem não encontrado para este material.",
                        SustainabilityImpact = "Guia de sustentabilidade não encontrado para este material.",
                        DataSource = "API_No_Guide_For_Material", // Indica que a API achou, mas nós não temos o guia
                        SuggestionNeeded = true
                    });
                }
            }

            // Se chegou aqui, o produto não foi encontrado na nossa tabela Products (verificado no início)
            // E também não foi encontrado na API OpenFoodFacts (ou a API falhou).
            Console.WriteLine($"Product {barcode} not found in local DB or API. Suggestion needed.");
            return Ok(new EcoProductInfoResponse
            {
                Barcode = barcode,
                ProductName = (apiResponse?.Product?.ProductNamePt ?? apiResponse?.Product?.ProductName) ?? "Produto não encontrado", // Tenta usar nome da API se houver, mesmo que "product not found"
                Material = "Material não identificado",
                DisposalTips = null, // Deixa nulo para o formulário de sugestão
                RecyclingInfo = null,
                SustainabilityImpact = null,
                DataSource = "Not_Found_Everywhere",
                SuggestionNeeded = true
            });
        }
    }
}