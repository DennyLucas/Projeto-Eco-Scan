using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialRecyclingGuides",
                columns: table => new
                {
                    MaterialNameKey = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayMaterialName = table.Column<string>(type: "TEXT", nullable: false),
                    DisposalTips = table.Column<string>(type: "TEXT", nullable: true),
                    RecyclingInfo = table.Column<string>(type: "TEXT", nullable: true),
                    SustainabilityImpact = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialRecyclingGuides", x => x.MaterialNameKey);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Barcode = table.Column<string>(type: "TEXT", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    Material = table.Column<string>(type: "TEXT", nullable: false),
                    DisposalTips = table.Column<string>(type: "TEXT", nullable: true),
                    RecyclingInfo = table.Column<string>(type: "TEXT", nullable: true),
                    SustainabilityImpact = table.Column<string>(type: "TEXT", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Barcode);
                });

            migrationBuilder.CreateTable(
                name: "Suggestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Barcode = table.Column<string>(type: "TEXT", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    Material = table.Column<string>(type: "TEXT", nullable: false),
                    DisposalTips = table.Column<string>(type: "TEXT", nullable: false),
                    RecyclingInfo = table.Column<string>(type: "TEXT", nullable: false),
                    SustainabilityImpact = table.Column<string>(type: "TEXT", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MaterialRecyclingGuides",
                columns: new[] { "MaterialNameKey", "DisplayMaterialName", "DisposalTips", "RecyclingInfo", "SustainabilityImpact" },
                values: new object[,]
                {
                    { "en:aluminium", "Alumínio (API)", "Latas de alumínio e aço (bebidas, alimentos), tampas metálicas e papel alumínio devem estar limpos. Latas preferencialmente amassadas.", "Não devem ser misturados aos recicláveis comuns: esponjas de aço, latas de tinta ou produtos químicos. Pilhas e baterias exigem descarte especial em pontos de coleta específicos.", "Reciclar metais, especialmente alumínio, economiza uma quantidade significativa de energia e reduz a necessidade de mineração." },
                    { "en:cardboard", "Papelão (API)", "Desmonte caixas para reduzir volume. Certifique-se de que estejam limpas, livres de gordura ou umidade excessiva. Embalagens longa vida (Tetra Pak) podem ser recicladas se sua cidade tiver coleta específica.", "Não são recicláveis: Papelão muito engordurado (como o fundo de caixas de pizza sujas) ou molhado (estes vão para o lixo comum).", "A reciclagem de papelão conserva recursos naturais, economiza energia e reduz o lixo em aterros." },
                    { "en:glass", "Vidro (API)", "Garrafas, potes e frascos devem ser descartados limpos. Cacos de vidro precisam ser cuidadosamente embalados em jornal ou caixa de papelão resistente e devidamente sinalizados com 'CUIDADO: VIDRO QUEBRADO'.", "Não são recicláveis: espelhos, vidros temperados (Pyrex, box de banheiro), lâmpadas (requerem descarte especial), cerâmicas ou porcelanas. Estes devem ir para o lixo comum ou pontos de coleta específicos.", "O vidro é 100% reciclável e pode ser reciclado infinitas vezes, economizando energia e matéria-prima." },
                    { "en:paper", "Papel (API)", "Descarte jornais, revistas, folhas de caderno e caixas de papel limpas e secas. Evite amassar em bola. Remova contaminantes como clipes grandes ou fitas adesivas.", "Não são recicláveis: papéis higiênicos usados, papel carbono, fotografias, papéis plastificados ou aqueles muito sujos com gordura ou restos de alimentos (estes vão para o lixo comum).", "Reciclar papel salva árvores, reduz o consumo de água e energia, e diminui a quantidade de resíduos em aterros." },
                    { "en:plastic", "Plástico (API)", "Lave, seque e, se possível, amasse embalagens como garrafas PET, embalagens de produtos de limpeza/higiene e potes de alimentos. Verifique o símbolo de reciclabilidade.", "Geralmente não são recicláveis (verifique na sua cidade): isopor, embalagens metalizadas (ex: salgadinhos), adesivos e plásticos termorrígidos. Estes devem ir para o lixo comum para não contaminar o processo.", "Reciclar plástico economiza recursos naturais (petróleo), energia e reduz o volume de lixo nos aterros." },
                    { "metal", "Metais", "Latas de alumínio e aço (bebidas, alimentos), tampas metálicas e papel alumínio devem estar limpos. Latas preferencialmente amassadas.", "Não devem ser misturados aos recicláveis comuns: esponjas de aço, latas de tinta ou produtos químicos. Pilhas e baterias exigem descarte especial em pontos de coleta específicos.", "Reciclar metais, especialmente alumínio, economiza uma quantidade significativa de energia e reduz a necessidade de mineração." },
                    { "papel", "Papel", "Descarte jornais, revistas, folhas de caderno e caixas de papel limpas e secas. Evite amassar em bola. Remova contaminantes como clipes grandes ou fitas adesivas.", "Não são recicláveis: papéis higiênicos usados, papel carbono, fotografias, papéis plastificados ou aqueles muito sujos com gordura ou restos de alimentos (estes vão para o lixo comum).", "Reciclar papel salva árvores, reduz o consumo de água e energia, e diminui a quantidade de resíduos em aterros." },
                    { "papelao", "Papelão", "Desmonte caixas para reduzir volume. Certifique-se de que estejam limpas, livres de gordura ou umidade excessiva. Embalagens longa vida (Tetra Pak) podem ser recicladas se sua cidade tiver coleta específica.", "Não são recicláveis: Papelão muito engordurado (como o fundo de caixas de pizza sujas) ou molhado (estes vão para o lixo comum).", "A reciclagem de papelão conserva recursos naturais, economiza energia e reduz o lixo em aterros." },
                    { "plastico", "Plásticos", "Lave, seque e, se possível, amasse embalagens como garrafas PET, embalagens de produtos de limpeza/higiene e potes de alimentos. Verifique o símbolo de reciclabilidade.", "Geralmente não são recicláveis (verifique na sua cidade): isopor, embalagens metalizadas (ex: salgadinhos), adesivos e plásticos termorrígidos. Estes devem ir para o lixo comum para não contaminar o processo.", "Reciclar plástico economiza recursos naturais (petróleo), energia e reduz o volume de lixo nos aterros." },
                    { "vidro", "Vidro", "Garrafas, potes e frascos devem ser descartados limpos. Cacos de vidro precisam ser cuidadosamente embalados em jornal ou caixa de papelão resistente e devidamente sinalizados com 'CUIDADO: VIDRO QUEBRADO'.", "Não são recicláveis: espelhos, vidros temperados (Pyrex, box de banheiro), lâmpadas (requerem descarte especial), cerâmicas ou porcelanas. Estes devem ir para o lixo comum ou pontos de coleta específicos.", "O vidro é 100% reciclável e pode ser reciclado infinitas vezes, economizando energia e matéria-prima." }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Barcode", "DateAdded", "DisposalTips", "LastUpdated", "Material", "ProductName", "RecyclingInfo", "SustainabilityImpact" },
                values: new object[,]
                {
                    { "1234567890000", new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5309), "Remova a espiral metálica se possível (descarte com metais). As folhas de papel limpas vão para o cesto azul.", new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5310), "Papel e Metal (espiral)", "Caderno Espiral", "Folhas de papel são recicláveis. A espiral metálica também.", "Contribui para a preservação de árvores." },
                    { "7891000315507", new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5240), "Lave e amasse a lata. Deposite no cesto amarelo.", new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5285), "Alumínio", "Refrigerante em Lata", "O alumínio é 100% reciclável. Reciclar economiza 95% da energia.", "Reduz a extração de bauxita e o consumo de energia." },
                    { "7891149101017", new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5304), "Lave a garrafa. Deposite no cesto verde. Cuidado com cacos.", new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5305), "Vidro", "Cerveja Garrafa de Vidro", "Vidro é 100% reciclável.", "Economia de energia e matéria-prima." },
                    { "7891991010769", new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5291), "Esvazie, lave e amasse a garrafa. Separe a tampa. Deposite no cesto vermelho.", new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5293), "Plástico PET", "Água Mineral Garrafa PET", "PET é reciclável. A tampa (PP/PEAD) também. O rótulo pode precisar ser removido.", "Reduz extração de petróleo e volume em aterros." },
                    { "7896005800057", new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5298), "Esvazie e amasse levemente. Algumas cidades coletam separadamente.", new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5299), "Embalagem Cartonada (Papelão, Plástico, Alumínio)", "Leite Longa Vida Caixa", "A reciclagem de embalagens longa vida é possível, mas depende da infraestrutura local.", "Quando reciclada, contribui para a economia de recursos." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_Barcode",
                table: "Suggestions",
                column: "Barcode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialRecyclingGuides");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suggestions");
        }
    }
}
