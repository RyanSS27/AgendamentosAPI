# Guia de Consulta Rápida: Tipos de Data e Hora no .NET

Documento de referência rápida sobre a aplicação e funcionamento das estruturas de data e hora do .NET no contexto do projeto de **Disponibilidade de Agenda**.

---

## 1. Visão Geral Comparativa

| Tipo | O que representa? | Analogia | Exemplo no Projeto |
| :--- | :--- | :--- | :--- |
| **`DateOnly`** | Data pura (ano, mês, dia) | Calendário de parede | Data de referência da busca (`2026-09-02`) |
| **`TimeOnly`** | Horário puro (hora, minuto) | Relógio de parede | Início/fim de expediente (`08:00`, `23:30`) |
| **`TimeSpan`** | Duração ou quantidade de tempo | Cronômetro | Duração do agendamento (30 min) e fuso/offset |
| **`DateTimeOffset`** | Instante exato no tempo global | Marcador na linha do tempo universal | `Start` e `End` dos agendamentos e ocupações |

---

## 2. Detalhamento Técnico e Operações Básicas

### A. `DateOnly`
Representa um dia específico sem conceito de horário ou fuso.

* **Instanciação:**
  ```csharp
  var data = new DateOnly(2026, 9, 2);
  ```
* **Avançar/recuar dias:**
  ```csharp
  var proximoDia = data.AddDays(1); // Retorna 2026-09-03
  ```
* **Fusão com horário (`ToDateTime`):**
  Combina a data com um `TimeOnly` para gerar um `DateTime` local.
  ```csharp
  TimeOnly hora = new TimeOnly(8, 0);
  DateTime dataEHoraLocal = data.ToDateTime(hora); 
  ```

---

### B. `TimeOnly`
Representa uma posição fixa no relógio dentro do ciclo de 24 horas.

* **Instanciação:**
  ```csharp
  var inicio = new TimeOnly(8, 0);   // 08:00
  var fim = new TimeOnly(23, 30);    // 23:30
  ```
* **Comparação nativa:**
  ```csharp
  bool ehAnterior = inicio < fim; // true
  ```
* **Detecção de turno noturno (virada de noite):**
  ```csharp
  // Se o horário de término for menor que o de início, o turno atravessa a meia-noite.
  bool ehTurnoNoturno = workEnd < workStart; 
  ```

---

### C. `TimeSpan`
Mede uma quantidade/duração de tempo decorrido.

* **Instanciação por unidade:**
  ```csharp
  TimeSpan duracaoDesejada = TimeSpan.FromMinutes(30);
  ```
* **Diferença entre `.TotalMinutes` e `.Minutes` (Atenção):**
  * `TotalMinutes`: Retorna a duração inteira convertida em minutos (ex: 1h30m -> `90.0`). **[Usar para comparações]**
  * `Minutes`: Retorna apenas o componente de minutos do intervalo (ex: 1h30m -> `30`).

* **Cálculo da duração de uma lacuna:**
  ```csharp
  TimeSpan duracaoIntervalo = fimLacuna - inicioLacuna;
  bool cabeAgendamento = duracaoIntervalo.TotalMinutes >= duracaoDesejada.TotalMinutes;
  ```

---

### D. `DateTimeOffset`
Guarda um instante preciso no tempo juntamente com sua distância (Offset) do UTC.

* **Instanciação unindo peças:**
  ```csharp
  DateOnly data = new DateOnly(2026, 9, 2);
  TimeOnly hora = new TimeOnly(23, 30);
  TimeSpan offset = TimeSpan.Zero; // UTC

  DateTimeOffset instante = new DateTimeOffset(data.ToDateTime(hora), offset);
  ```
* **Operações matemáticas com `TimeSpan`:**
  ```csharp
  DateTimeOffset terminoAgendamento = instante.Add(TimeSpan.FromMinutes(30));
  ```
* **Comparação direta:**
  Dois `DateTimeOffset` com offsets diferentes são comparados pelo instante absoluto no UTC.
  ```csharp
  bool conflito = inicioOcupacao < fimExpediente;
  ```

---

## 3. Padrão de Construção de Intervalos de Expediente

Algoritmo-base para construção dos limites reais do expediente (`shiftStart` e `shiftEnd`) tratando a regra da meia-noite:

```csharp
DateOnly currentDate = new DateOnly(2026, 9, 2);
TimeOnly workStart = provider.WorkStartTime; // ex: 23:30
TimeOnly workEnd = provider.WorkEndTime;     // ex: 06:00
TimeSpan offset = TimeSpan.Zero;             // Regra da API em UTC

// 1. Início do expediente no dia de referência
DateTime dtStart = currentDate.ToDateTime(workStart);
DateTimeOffset shiftStart = new DateTimeOffset(dtStart, offset);

// 2. Determinação do dia de término (se atravessa a meia-noite, soma 1 dia)
DateOnly endDate = workEnd < workStart ? currentDate.AddDays(1) : currentDate;

// 3. Término do expediente no dia correspondente
DateTime dtEnd = endDate.ToDateTime(workEnd);
DateTimeOffset shiftEnd = new DateTimeOffset(dtEnd, offset);

// 4. Instância do período contínuo
var expedienteContinuo = new TimePeriod(shiftStart, shiftEnd);
```
