# Lógica da solução — Listagem de horários disponíveis

## 1. Receber a requisição

A requisição deve conter:

- id do prestador;
- período da pesquisa, representado por uma data de início e uma data de fim;
- duração desejada para o agendamento.

O período da pesquisa representa um **intervalo de dias**, e não um intervalo arbitrário de data e hora.

---

## 2. Consultar o prestador no repositório

O prestador é consultado no repositório, obtendo os dados necessários para o cálculo, por exemplo:

```text
string name
string? email
TimeOnly workStartTime
TimeOnly workEndTime
string? calendarId
```

O `TimeOnly` representa apenas o horário do início e fim do expediente.

---

## 3. Consultar os períodos ocupados

A função responsável pelo calendário é acionada:

```text
GetBusyPeriodsAsync(
    string calendarId,
    DateTimeOffset start,
    DateTimeOffset end
)
```

Ela retorna uma coleção de períodos ocupados representados por uma abstração semelhante a:

```text
TimePeriod(
    DateTimeOffset Start,
    DateTimeOffset End
)
```

Um período ocupado representa qualquer intervalo em que o prestador não possa receber um novo agendamento. Isso pode incluir compromissos, folgas, bloqueios ou outros períodos de indisponibilidade.

Neste MVP, será assumido que os períodos retornados são válidos e não possuem sobreposição. Caso seja necessário garantir essa condição, o tratamento poderá ser feito na implementação responsável por fornecer os períodos ocupados.

---

## 4. Percorrer o período da pesquisa dia a dia

O período da pesquisa é percorrido dia a dia.

Cada dia funciona como uma **data de referência** para a construção do expediente daquele dia.

---

## 5. Construir o intervalo de expediente do dia

O expediente armazenado no prestador utiliza `TimeOnly`, mas o cálculo da disponibilidade deverá utilizar um intervalo temporal concreto.

Para um expediente diurno:

```text
08:00 → 18:00
```

no dia 02/09/2026:

```text
02/09/2026 08:00 → 02/09/2026 18:00
```

Para um expediente que atravessa a meia-noite:

```text
23:30 → 06:00
```

no dia 02/09/2026:

```text
02/09/2026 23:30 → 03/09/2026 06:00
```

O turno noturno deve ser tratado como **um único intervalo temporal contínuo**. A meia-noite não deve interromper o cálculo.

Dessa forma, a informação de que o intervalo atravessou a meia-noite já está contida nas próprias datas de `Start` e `End`.

---

## 6. Identificar os períodos ocupados que interferem no expediente

Devem ser considerados os períodos ocupados que possuam interseção com o intervalo de expediente construído.

Isso é importante para expedientes que atravessam a meia-noite.

Exemplo:

```text
Expediente:
02/09/2026 23:30 → 03/09/2026 06:00

Ocupação:
03/09/2026 00:30 → 03/09/2026 01:30
```

A ocupação deve ser considerada, mesmo pertencendo ao dia seguinte em termos de calendário, porque interfere no intervalo de trabalho iniciado em 02/09.

---

## 7. Ordenar os períodos ocupados

Os períodos ocupados devem ser ordenados cronologicamente pelo início antes da varredura.

Será assumido neste MVP que não existem períodos ocupados sobrepostos.

---

## 8. Instanciar a coleção de intervalos livres

Deve ser criada uma coleção para armazenar os `TimePeriods` livres encontrados durante o cálculo.

---

## 9. Percorrer os períodos ocupados e identificar as lacunas

O algoritmo deve procurar os intervalos existentes entre o início e o fim do expediente e os períodos ocupados.

Devem ser analisadas, sucessivamente:

- início do expediente → início da primeira ocupação;
- fim de uma ocupação → início da próxima ocupação;
- fim da última ocupação → fim do expediente.

Cada uma dessas lacunas representa um possível intervalo livre.

---

## 10. Calcular a duração de cada intervalo livre

Para cada intervalo encontrado, deve ser calculada sua duração:

```text
End - Start
```

A duração do intervalo livre será comparada com a duração solicitada para o novo agendamento.

A regra é:

```text
duração do intervalo livre >= duração desejada
```

Um intervalo cuja duração seja exatamente igual à duração solicitada também é válido.

---

## 11. Adicionar os intervalos livres válidos

Somente os intervalos livres que comportem o agendamento devem ser adicionados à coleção.

O intervalo deve permanecer **inteiro no resultado**, não sendo dividido em slots.

Exemplo:

```text
Expediente:
08:00 → 16:00

Ocupações:
11:00 → 11:40
12:00 → 13:30

Duração desejada:
30 minutos
```

Resultado:

```text
08:00 → 11:00
13:30 → 16:00
```

O intervalo:

```text
11:40 → 12:00
```

não deve ser retornado, pois possui apenas 20 minutos.

---

## 12. Agrupar os resultados por dia

Os intervalos encontrados devem ser agrupados por **data de referência** para formar a resposta.

O agrupamento por dia não deve interferir no cálculo dos intervalos.

Em especial, um `TimePeriod` pode atravessar a meia-noite e continuar sendo considerado pertencente ao grupo do dia em que seu intervalo foi iniciado.

Por exemplo:

```text
Data de referência:
02/09/2026

TimePeriod:
02/09/2026 23:30 → 03/09/2026 01:00
```

Nesse caso, o período pode ser associado ao grupo de:

```text
02/09/2026
```

mas seus campos `Start` e `End` continuam contendo **data e hora completas**, preservando a informação da mudança de dia.

Exemplo de retorno:

```json
{
  "Days": [
    {
      "Date": "2026-09-02",
      "TimePeriods": [
        {
          "Start": "2026-09-02T23:30:00-03:00",
          "End": "2026-09-03T01:00:00-03:00"
        }
      ]
    }
  ]
}
```

Assim, `Date` funciona como **data de referência do grupo**, enquanto `Start` e `End` representam o intervalo temporal real.

---

## 13. Retornar a coleção

A função retorna a coleção de dias com seus respectivos períodos livres.

Estrutura conceitual:

```text
Days
  ├── Date
  └── TimePeriods
        ├── Start
        └── End
```

Os `TimePeriods` representam intervalos contínuos e podem atravessar a meia-noite.

---

# Princípio central da lógica

A regra principal pode ser resumida como:

```text
Intervalo de expediente
        ↓
identificar ocupações que interferem nele
        ↓
ordenar ocupações
        ↓
encontrar lacunas
        ↓
calcular duração de cada lacuna
        ↓
manter apenas lacunas cuja duração seja
maior ou igual à duração solicitada
        ↓
agrupar resultado pela data de referência
```

O cálculo deve ser realizado utilizando **intervalos temporais contínuos**.

A divisão por dias existe para organizar a consulta e a resposta, mas não deve criar uma fronteira artificial para intervalos que atravessam a meia-noite.