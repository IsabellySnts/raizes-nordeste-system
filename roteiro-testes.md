# Roteiro de Testes — Rede Raízes do Nordeste API

> Fluxo completo e sequencial para validação do sistema.  
> Execute os passos na ordem apresentada. Todos os IDs assumem banco limpo (recém-criado).

---

## 1. Criar Usuário Admin

`POST /api/Usuario`

```json
{
  "email": "admin@raizes.com",
  "senha": "Admin@123",
  "confirmarSenha": "Admin@123",
  "perfil": 0
}
```

> **Nota:** Este é o primeiro usuário do sistema. Como não há Admin ainda, esse endpoint deve permitir a criação inicial. Caso exija autenticação, verifique se há seed de Admin no banco.

---

## 2. Login como Admin

`POST /api/Usuario/login`

```json
{
  "email": "admin@raizes.com",
  "senha": "Admin@123"
}
```

> **Copie o token JWT retornado.** No Swagger, clique em "Authorize" (cadeado) e insira: `Bearer {seu_token}`

---

## 3. Criar Unidade (Franquia)

`POST /api/Unidade` — **Requer: Admin**

```json
{
  "nome": "Raízes do Nordeste - Centro Recife",
  "cidade": "Recife",
  "estado": "PE",
  "pais": "Brasil",
  "logradouro": "Rua do Bom Jesus, 150 - Recife Antigo",
  "complemento": "Loja 3",
  "diasFuncionamento": "Segunda a Sábado",
  "horarioFuncionamento": "10:00 - 22:00",
  "tipoCozinha": 1
}
```

> `tipoCozinha`: 1 = Completa, 2 = Reduzida

---

## 4. Criar Categoria

`POST /api/Categoria` — **Requer: Admin ou Gerente**

```json
{
  "nome": "Pratos Típicos",
  "descricao": "Pratos tradicionais da culinária nordestina"
}
```

---

## 5. Criar Produto

`POST /api/Produtos` — **Requer: Admin ou Gerente**

```json
{
  "nome": "Baião de Dois",
  "idCategoria": 1,
  "descricao": "Arroz com feijão-de-corda, queijo coalho, nata e charque",
  "preco": 28.90,
  "flagSazonal": false,
  "dataInicioDisponibilidade": null,
  "dataFimDisponibilidade": null,
  "foto": ""
}
```

---

## 6. Vincular Produto ao Cardápio da Unidade

`POST /api/Cardapio?unidadeId=1` — **Requer: Admin ou Gerente**

```json
{
  "idUnidade": 1,
  "idProduto": 1,
  "disponivel": true,
  "precoLocal": 28.90,
  "variacaoRegional": "Versão original pernambucana"
}
```

---

## 7. Criar Estoque do Produto na Unidade

`POST /api/Estoque?unidadeId=1` — **Requer: Admin ou Gerente**

```json
{
  "idUnidade": 1,
  "idProduto": 1,
  "quantidade": 50,
  "quantidadeMinima": 5
}
```

---

## 8. Criar Funcionário

`POST /api/Funcionario` — **Requer: Admin**

```json
{
  "nome": "Maria da Silva",
  "cpf": "98765432100",
  "email": "maria@raizes.com",
  "telefone": "81999998888",
  "idUnidade": 1,
  "cargo": 2,
  "senha": "Func@123",
  "confirmarSenha": "Func@123"
}
```

> `cargo`: 1 = Gerente, 2 = Atendente, 3 = Cozinheiro, 4 = GestorMatriz

---

## 9. Cadastrar Cliente (Endpoint Público)

`POST /api/Cliente` — **Sem autenticação**

```json
{
  "nomeCompleto": "João Pedro Oliveira",
  "cpf": "12345678901",
  "telefone": "81988887777",
  "email": "joao@email.com",
  "dataNascimento": "1995-06-15T00:00:00Z",
  "senha": "Cliente@123",
  "confirmarSenha": "Cliente@123",
  "consentimentos": [
    {
      "permissao": 2,
      "aceite": true
    },
    {
      "permissao": 6,
      "aceite": true
    }
  ]
}
```

> **Consentimentos obrigatórios:** 2 = Dados Pessoais, 6 = Termos de Uso  
> Opcionais: 1 = Marketing, 3 = Fidelização, 4 = Comunicação Email, 5 = Comunicação SMS

---

## 10. Login como Cliente

`POST /api/Usuario/login`

```json
{
  "email": "joao@email.com",
  "senha": "Cliente@123"
}
```

> **Copie o novo token JWT.** Atualize o Authorize no Swagger com o token do cliente.

---

## 11. Registrar Consentimento Adicional (Fidelização)

`POST /api/Consentimento?clienteId=1` — **Requer: Cliente (logado)**

```json
{
  "idCliente": 1,
  "permissao": 3
}
```

> Permissão 3 = Fidelização. Necessário para participar do programa de pontos.

---

## 12. Consultar Cardápio Disponível

`GET /api/Cardapio/unidade/1/disponiveis` — **Requer: Autenticação**

> Retorna os produtos disponíveis na unidade 1 com preços e variações regionais.

---

## 13. Criar Pedido

`POST /api/Pedido` — **Requer: Admin, Gerente, Atendente ou Cliente**

```json
{
  "idCliente": 1,
  "idUnidade": 1,
  "idFuncionario": null,
  "canalOrigem": 1,
  "itens": [
    {
      "idProduto": 1,
      "quantidade": 2,
      "observacao": "Sem coentro, por favor"
    }
  ],
  "pontosParaResgatar": 0
}
```

> `canalOrigem`: 1 = App, 2 = Totem, 3 = Balcão, 4 = PickUp  
> `pontosParaResgatar`: 0 para primeiro pedido (cliente ainda não tem pontos)  
> **Resposta inclui:** ValorTotal, PontosAcumulados (se implementado)

---

## 14. Criar Pagamento (Stripe)

`POST /api/Pagamento` — **Requer: Admin, Gerente, Atendente ou Cliente**

```json
{
  "idPedido": 1
}
```

> **Resposta:** retorna `clientSecret` e `stripePaymentIntentId`  
> Em produção, o front-end usaria o `clientSecret` para confirmar via Stripe.js

---

## 15. Confirmar Pagamento Manualmente

`POST /api/Pagamento/1/confirmar` — **Requer: Admin ou Gerente**

> Sem body. O ID na URL é o ID do **pedido**.  
> **Troque o token para Admin/Gerente** antes de chamar este endpoint.  
> Simula a confirmação do pagamento sem depender do webhook do Stripe.

---

## 16. Consultar Pagamento

`GET /api/Pagamento/pedido/1` — **Requer: Admin, Gerente, Atendente ou Cliente**

> Retorna status do pagamento, código de transação Stripe e dados do pedido.

---

## 17. Atualizar Status do Pedido

Execute na sequência (cada chamada avança o status):

`PATCH /api/Pedido/1/status` — **Requer: Admin, Gerente, Atendente ou Cozinheiro**

### 17.1 — Pago → Em Preparo

```json
{
  "idPedido": 1,
  "novoStatus": 3
}
```

### 17.2 — Em Preparo → Pronto para Retirada

```json
{
  "idPedido": 1,
  "novoStatus": 4
}
```

### 17.3 — Pronto → Entregue

```json
{
  "idPedido": 1,
  "novoStatus": 5
}
```

> **Ao marcar como Entregue**, os pontos de fidelidade são acumulados automaticamente (R$1 = 1 ponto).  
> Os valores de `novoStatus` podem variar conforme seu enum `StatusPedido` — ajuste se necessário.

---

## 18. Consultar Pontos de Fidelidade

`GET /api/Fidelidade?clienteId=1` — **Requer: Autenticação**

> Retorna saldo de pontos, nível (Bronze/Prata/Ouro) e histórico de movimentações.  
> Após o pedido de R$57,80 (2x Baião de Dois), o cliente deve ter **57 pontos**.

---

## 19. Criar Campanha Promocional

`POST /api/Campanha` — **Requer: Admin ou Gerente**

```json
{
  "nome": "Sabores do São João",
  "descricao": "Campanha junina com pratos típicos do Nordeste e descontos especiais",
  "criterios": "Válido para pedidos acima de R$30,00 em qualquer unidade",
  "dataInicio": "2026-10-12T00:00:00Z",
  "dataFim": "2026-11-12T23:59:59Z",
  "beneficio": "15% de desconto em pratos sazonais"
}
```

---

## 20. Testar Tratamento de Erros

### 20.1 — Validação (campo obrigatório vazio)

`POST /api/Cliente` — **Sem autenticação**

```json
{
  "nomeCompleto": "",
  "cpf": "",
  "telefone": "",
  "email": "",
  "dataNascimento": "2026-01-01T00:00:00Z",
  "senha": "",
  "confirmarSenha": "",
  "consentimentos": []
}
```

> **Esperado:** 400 Bad Request com mensagens de validação no formato `ResultViewModel`

### 20.2 — Recurso não encontrado

`GET /api/Fidelidade?clienteId=9999`

> **Esperado:** 404 Not Found

### 20.3 — Sem autenticação

`POST /api/Unidade` (sem token no Authorize)

> **Esperado:** 401 Unauthorized

### 20.4 — Sem permissão

`POST /api/Unidade` (logado como Cliente)

> **Esperado:** 403 Forbidden

---

## Resumo do Fluxo Testado

| Funcionalidade | Endpoint | Status |
|---|---|---|
| Persistência em banco | Todos os POST | ✅ |
| Autenticação JWT | Login + Token | ✅ |
| Autorização por perfil | Admin, Gerente, Cliente | ✅ |
| Tratamento de erros | Validação, 401, 403, 404 | ✅ |
| Fluxo crítico completo | Pedido → Pagamento → Entrega → Pontos | ✅ |
| Integração externa | Stripe (PaymentIntent) | ✅ |
| LGPD | Consentimentos obrigatórios | ✅ |
| Programa de fidelidade | Acúmulo e consulta de pontos | ✅ |
