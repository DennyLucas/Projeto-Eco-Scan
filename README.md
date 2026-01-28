# EcoScan - Informações de Reciclagem na Palma da Mão ♻️📱

**Repositório:**  https://github.com/IzidoroSouza/ProjetoEcoScan 

Aplicação móvel Full Stack (React Native com Expo + ASP.NET Core) que permite escanear códigos de barras de produtos para obter informações sobre seus materiais, dicas de descarte e reciclagem, e impacto na sustentabilidade. O projeto também permite que usuários contribuam com informações para produtos não encontrados.

---

## 🎯 Visão Geral do Projeto

O EcoScan visa facilitar o descarte correto e a reciclagem de produtos. Ao escanear um código de barras:

1.  O aplicativo primeiro tenta obter informações do produto através da API pública [OpenFoodFacts](https://world.openfoodfacts.org/).
2.  Se o produto é encontrado na API, o EcoScan busca em seu próprio banco de dados guias de reciclagem para o material identificado.
3.  Se o produto não é encontrado na API pública, o EcoScan verifica seu banco de dados local por informações já cadastradas para aquele código de barras.
4.  Caso o produto não seja encontrado em nenhuma fonte ou as informações de reciclagem para o material sejam desconhecidas, o usuário é convidado a preencher um formulário de sugestão, contribuindo para enriquecer a base de dados do EcoScan.

---

## 🛠️ Tecnologias Utilizadas

**Backend:**
*   .NET 8
*   ASP.NET Core Web API
*   Entity Framework Core 8.0.4 (Verifique a versão exata no `.csproj`)
*   SQLite
*   C#

**Frontend:**
*   React Native (com Expo)
*   JavaScript (ES6+)
*   Expo SDK (Verifique a versão no `package.json`)
*   Expo Camera

**Banco de Dados:**
*   SQLite (arquivo `ProdutosEco.db`)

**API Externa:**
*   OpenFoodFacts API v2

---

## ✅ Pré-requisitos

*   [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0) (ou a versão especificada no seu `global.json` ou `TargetFramework`)
*   [Node.js](https://nodejs.org/) (v18 ou superior recomendado) e npm (ou Yarn)
*   [Git](https://git-scm.com/)
*   [Expo Go App](https://expo.dev/go) instalado no seu dispositivo móvel (Android/iOS) para testes em dispositivo físico.
*   Um emulador Android configurado (via Android Studio) ou simulador iOS (via Xcode em um macOS) para testes em ambiente virtual.

---

## ⚙️ Configuração e Instalação

Siga os passos para rodar o projeto localmente:

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/SEU_USUARIO_GITHUB/EcoScan.git
    cd EcoScan
    ```
    (A estrutura esperada é `EcoScan/BackEnd/BackEnd/` e `EcoScan/FrontEnd/`)

2.  **Configure o Backend:**
    *   Navegue até a pasta do projeto backend:
        ```bash
        cd BackEnd/BackEnd
        ```
    *   Restaure as dependências .NET:
        ```bash
        dotnet restore
        ```
    *   Crie/Atualize o banco de dados SQLite (`ProdutosEco.db`):
        ```bash
        dotnet ef database update
        ```
        > **Nota Importante:** Se o comando `dotnet ef` falhar com "comando não encontrado":
        > 1.  Instale as ferramentas do Entity Framework Core globalmente:
        >     ```bash
        >     dotnet tool install --global dotnet-ef 
        >     ```
        >     (Substitua `8.0.x` pela **mesma versão major.minor.patch dos pacotes `Microsoft.EntityFrameworkCore.Design` e `Microsoft.EntityFrameworkCore.Tools`** listados no seu arquivo `BackEnd/BackEnd/BackEnd.csproj`. Por exemplo, se os pacotes são `8.0.4`, use `--version 8.0.4`).
        > 2.  **Feche e reabra seu terminal/prompt de comando.**
        > 3.  Tente `dotnet ef database update` novamente na pasta `BackEnd/BackEnd`.

3.  **Configure o Frontend:**
    *   Navegue até a pasta do frontend:
        ```bash
        cd ../../FrontEnd
        ```
        (Se você estava em `BackEnd/BackEnd`, volte duas pastas e entre em `FrontEnd`)
    *   Instale as dependências Node.js:
        ```bash
        npm install
        ```
        (ou `yarn install` se você preferir Yarn)

4.  **⚠️ Ajuste as URLs de Conexão (MUITO IMPORTANTE):**

    *   **Frontend (`FrontEnd/App.js`):**
        *   Abra o arquivo `FrontEnd/App.js`.
        *   Localize a constante `BACKEND_URL`.
        *   Você precisará alterá-la para o endereço IP da máquina onde o backend estará rodando, acessível pelo seu emulador/dispositivo:
            *   **Para Emulador Android (backend no mesmo PC):** Geralmente `http://10.0.2.2:PORTA_DO_BACKEND` (ex: `http://10.0.2.2:5291`).
            *   **Para Dispositivo Físico ou Emulador iOS (backend no mesmo PC):** Use o IP da sua máquina na rede local (Wi-Fi). Ex: `http://192.168.1.10:5291`. Você pode encontrar seu IP local com `ipconfig` (Windows) ou `ifconfig`/`ip addr` (Linux/macOS).
            *   **Exemplo de como deve ficar no código:**
                ```javascript
                const BACKEND_URL = 'http://SEU_IP_CORRETO:5291';
                ```

    *   **Backend (`BackEnd/BackEnd/Program.cs`):**
        *   Abra o arquivo `BackEnd/BackEnd/Program.cs`.
        *   Localize as linhas `app.Urls.Add(...)`.
        *   Certifique-se de que uma das URLs adicionadas seja o mesmo IP que você configurou no frontend (além de `http://localhost:5291` para testes locais no PC).
            *   **Exemplo:**
                ```csharp
                app.Urls.Add("http://localhost:5291");
                app.Urls.Add("http://SEU_IP_CORRETO:5291"); // Mesma porta e IP usado no frontend
                ```

---

## ▶️ Execução

Execute o backend e o frontend **simultaneamente** em terminais separados:

1.  **Iniciar Backend (.NET API):**
    *   Abra um **Terminal 1**.
    *   Navegue até `EcoScan/BackEnd/BackEnd/`.
    *   Execute:
        ```bash
        dotnet run
        ```
    *   Observe as URLs informadas onde o servidor está escutando (ex: `http://localhost:5291` e `http://SEU_IP_CORRETO:5291`). **Mantenha este terminal aberto.**

2.  **Iniciar Frontend (React Native App com Expo):**
    *   Abra um **Terminal 2**.
    *   Navegue até `EcoScan/FrontEnd/`.
    *   Execute:
        ```bash
        npx expo start
        ```
        (ou `npm start`, ou `yarn start`)
    *   Isso iniciará o Metro Bundler e exibirá um QR Code no terminal e/ou abrirá o Expo Dev Tools no seu navegador.
    *   **Para rodar o aplicativo:**
        *   **Emulador/Simulador:** No terminal do Expo ou no Dev Tools, pressione `a` para Android ou `i` para iOS.
        *   **Dispositivo Físico:** Abra o aplicativo **Expo Go** no seu celular/tablet (conectado à mesma rede Wi-Fi que seu computador) e escaneie o QR Code.

---

## ✨ Funcionalidades e Testes

A API não requer autenticação. A interação principal é através do aplicativo móvel.

### Interface do Usuário (Frontend - App Móvel)

*   **Tela Inicial:** Botão para "Escanear Código de Barras".
*   **Tela da Câmera:** Use a câmera para escanear um código de barras.
*   **Exibição de Informações:** Se o produto for identificado (via API ou banco de dados local), suas informações de material, descarte, etc., são exibidas.
*   **Formulário de Sugestão:**
    *   Aparece se o produto não for encontrado em nenhuma fonte, ou se as informações de reciclagem para um material específico não estiverem no banco de dados do EcoScan.
    *   Campos: Código de Barras (pré-preenchido), Nome do Produto\*, Material Principal\*, Dicas de Descarte, Informações de Reciclagem, Impacto/Sustentabilidade. (\* obrigatórios).
    *   Ao enviar, o usuário recebe uma mensagem de agradecimento.

### Ações Principais

*   **Escanear Código de Barras:**
    1.  O app tenta buscar informações na API OpenFoodFacts.
    2.  Se encontrado e o material tiver um guia de reciclagem no DB do EcoScan: exibe informações combinadas.
    3.  Se encontrado e o material NÃO tiver um guia: exibe informações da API e permite sugestão para o guia.
    4.  Se não encontrado na API: busca no DB de produtos do EcoScan.
    5.  Se encontrado no DB do EcoScan: exibe informações do DB.
    6.  Se não encontrado em lugar nenhum: exibe formulário de sugestão para o produto.
*   **Enviar Sugestão:**
    1.  Os dados do formulário são enviados para o backend.
    2.  Uma nova entrada é criada na tabela `Suggestions` do banco de dados.
    3.  Campos de dicas não preenchidos recebem "Aguardando análise".

### Testando a API Diretamente (Ex: Postman ou Swagger UI)

*   **URL Base do Swagger:** `http://localhost:5291/swagger` (quando o backend estiver rodando)

*   **`GET /api/productinfo?barcode={codigo_de_barras}`**
    *   _Função:_ Busca informações do produto.
    *   _Parâmetro de Query:_ `barcode` (string) - O código de barras a ser pesquisado.
    *   _Resposta Esperada:_ Objeto `EcoProductInfoResponse` contendo detalhes do produto, fonte dos dados e se uma sugestão é necessária.
    *   _Teste:_ Use códigos de barras conhecidos, desconhecidos, e aqueles que você semeou no DB.

*   **`POST /api/suggestions`**
    *   _Função:_ Envia um novo formulário de sugestão.
    *   _Corpo da Requisição (JSON):_ Objeto `CreateSuggestionDto`.
        ```json
        {
          "barcode": "9998887776665",
          "productName": "Meu Produto Sugerido",
          "material": "Plástico Tipo X",
          "disposalTips": "Lavar bem.", // Opcional
          "recyclingInfo": "Coleta seletiva.", // Opcional
          "sustainabilityImpact": "Ajuda a reduzir o lixo." // Opcional
        }
        ```
    *   _Resposta Esperada (Sucesso):_ `200 OK` com uma mensagem como `{"message":"Obrigado pela sua colaboração!"}`.
    *   _Teste:_ Envie um formulário válido e verifique se a sugestão é salva no banco de dados (usando uma ferramenta de visualização SQLite ou um endpoint GET para sugestões, se criado).

*   **`GET /api/suggestions` (Opcional - para administradores/debug)**
    *   _Função:_ Lista todas as sugestões enviadas.
    *   _Teste:_ Verificar se as sugestões enviadas via POST aparecem aqui.

---

## 📝 Notas Adicionais

*   **Persistência:** Os dados do backend são salvos no arquivo `EcoScan/BackEnd/BackEnd/ProdutosEco.db`.
*   **Qualidade dos Dados da API Externa:** A completude e precisão dos dados da OpenFoodFacts API podem variar. O EcoScan tenta complementar ou permitir que os usuários complementem essas informações.
*   **Processo de Moderação de Sugestões:** Este README não cobre um sistema de moderação/aprovação de sugestões. Em uma aplicação real, as sugestões seriam revisadas antes de serem incorporadas à tabela principal de `Products` ou `MaterialRecyclingGuides`.
