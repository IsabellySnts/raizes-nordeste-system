# Rede Raizes do Nordeste - Sistema de Gestao

Sistema back-end para gerenciamento de uma rede de franquias de culinaria nordestina brasileira. Desenvolvido como projeto academico do 5o semestre do CST em Analise e Desenvolvimento de Sistemas (UNINTER).

**Aluna:** Isabelly dos Santos | **RU:** 4728265

---

## Tecnologias

- **Runtime:** .NET 10 / C# 13
- **Arquitetura:** Clean Architecture + CQRS com MediatR
- **Banco de dados:** SQL Server 2022 (Docker)
- **Autenticacao:** JWT Bearer Tokens
- **Validacao:** FluentValidation + ValidationBehavior (pipeline MediatR)
- **Pagamentos:** Stripe (PaymentIntent API)
- **Mensageria:** RabbitMQ (infraestrutura provisionada)
- **Documentacao:** Swagger / OpenAPI

---

## Estrutura da Solucao

```
RaizesDoNordeste/
├── RaizesDoNordeste.API            # Controllers, Middlewares, Program.cs
├── RaizesDoNordeste.Application    # Commands, Queries, Handlers, Validators, Services
├── RaizesDoNordeste.Domain         # Entidades, Enums, Interfaces de Repositorio
├── RaizesDoNordeste.Infrastructure # Repositorios (EF Core), DbContext, Auth, Stripe
├── RaizesDoNordeste.Consumer       # Worker Service (RabbitMQ)
└── docker-compose.yaml             # SQL Server + RabbitMQ
```

---

## Pre-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/pt-br/downloads/) ou VS Code com extensao C#

---
## Verificar as instalações

Abra o terminal e execute:

git --version
dotnet --version
docker --version
docker compose version

O comando dotnet --version deve retornar uma versão compatível com o .NET 10. O Docker Desktop também precisa estar aberto e funcionando.

## Como Executar

### 1. Clonar o repositorio

```bash
git clone https://github.com/IsabellySnts/raizes-nordeste-system.git
cd raizes-nordeste-system
```

### 2. Subir os containers (SQL Server + RabbitMQ)

```bash
docker-compose up -d
```

Isso inicia:
- **SQL Server** na porta `1433` (usuario: `sa`, senha: `Raizes@2026!`)
- **RabbitMQ** na porta `5672` (management: `http://localhost:15672`, usuario: `admin`, senha: `admin`)

Para verificar se os containers estão em execução:
```bash
docker compose ps
```
### 3. Instalar a ferramenta de migrations

O Entity Framework Core utiliza a ferramenta dotnet-ef para executar migrations e atualizar o banco de dados.

Caso ainda não esteja instalada, execute:

```bash
dotnet tool install --global dotnet-ef
```
Verifique a instalação:
```bash
dotnet ef --version
```

### 4. Aplicar as migrations (criar o banco)

```bash
cd RaizesDoNordeste.API
dotnet ef database update --project ..\RaizesDoNordeste.Infrastructure
```
### 5. Executar a API

```bash
dotnet run --project RaizesDoNordeste.API
```

A API estara disponivel em: `https://localhost:7XXX/swagger` (a porta exata aparece no console).

---

## Autenticacao

A API utiliza JWT Bearer Token. Para acessar endpoints protegidos:

1. Crie um usuario via `POST /api/Usuario`
2. Faca login via `POST /api/Usuario/login`
3. Copie o token retornado
4. No Swagger, clique no cadeado "Authorize" e insira: `Bearer {seu_token}`

### Perfis de Acesso

| Perfil | Codigo | Permissoes |
|--------|--------|------------|
| Admin | 0 | Acesso total ao sistema |
| Gerente | 1 | Gestao de unidade, cardapio, estoque, pedidos |
| Atendente | 2 | Pedidos e atendimento |
| Cozinheiro | 3 | Atualizacao de status de pedidos |
| Cliente | 4 | Pedidos, fidelidade, consentimentos |

---

## Principais Funcionalidades

### Gestao de Franquias
- CRUD de unidades com tipo de cozinha (Completa/Reduzida)
- Cardapio por unidade com variacao regional e preco local
- Controle de estoque por unidade com quantidade minima

### Pedidos e Pagamentos
- Criacao de pedidos com multiplos itens e observacoes
- Integracao com Stripe para processamento de pagamentos (PaymentIntent)
- Fluxo de status: Criado -> AguardandoPagamento -> Pago -> EmPreparo -> Pronto -> Entregue
- Endpoint de confirmacao manual para testes sem webhook

### Programa de Fidelidade
- Acumulo automatico de pontos ao entregar pedido (R$1 = 1 ponto)
- Niveis: Bronze (< 500 pts), Prata (>= 500 pts), Ouro (>= 1000 pts)
- Resgate de pontos em pedidos futuros
- Historico de movimentacoes (acumulo/resgate)

### Conformidade LGPD
- Consentimentos obrigatorios: Dados Pessoais e Termos de Uso
- Consentimentos opcionais: Marketing, Fidelizacao, Comunicacao (Email/SMS)
- Registro e revogacao de consentimentos por cliente

### Seguranca e Qualidade
- Tratamento global de excecoes (`IExceptionHandler`)
- Validacao de entrada com FluentValidation no pipeline MediatR
- Respostas padronizadas via `ResultViewModel` com `ResultStatus`
- Auditoria de operacoes criticas

---

## Endpoints da API

| Recurso | Metodos | Autenticacao |
|---------|---------|-------------|
| `/api/Usuario` | POST, GET, Login | Publico (criar) / JWT |
| `/api/Cliente` | POST, GET, PUT, DELETE | Publico (cadastro) / JWT |
| `/api/Unidade` | POST, GET, PUT, DELETE | Admin |
| `/api/Categoria` | POST, GET, PUT, DELETE | Admin, Gerente |
| `/api/Produtos` | POST, GET, PUT, DELETE | Admin, Gerente |
| `/api/Cardapio` | POST, GET, PUT, DELETE | Admin, Gerente |
| `/api/Estoque` | POST, GET, PUT | Admin, Gerente |
| `/api/Funcionario` | POST, GET, PUT, DELETE | Admin |
| `/api/Pedido` | POST, GET, PATCH (status) | Admin, Gerente, Atendente, Cliente |
| `/api/Pagamento` | POST, GET, confirmar | Admin, Gerente, Atendente, Cliente |
| `/api/Fidelidade` | GET | JWT |
| `/api/Consentimento` | POST, GET, DELETE | Cliente |
| `/api/Campanha` | POST, GET, PUT, DELETE | Admin, Gerente |
| `/api/Auditoria` | GET | Admin |

---

## Roteiro de Testes

O arquivo `roteiro-testes.md` contem um fluxo completo e sequencial com 20 passos para validacao do sistema, desde a criacao do primeiro usuario ate os testes de tratamento de erros. Inclui todos os payloads JSON necessarios.

**Fluxo critico testado:**
1. Criar Admin -> Login -> Criar Unidade -> Categoria -> Produto -> Cardapio -> Estoque
2. Criar Funcionario -> Cadastrar Cliente -> Login Cliente
3. Criar Pedido -> Pagamento (Stripe) -> Confirmar -> Atualizar Status ate Entregue
4. Verificar acumulo de pontos de fidelidade
5. Testes de erro: validacao (400), sem autenticacao (401), sem permissao (403), nao encontrado (404)

---

## Decisoes Arquiteturais

- **Clean Architecture:** separacao clara entre API, Application, Domain e Infrastructure, facilitando manutencao e testabilidade.
- **CQRS com MediatR:** Commands e Queries separados, com handlers independentes e pipeline de validacao.
- **ValidationBehavior:** intercepta todos os requests no pipeline do MediatR antes de chegar ao handler, retornando erros de validacao padronizados.
- **GlobalExceptionHandler (IExceptionHandler):** tratamento centralizado de excecoes, retornando sempre `ResultViewModel`.
- **Stripe PaymentIntent:** fluxo seguro de pagamentos onde o servidor cria a intencao e o cliente confirma, com endpoint manual para testes academicos.
- **Consentimentos LGPD:** modelagem explicita de consentimentos obrigatorios e opcionais, com registro e revogacao.

---

## Configuracao Stripe (Teste)

A API utiliza chaves de teste do Stripe. Para testar pagamentos:
1. Crie um pedido via `POST /api/Pedido`
2. Crie o pagamento via `POST /api/Pagamento` (retorna `clientSecret`)
3. Confirme manualmente via `POST /api/Pagamento/{idPedido}/confirmar` (requer Admin/Gerente)

> As chaves no `appsettings.json` sao de teste (`sk_test_...`) e nao processam pagamentos reais.
