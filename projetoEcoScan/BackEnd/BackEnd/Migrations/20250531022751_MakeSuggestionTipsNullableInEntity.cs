using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class MakeSuggestionTipsNullableInEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "metal");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "papel");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "papelao");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "plastico");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "vidro");

            migrationBuilder.DropColumn(
                name: "DisposalTips",
                table: "Suggestions");

            migrationBuilder.DropColumn(
                name: "RecyclingInfo",
                table: "Suggestions");

            migrationBuilder.DropColumn(
                name: "SustainabilityImpact",
                table: "Suggestions");

            migrationBuilder.InsertData(
                table: "MaterialRecyclingGuides",
                columns: new[] { "MaterialNameKey", "DisplayMaterialName", "DisposalTips", "RecyclingInfo", "SustainabilityImpact" },
                values: new object[,]
                {
                    { "Alumínio", "Alumínio", "Latas de bebidas, papel alumínio limpo. Lave e amasse.", "Altamente reciclável, economizando até 95% de energia.", "Diminui a necessidade de mineração de bauxita." },
                    { "Borracha", "Borracha", "Pneus velhos geralmente têm pontos de coleta específicos ou ecopontos. Outros itens de borracha, verifique localmente.", "A reciclagem de borracha é complexa, mas possível para certos tipos, transformando-a em outros produtos.", "Evita o descarte inadequado e pode gerar novos materiais." },
                    { "Eletrônico", "Lixo Eletrônico", "Celulares, computadores, pilhas, baterias, TVs. NUNCA descarte no lixo comum ou reciclável.", "Procure pontos de coleta específicos para lixo eletrônico (lojas, fabricantes, ecopontos municipais). Contêm metais pesados e substâncias tóxicas.", "O descarte correto evita contaminação do solo/água e permite a recuperação de materiais valiosos." },
                    { "Metal", "Metal (Aço/Ferro)", "Latas de alimentos (aço), tampas. Lave e amasse, se possível.", "Metais ferrosos são recicláveis. Não misture com pilhas ou latas de tinta/químicos.", "Reduz a extração de minério e o consumo de energia." },
                    { "Orgânico", "Resíduo Orgânico", "Restos de frutas, verduras, legumes, cascas de ovos, borra de café. Pode ser usado em compostagem doméstica.", "Não misture com lixo reciclável ou rejeitos. Evite carnes, laticínios e gorduras na compostagem caseira (a menos que tenha um sistema adequado).", "Reduz o metano em aterros e produz adubo rico para o solo." },
                    { "Papel", "Papel", "Mantenha seco e limpo. Não amasse em bola. Remova clipes grandes.", "Jornais, revistas, folhas são recicláveis. Papel carbono, fotográfico ou plastificado não são.", "Salva árvores, água e energia." },
                    { "Papelão", "Papelão", "Desmonte caixas. Mantenha seco e limpo. Embalagens longa vida podem ter coleta específica.", "Papelão limpo é reciclável. Evite o que estiver muito engordurado.", "Conserva recursos e reduz o volume de lixo." },
                    { "Plástico", "Plástico", "Lave, seque e, se possível, amasse embalagens plásticas. Verifique o símbolo de reciclabilidade (ex: PET, PP).", "Muitos plásticos são recicláveis. Evite misturar com orgânicos. Isopor e embalagens metalizadas geralmente não são.", "Reciclar plástico economiza petróleo e energia, reduzindo o lixo em aterros." },
                    { "Vidro", "Vidro", "Lave potes e garrafas. Deposite em coletores verdes. Embale vidros quebrados com segurança e sinalize.", "Vidro é 100% reciclável. Não misture com cerâmica, espelhos ou lâmpadas.", "Economia infinita de matéria-prima e energia na reciclagem." }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Barcode",
                keyValue: "1234567890000",
                columns: new[] { "DateAdded", "LastUpdated" },
                values: new object[] { new DateTime(2025, 5, 31, 2, 27, 50, 340, DateTimeKind.Utc).AddTicks(3411), new DateTime(2025, 5, 31, 2, 27, 50, 340, DateTimeKind.Utc).AddTicks(3411) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Barcode",
                keyValue: "7891000315507",
                columns: new[] { "DateAdded", "LastUpdated" },
                values: new object[] { new DateTime(2025, 5, 31, 2, 27, 50, 340, DateTimeKind.Utc).AddTicks(3392), new DateTime(2025, 5, 31, 2, 27, 50, 340, DateTimeKind.Utc).AddTicks(3395) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Barcode",
                keyValue: "7891149101017",
                columns: new[] { "DateAdded", "LastUpdated" },
                values: new object[] { new DateTime(2025, 5, 31, 2, 27, 50, 340, DateTimeKind.Utc).AddTicks(3407), new DateTime(2025, 5, 31, 2, 27, 50, 340, DateTimeKind.Utc).AddTicks(3408) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Barcode",
                keyValue: "7891991010769",
                columns: new[] { "DateAdded", "LastUpdated" },
                values: new object[] { new DateTime(2025, 5, 31, 2, 27, 50, 340, DateTimeKind.Utc).AddTicks(3399), new DateTime(2025, 5, 31, 2, 27, 50, 340, DateTimeKind.Utc).AddTicks(3400) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Barcode",
                keyValue: "7896005800057",
                columns: new[] { "DateAdded", "LastUpdated" },
                values: new object[] { new DateTime(2025, 5, 31, 2, 27, 50, 340, DateTimeKind.Utc).AddTicks(3403), new DateTime(2025, 5, 31, 2, 27, 50, 340, DateTimeKind.Utc).AddTicks(3404) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "Alumínio");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "Borracha");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "Eletrônico");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "Metal");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "Orgânico");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "Papel");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "Papelão");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "Plástico");

            migrationBuilder.DeleteData(
                table: "MaterialRecyclingGuides",
                keyColumn: "MaterialNameKey",
                keyValue: "Vidro");

            migrationBuilder.AddColumn<string>(
                name: "DisposalTips",
                table: "Suggestions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecyclingInfo",
                table: "Suggestions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SustainabilityImpact",
                table: "Suggestions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "MaterialRecyclingGuides",
                columns: new[] { "MaterialNameKey", "DisplayMaterialName", "DisposalTips", "RecyclingInfo", "SustainabilityImpact" },
                values: new object[,]
                {
                    { "metal", "Metais", "Latas de alumínio e aço (bebidas, alimentos), tampas metálicas e papel alumínio devem estar limpos. Latas preferencialmente amassadas.", "Não devem ser misturados aos recicláveis comuns: esponjas de aço, latas de tinta ou produtos químicos. Pilhas e baterias exigem descarte especial em pontos de coleta específicos.", "Reciclar metais, especialmente alumínio, economiza uma quantidade significativa de energia e reduz a necessidade de mineração." },
                    { "papel", "Papel", "Descarte jornais, revistas, folhas de caderno e caixas de papel limpas e secas. Evite amassar em bola. Remova contaminantes como clipes grandes ou fitas adesivas.", "Não são recicláveis: papéis higiênicos usados, papel carbono, fotografias, papéis plastificados ou aqueles muito sujos com gordura ou restos de alimentos (estes vão para o lixo comum).", "Reciclar papel salva árvores, reduz o consumo de água e energia, e diminui a quantidade de resíduos em aterros." },
                    { "papelao", "Papelão", "Desmonte caixas para reduzir volume. Certifique-se de que estejam limpas, livres de gordura ou umidade excessiva. Embalagens longa vida (Tetra Pak) podem ser recicladas se sua cidade tiver coleta específica.", "Não são recicláveis: Papelão muito engordurado (como o fundo de caixas de pizza sujas) ou molhado (estes vão para o lixo comum).", "A reciclagem de papelão conserva recursos naturais, economiza energia e reduz o lixo em aterros." },
                    { "plastico", "Plásticos", "Lave, seque e, se possível, amasse embalagens como garrafas PET, embalagens de produtos de limpeza/higiene e potes de alimentos. Verifique o símbolo de reciclabilidade.", "Geralmente não são recicláveis (verifique na sua cidade): isopor, embalagens metalizadas (ex: salgadinhos), adesivos e plásticos termorrígidos. Estes devem ir para o lixo comum para não contaminar o processo.", "Reciclar plástico economiza recursos naturais (petróleo), energia e reduz o volume de lixo nos aterros." },
                    { "vidro", "Vidro", "Garrafas, potes e frascos devem ser descartados limpos. Cacos de vidro precisam ser cuidadosamente embalados em jornal ou caixa de papelão resistente e devidamente sinalizados com 'CUIDADO: VIDRO QUEBRADO'.", "Não são recicláveis: espelhos, vidros temperados (Pyrex, box de banheiro), lâmpadas (requerem descarte especial), cerâmicas ou porcelanas. Estes devem ir para o lixo comum ou pontos de coleta específicos.", "O vidro é 100% reciclável e pode ser reciclado infinitas vezes, economizando energia e matéria-prima." }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Barcode",
                keyValue: "1234567890000",
                columns: new[] { "DateAdded", "LastUpdated" },
                values: new object[] { new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5309), new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5310) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Barcode",
                keyValue: "7891000315507",
                columns: new[] { "DateAdded", "LastUpdated" },
                values: new object[] { new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5240), new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5285) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Barcode",
                keyValue: "7891149101017",
                columns: new[] { "DateAdded", "LastUpdated" },
                values: new object[] { new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5304), new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5305) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Barcode",
                keyValue: "7891991010769",
                columns: new[] { "DateAdded", "LastUpdated" },
                values: new object[] { new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5291), new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5293) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Barcode",
                keyValue: "7896005800057",
                columns: new[] { "DateAdded", "LastUpdated" },
                values: new object[] { new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5298), new DateTime(2025, 5, 30, 5, 11, 21, 913, DateTimeKind.Utc).AddTicks(5299) });
        }
    }
}
