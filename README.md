# VDM.Pastelaria

## Características do projeto:

- .NET 6.0
- Framework de Testes: xUnit
- Framework de Assertions: FluentAssertions
- Framework de Mock: NSubstitute
- Framework de fake data generator: Bogus
- Code Analyzer: FxCopAnalyzers, SonarAnalyzer.CSharp, StyleCop.Analyzers
- Projeto para testes de Unidade
- Controllers e Actions atendendo os padrões RESTFul
- Controller LogLevelSwith com ajuste de nível de log em tempo de execução
- Utilização do APIAnalyzer e APIConventions para padronização e documentação de API
- Tratamento de Warning como Error
- Healthcheck
    - Liveness
    - Readyness
- Log com Serilog
- Propagação de header X-Correlation-Id para dependencias usando Refit
- Cake Build

## Setup

* Quais são as ferramentas necessárias
    - Visual Studio, .NET Core SDK 6.0+ etc

## Políticas de Código 

* As regras estão definidas no editorconfig

### Como calcular cobertura dos testes

```powershell
dotnet tool restore
dotnet dotnet-cake --target=Coverage
```

O passo do `dotnet tool restore` só precisa ser feito uma única fez na sua máquina

Para visualizar a cobertura de forma simples, você pode rodar esse comando powershell
```powershell
cat .\coverageOutput\Summary.txt -Head 11
```

Ou então abrir o arquivo
```
coverageOutput\index.html
```

### Como executar os testes de mutação

```powershell
dotnet tool restore
dotnet dotnet-stryker
```

O passo do `dotnet tool restore` só precisa ser feito uma única fez na sua máquina