using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BackEnd.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<MaterialRecyclingGuide> MaterialRecyclingGuides { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Barcode);
            });

            modelBuilder.Entity<MaterialRecyclingGuide>(entity =>
            {
                entity.HasKey(e => e.MaterialNameKey);
            });

            modelBuilder.Entity<Suggestion>(entity =>
            {
                entity.HasIndex(e => e.Barcode);
            });

            modelBuilder.Entity<MaterialRecyclingGuide>().HasData(
                new MaterialRecyclingGuide {
                    MaterialNameKey = "Plástico", // Chave IGUAL ao que será exibido/enviado pelo ComboBox
                    DisplayMaterialName = "Plástico",
                    DisposalTips = "Lave, seque e, se possível, amasse embalagens plásticas. Verifique o símbolo de reciclabilidade (ex: PET, PP).",
                    RecyclingInfo = "Muitos plásticos são recicláveis. Evite misturar com orgânicos. Isopor e embalagens metalizadas geralmente não são.",
                    SustainabilityImpact = "Reciclar plástico economiza petróleo e energia, reduzindo o lixo em aterros."
                },
                new MaterialRecyclingGuide {
                    MaterialNameKey = "Vidro",
                    DisplayMaterialName = "Vidro",
                    DisposalTips = "Lave potes e garrafas. Deposite em coletores verdes. Embale vidros quebrados com segurança e sinalize.",
                    RecyclingInfo = "Vidro é 100% reciclável. Não misture com cerâmica, espelhos ou lâmpadas.",
                    SustainabilityImpact = "Economia infinita de matéria-prima e energia na reciclagem."
                },
                new MaterialRecyclingGuide {
                    MaterialNameKey = "Papel",
                    DisplayMaterialName = "Papel",
                    DisposalTips = "Mantenha seco e limpo. Não amasse em bola. Remova clipes grandes.",
                    RecyclingInfo = "Jornais, revistas, folhas são recicláveis. Papel carbono, fotográfico ou plastificado não são.",
                    SustainabilityImpact = "Salva árvores, água e energia."
                },
                new MaterialRecyclingGuide {
                    MaterialNameKey = "Papelão",
                    DisplayMaterialName = "Papelão",
                    DisposalTips = "Desmonte caixas. Mantenha seco e limpo. Embalagens longa vida podem ter coleta específica.",
                    RecyclingInfo = "Papelão limpo é reciclável. Evite o que estiver muito engordurado.",
                    SustainabilityImpact = "Conserva recursos e reduz o volume de lixo."
                },
                new MaterialRecyclingGuide {
                    MaterialNameKey = "Metal", // Genérico para aço, ferro, etc.
                    DisplayMaterialName = "Metal (Aço/Ferro)",
                    DisposalTips = "Latas de alimentos (aço), tampas. Lave e amasse, se possível.",
                    RecyclingInfo = "Metais ferrosos são recicláveis. Não misture com pilhas ou latas de tinta/químicos.",
                    SustainabilityImpact = "Reduz a extração de minério e o consumo de energia."
                },
                new MaterialRecyclingGuide {
                    MaterialNameKey = "Alumínio",
                    DisplayMaterialName = "Alumínio",
                    DisposalTips = "Latas de bebidas, papel alumínio limpo. Lave e amasse.",
                    RecyclingInfo = "Altamente reciclável, economizando até 95% de energia.",
                    SustainabilityImpact = "Diminui a necessidade de mineração de bauxita."
                },
                new MaterialRecyclingGuide {
                    MaterialNameKey = "Borracha",
                    DisplayMaterialName = "Borracha",
                    DisposalTips = "Pneus velhos geralmente têm pontos de coleta específicos ou ecopontos. Outros itens de borracha, verifique localmente.",
                    RecyclingInfo = "A reciclagem de borracha é complexa, mas possível para certos tipos, transformando-a em outros produtos.",
                    SustainabilityImpact = "Evita o descarte inadequado e pode gerar novos materiais."
                },
                 new MaterialRecyclingGuide {
                    MaterialNameKey = "Orgânico",
                    DisplayMaterialName = "Resíduo Orgânico",
                    DisposalTips = "Restos de frutas, verduras, legumes, cascas de ovos, borra de café. Pode ser usado em compostagem doméstica.",
                    RecyclingInfo = "Não misture com lixo reciclável ou rejeitos. Evite carnes, laticínios e gorduras na compostagem caseira (a menos que tenha um sistema adequado).",
                    SustainabilityImpact = "Reduz o metano em aterros e produz adubo rico para o solo."
                },
                new MaterialRecyclingGuide {
                    MaterialNameKey = "Eletrônico",
                    DisplayMaterialName = "Lixo Eletrônico",
                    DisposalTips = "Celulares, computadores, pilhas, baterias, TVs. NUNCA descarte no lixo comum ou reciclável.",
                    RecyclingInfo = "Procure pontos de coleta específicos para lixo eletrônico (lojas, fabricantes, ecopontos municipais). Contêm metais pesados e substâncias tóxicas.",
                    SustainabilityImpact = "O descarte correto evita contaminação do solo/água e permite a recuperação de materiais valiosos."
                },
                new MaterialRecyclingGuide
                {
                    MaterialNameKey = "en:plastic",
                    DisplayMaterialName = "Plástico (API)",
                    DisposalTips = "Lave, seque e, se possível, amasse embalagens como garrafas PET, embalagens de produtos de limpeza/higiene e potes de alimentos. Verifique o símbolo de reciclabilidade.",
                    RecyclingInfo = "Geralmente não são recicláveis (verifique na sua cidade): isopor, embalagens metalizadas (ex: salgadinhos), adesivos e plásticos termorrígidos. Estes devem ir para o lixo comum para não contaminar o processo.",
                    SustainabilityImpact = "Reciclar plástico economiza recursos naturais (petróleo), energia e reduz o volume de lixo nos aterros."
                },
                 new MaterialRecyclingGuide
                {
                    MaterialNameKey = "en:aluminium",
                    DisplayMaterialName = "Alumínio (API)",
                    DisposalTips = "Latas de alumínio e aço (bebidas, alimentos), tampas metálicas e papel alumínio devem estar limpos. Latas preferencialmente amassadas.",
                    RecyclingInfo = "Não devem ser misturados aos recicláveis comuns: esponjas de aço, latas de tinta ou produtos químicos. Pilhas e baterias exigem descarte especial em pontos de coleta específicos.",
                    SustainabilityImpact = "Reciclar metais, especialmente alumínio, economiza uma quantidade significativa de energia e reduz a necessidade de mineração."
                },
                new MaterialRecyclingGuide
                {
                    MaterialNameKey = "en:glass",
                    DisplayMaterialName = "Vidro (API)",
                    DisposalTips = "Garrafas, potes e frascos devem ser descartados limpos. Cacos de vidro precisam ser cuidadosamente embalados em jornal ou caixa de papelão resistente e devidamente sinalizados com 'CUIDADO: VIDRO QUEBRADO'.",
                    RecyclingInfo = "Não são recicláveis: espelhos, vidros temperados (Pyrex, box de banheiro), lâmpadas (requerem descarte especial), cerâmicas ou porcelanas. Estes devem ir para o lixo comum ou pontos de coleta específicos.",
                    SustainabilityImpact = "O vidro é 100% reciclável e pode ser reciclado infinitas vezes, economizando energia e matéria-prima."
                },
                new MaterialRecyclingGuide
                {
                    MaterialNameKey = "en:paper",
                    DisplayMaterialName = "Papel (API)",
                    DisposalTips = "Descarte jornais, revistas, folhas de caderno e caixas de papel limpas e secas. Evite amassar em bola. Remova contaminantes como clipes grandes ou fitas adesivas.",
                    RecyclingInfo = "Não são recicláveis: papéis higiênicos usados, papel carbono, fotografias, papéis plastificados ou aqueles muito sujos com gordura ou restos de alimentos (estes vão para o lixo comum).",
                    SustainabilityImpact = "Reciclar papel salva árvores, reduz o consumo de água e energia, e diminui a quantidade de resíduos em aterros."
                },
                new MaterialRecyclingGuide
                {
                    MaterialNameKey = "en:cardboard",
                    DisplayMaterialName = "Papelão (API)",
                    DisposalTips = "Desmonte caixas para reduzir volume. Certifique-se de que estejam limpas, livres de gordura ou umidade excessiva. Embalagens longa vida (Tetra Pak) podem ser recicladas se sua cidade tiver coleta específica.",
                    RecyclingInfo = "Não são recicláveis: Papelão muito engordurado (como o fundo de caixas de pizza sujas) ou molhado (estes vão para o lixo comum).",
                    SustainabilityImpact = "A reciclagem de papelão conserva recursos naturais, economiza energia e reduz o lixo em aterros."
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Barcode = "7891000315507",
                    ProductName = "Refrigerante em Lata",
                    Material = "Alumínio",
                    DisposalTips = "Lave e amasse a lata. Deposite no cesto amarelo.",
                    RecyclingInfo = "O alumínio é 100% reciclável. Reciclar economiza 95% da energia.",
                    SustainabilityImpact = "Reduz a extração de bauxita e o consumo de energia.",
                    DateAdded = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                },
                new Product
                {
                    Barcode = "7891991010769",
                    ProductName = "Água Mineral Garrafa PET",
                    Material = "Plástico PET",
                    DisposalTips = "Esvazie, lave e amasse a garrafa. Separe a tampa. Deposite no cesto vermelho.",
                    RecyclingInfo = "PET é reciclável. A tampa (PP/PEAD) também. O rótulo pode precisar ser removido.",
                    SustainabilityImpact = "Reduz extração de petróleo e volume em aterros.",
                    DateAdded = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                },
                new Product
                {
                    Barcode = "7896005800057",
                    ProductName = "Leite Longa Vida Caixa",
                    Material = "Embalagem Cartonada (Papelão, Plástico, Alumínio)",
                    DisposalTips = "Esvazie e amasse levemente. Algumas cidades coletam separadamente.",
                    RecyclingInfo = "A reciclagem de embalagens longa vida é possível, mas depende da infraestrutura local.",
                    SustainabilityImpact = "Quando reciclada, contribui para a economia de recursos.",
                    DateAdded = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                },
                new Product
                {
                    Barcode = "7891149101017",
                    ProductName = "Cerveja Garrafa de Vidro",
                    Material = "Vidro",
                    DisposalTips = "Lave a garrafa. Deposite no cesto verde. Cuidado com cacos.",
                    RecyclingInfo = "Vidro é 100% reciclável.",
                    SustainabilityImpact = "Economia de energia e matéria-prima.",
                    DateAdded = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                },
                new Product
                {
                    Barcode = "1234567890000",
                    ProductName = "Caderno Espiral",
                    Material = "Papel e Metal (espiral)",
                    DisposalTips = "Remova a espiral metálica se possível (descarte com metais). As folhas de papel limpas vão para o cesto azul.",
                    RecyclingInfo = "Folhas de papel são recicláveis. A espiral metálica também.",
                    SustainabilityImpact = "Contribui para a preservação de árvores.",
                    DateAdded = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                }
            );
        }
    }
}