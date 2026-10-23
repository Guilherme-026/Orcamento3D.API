# Orcamento3D.API - Sistema de Precificacao para Impressora 3D

# 🖨️ Orcamento3D.API — Sistema de Precificação e Propostas para Impressão 3D

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![TailwindCSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)

> **Solução completa desenvolvida para um cenário real de negócio:** Automação do cálculo de custos operacionais de impressão 3D, precificação estratégica em múltiplas faixas (*Tiers*) e geração instantânea de propostas comerciais em PDF.

---

## 💡 O Problema de Negócio

No dia a dia de um estúdio de impressão 3D, calcular manualmente o consumo proporcional de filamento por grama, somar a depreciação/energia da máquina por hora e formatar um orçamento profissional para o cliente gerava retrabalho e perda de tempo. Além disso, o sistema precisava rodar de forma extremamente leve no computador de produção do estúdio (hardware antigo com restrição de memória RAM).

## 🚀 A Solução de Engenharia

O **Orcamento3D.API** foi construído aplicando princípios de **Programação Orientada a Objetos (POO)** e **Responsabilidade Única (SRP)**, entregando:

1. **Cálculo Preciso de Insumos e Máquina:** Uso estrito de `decimal` para operações financeiras e suporte a medidas exatas vindas dos fatiadores 3D (gramatura com casas decimais como `72,01g` e tempo separado em `Horas` e `Minutos`).
2. **Precificação em 3 Tiers Comerciais (Visão do Operador):**
   * 🟢 **Low-Ticket (+30%):** Alta competitividade, lotes ou peças simples.
   * 🔵 **Average-Ticket (+50%):** Margem padrão recomendada para operação regular.
   * 🟣 **High-Ticket (+100%):** Peças complexas, exclusividade ou taxa de urgência.
3. **Geração de PDF com Valor Único (Visão do Cliente):** Integração com **QuestPDF** que oculta as opções internas de margem e gera um documento comercial limpo contendo apenas o valor final escolhido, alocando a diferença estratégica sob a rubrica de *"Mão de Obra e Acabamento"*.
4. **Arquitetura de Dupla Entrega (DevOps + Nativo):**
   * **Containerização em Docker:** *Multi-Stage Build* otimizado para ambientes de servidor e nuvem.
   * **Binário Nativo Ultraleve (`win-x64`):** Publicação *Self-Contained* acoplada a uma interface Web/PWA (`wwwroot`) iniciada silenciosamente via script `.vbs`, consumindo apenas **~30 MB de RAM** na máquina do cliente final (evitando o overhead de 2 GB a 4 GB do WSL2/Docker Desktop em hardwares modestos).

---

## 🛠️ Tecnologias Utilizadas

* **Back-End:** C# / ASP.NET Core Web API (.NET 10)
* **Geração de Documentos:** QuestPDF (Community License)
* **Front-End Integrado:** HTML5, Tailwind CSS, JavaScript (ES6 Fetch API)
* **Infraestrutura & Deploy:** Docker (Linux Containers) & .NET Self-Contained Deployment (`win-x64`)
* **IDE:** JetBrains Rider

---

## 📂 Estrutura do Projeto

```text
📁 Orcamento3D.API/
 ├── 📁 Controllers/
 │    └── 📄 OrcamentosController.cs   # Endpoints REST: calcular-orcamento e gerar-pdf
 ├── 📁 Models/
 │    ├── 📄 Filamento.cs              # Entidade e cálculo de custo por grama
 │    ├── 📄 Impressora.cs             # Entidade de custo hora-máquina
 │    ├── 📄 Peca3D.cs                 # Entidade física da peça 3D
 │    └── 📄 Orcamento.cs              # Regras de negócio, Tiers e layout do QuestPDF
 ├── 📁 wwwroot/
 │    └── 📄 index.html                # Interface gráfica responsiva integrada à API
 ├── 📄 Program.cs                     # Pipeline da API e servidor de arquivos estáticos
 ├── 📄 Dockerfile                     # Multi-Stage Build (.NET 10 SDK -> ASP.NET Runtime)
 └── 📄 iniciar.vbs                    # Automação para abertura silenciosa em modo App
```
## ⚙️ Como Executar o Projeto
Opção 1: Via Docker (Recomendado para Servidores / Avaliação)
Na raiz do projeto, execute os comandos abaixo no terminal:
```text
# 1. Construir a imagem otimizada
docker build -t orcamento3d-api .

# 2. Subir o container em segundo plano na porta 8080
docker run -d -p 8080:8080 --name orcamento-app orcamento3d-api
```
Opção 2: Gerando o Executável Nativo Ultraleve (Windows)
Para gerar o pacote que roda sem necessidade do .NET SDK ou Docker instalados na máquina de destino:
```text
dotnet publish Orcamento3D.API/Orcamento3D.API.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```
