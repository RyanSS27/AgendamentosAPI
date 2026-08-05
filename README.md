# 📅 AgendamentosAPI - Web API

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white" alt=".NET 10">
  <img src="https://img.shields.io/badge/PostgreSQL-336791?logo=postgresql&logoColor=white" alt="PostgreSQL">
  <img src="https://img.shields.io/badge/Google_Calendar-4285F4?logo=googlecalendar&logoColor=white" alt="Google Calendar">
  <img src="https://img.shields.io/badge/API-REST-009688" alt="REST API">
  <img src="https://img.shields.io/badge/Architecture-Hexagonal-orange" alt="Hexagonal Architecture">
</p>

API REST desenvolvida em .NET 10 para gerenciamento de agendamentos e integração com o Google Calendar. O objetivo é consolidar boas práticas de arquitetura, mantendo o domínio genérico e flexível para diferentes regras de negócio (como clínicas, consultorias ou escritórios).

A aplicação permite gerenciar prestadores de serviço (`ServiceProvider`), clientes (`Customer`) e sincronizar as marcações diretamente em calendários externos, seguindo a Arquitetura Hexagonal para manter o núcleo da aplicação totalmente isolado de bancos de dados e APIs de terceiros.

---

## 🚀 Roadmap e Checklist de Progressão

Nosso desenvolvimento está focado em entregas incrementais (MVP). Acompanhe o progresso:

- [x] **Fase 1: Núcleo e Domínio**
  - [x] Modelagem genérica da entidade `ServiceProvider`.
  - [x] Modelagem genérica da entidade `Customer`.
  - [x] Implementação de Exceptions customizadas (`DomainException`).
- [ ] **Fase 2: Persistência (Próximo Passo)**
  - [ ] Criação das interfaces de repositório (`Ports`).
  - [ ] Configuração do `ApplicationDbContext` (EF Core).
  - [ ] Mapeamento fluente das entidades para o PostgreSQL.
  - [ ] Geração e aplicação das *Migrations*.
- [ ] **Fase 3: Casos de Uso (CRUDs)**
  - [ ] Endpoints para cadastro e gestão de `ServiceProvider`.
  - [ ] Endpoints para cadastro e gestão de `Customer`.
- [ ] **Fase 4: Agendamentos e Integração**
  - [ ] Modelagem da entidade `Appointment` (Agendamento).
  - [ ] Lógica de conflito de horários e regras de negócio.
  - [ ] Implementação do `GoogleCalendarAdapter` com o SDK oficial do Google.
  - [ ] Endpoint de criação de agendamento com sincronização externa.

---

## 🛠️ Tecnologias

- **.NET 10 / C#**
- **PostgreSQL** (via Entity Framework Core 10)
- **Google.Apis.Calendar.v3** (Integração OAuth 2.0 via Service Account)
- **Scalar.AspNetCore** (Documentação OpenAPI)

### Arquitetura e Conceitos

- Ports and Adapters (Arquitetura Hexagonal)
- Domain-Driven Design (Linguagem Ubíqua e Entidades Ricas)
- Princípios SOLID
- Injeção de Dependência
- Programação Defensiva (Guard Clauses)

---

## 🏗️ Estrutura do Projeto

A estrutura abaixo organiza a aplicação garantindo baixo acoplamento, separando estritamente as regras de negócio das ferramentas de infraestrutura.

```text
📁 AgendamentosAPI/
├── 📁 Adapters/
│   ├── 📁 Controllers/ (Endpoints REST)
│   └── 📁 Infrastructure/
│       ├── 📁 Data/ (ApplicationDbContext, Mapeamentos EF Core)
│       ├── 📁 ExternalServices/ (GoogleCalendarAdapter)
│       ├── 📁 Repositories/ (Implementações do EF Core)
│       └── 📄 GlobalExceptionHandler.cs
│
├── 📁 Domain/
│   ├── 📁 Entities/ (ServiceProvider, Customer, Appointment)
│   ├── 📁 Exceptions/ (DomainException)
│   └── 📁 Ports/ (Interfaces de Repositórios e Serviços Externos)
│
├── 📁 Dtos/ (Contratos de entrada e saída)
│
└── 📄 Program.cs
```

---

## ⚙️ Configuração do Ambiente Local (Em Breve)

*As instruções de configuração de banco de dados e injeção da chave `.json` da Service Account do Google Cloud via User Secrets serão detalhadas aqui conforme avançarmos na Fase 2 e 4.*
