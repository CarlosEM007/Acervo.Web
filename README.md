# Acervo

Plataforma de livraria digital composta por uma **API REST** e uma **aplicação Web** desenvolvidas com .NET 8.

---

## Visão Geral

O Acervo permite que usuários naveguem por um catálogo de livros, gerenciem favoritos, carrinho de compras e biblioteca pessoal. A API fornece todos os dados e regras de negócio; a aplicação Web consome essa API e entrega a interface para o usuário.

---

## Tecnologias

| Camada | Tecnologia |
|---|---|
| API | ASP.NET Core 8, Entity Framework Core, SQL Server |
| Web | Blazor Server (.NET 8) |
| Autenticação | JWT Bearer |
| Documentação | Swagger / OpenAPI |
| ORM | Entity Framework Core com Migrations |

---

## Estrutura do Projeto

```
Acervo/
├── Acervo.API/             # Controllers e configuração da API
├── Acervo.Application/     # Serviços, casos de uso e DTOs
├── Acervo.Domain/          # Entidades e regras de negócio
├── Acervo.Infrastructure/  # Repositórios, DbContext e DataSeeder
├── Acervo.Utils/           # Utilitários compartilhados
└── Acervo.Web/             # Interface Blazor Server
```

### Por que essa separação?

- **Domain**: contém apenas as entidades e suas regras. Não depende de nada externo.
- **Application**: orquestra o que o sistema faz (serviços e DTOs). Só conhece o Domain.
- **Infrastructure**: cuida do banco de dados. Implementa as interfaces do Domain.
- **API**: expõe os endpoints HTTP. Usa Application para processar as requisições.
- **Web**: interface do usuário. Consome a API via HTTP, sem acesso direto ao banco.

---

## API

### Autenticação

A API usa **JWT Bearer**. Endpoints de leitura pública (livros, autores, categorias) estão abertos. Ações que envolvem o usuário (carrinho, favoritos, compras) exigem token.

**Login:**
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@email.com",
  "passwordHash": "senha123"
}
```

A resposta retorna um token que deve ser enviado nas próximas requisições:
```http
Authorization: Bearer <token>
```

### Endpoints Disponíveis

| Recurso | Rota Base | Autenticado |
|---|---|---|
| Autenticação | `/api/auth` | Não |
| Livros | `/api/book` | Não |
| Autores | `/api/author` | Não |
| Categorias | `/api/category` | Não |
| Editoras | `/api/publisher` | Não |
| Usuários | `/api/user` | Parcial* |
| Vendedores | `/api/seller` | Sim |
| Estoque | `/api/stock` | Sim |
| Itens de Estoque | `/api/stockitem` | Sim |
| Carrinho | `/api/cart` | Sim |
| Itens do Carrinho | `/api/cartitem` | Sim |
| Vendas | `/api/sale` | Sim |
| Itens de Venda | `/api/saleitem` | Sim |
| Favoritos | `/api/favorites` | Sim |
| Itens de Favoritos | `/api/favoritesitem` | Sim |
| Biblioteca | `/api/library` | Sim |
| Itens da Biblioteca | `/api/libraryitem` | Sim |

*Cadastro de usuário (`POST /api/user`) é público. Os demais métodos exigem autenticação.

Todos os endpoints seguem o padrão:
- `GET /api/{recurso}` — lista todos
- `GET /api/{recurso}/{id}` — busca por ID
- `POST /api/{recurso}` — cria novo (recebe DTO no body)
- `PUT /api/{recurso}` — atualiza (recebe DTO com ID no body)
- `DELETE /api/{recurso}/{id}` — remove por ID

### Documentação Interativa

Com a API rodando, acesse o Swagger em:
```
https://localhost:7001/swagger
```

---

## Aplicação Web

Interface construída com **Blazor Server**, que se comunica com a API via HTTP.

### Páginas

| Página | Rota | Descrição |
|---|---|---|
| Início | `/home` | Lançamentos e mais vendidos |
| Catálogo | `/catalogo` | Todos os livros com filtro e ordenação |
| Autores | `/autores` | Lista de autores com busca |
| Busca | `/busca` | Pesquisa global por título, autor ou categoria |
| Detalhes do Livro | `/livro/{id}` | Informações completas de um livro |
| Carrinho | `/carrinho` | Itens selecionados para compra |
| Favoritos | `/favoritos` | Livros marcados como favoritos |
| Minha Conta | `/conta` | Perfil, pedidos e biblioteca pessoal |
| Login | `/login` | Autenticação |
| Cadastro | `/cadastro` | Criação de conta |

### Como a Web se conecta à API

Cada entidade tem um **Service** dedicado que faz as requisições HTTP:

```
BookService   → GET /api/book
AuthorService → GET /api/author
...
```

O token JWT é armazenado no `SessionService` (por usuário) e enviado automaticamente em todas as requisições via `AuthHandler` (DelegatingHandler).

---

## Como Rodar Localmente

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local ou Docker)

### 1. Configurar o banco de dados

No arquivo `Acervo.API/appsettings.json`, ajuste a connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Acervo;Trusted_Connection=True;"
}
```

### 2. Configurar o JWT

No mesmo `appsettings.json`:

```json
"Jwt": {
  "Chave": "sua-chave-secreta-longa-aqui",
  "Issuer": "Acervo.API",
  "Audience": "Acervo.Web"
}
```

### 3. Rodar a API

```bash
cd Acervo.API
dotnet run
```

As migrations são aplicadas e os dados iniciais são inseridos automaticamente ao subir a API.

### 4. Rodar o Web

```bash
cd Acervo.Web
dotnet run
```

Certifique-se de que a URL base da API no `Program.cs` do Web aponta para onde a API está rodando.

---

## Dados Iniciais (Seed)

Ao iniciar a API pela primeira vez, o `DataSeeder` popula automaticamente:

- 21 livros clássicos e contemporâneos com capas
- Autores, editoras e categorias correspondentes
- Itens de estoque com preços

---

## Observações de Segurança

- Senhas são armazenadas com hash **BCrypt**
- Endpoints sensíveis exigem `[Authorize]`
- A controller nunca recebe entidades de domínio diretamente — apenas DTOs
- O token JWT expira em 8 horas
