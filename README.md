# Paschoalotto Desafio

Este projeto é uma solução completa composta por um backend em .NET e um frontend em Angular, containerizados via Docker.

## Estrutura do Projeto

- **backend/**: RESTful API em .NET 9.
- **frontend/**: Aplicação SPA em Angular.

## Pré-requisitos

- [Docker](https://www.docker.com/get-started) e Docker Compose instalados.
- Para execução local sem Docker:
  
## Como Executar

Na raiz do projeto, execute o comando para construir e subir os containers:

```bash
docker compose up -d --build
```

Acesse os serviços pelas URLs:

 - **Frontend**: [http://localhost:4200](http://localhost:4200)
 - **Backend**: [http://localhost:5000](http://localhost:5000)

Testes automatizados são executados assim:

```
cd frontend; npm test
cd backend; dotnet test
```

## Arquitetura

A arquitetura aplica as heurísticas de DDD, TDD, Clean Architecture, SOLID, DRY, KISS, YAGNI, DI, SoC, CleanCode.

| backend | frontend |
|-|-|
| .NET 9, EntityFramework, Sqlserver, nLog, DDD, xUnit, response caching, soft-delete, server locator, fluent repository, unit of work, server-side pagination (support) | Angular, PrimeNG, signals, vitest, routing, pagination, ordering, stand-alone component, barrel files |

## Observações

Para fins de agilidade em relação ao tempo de build e execução dos serviços no compose , está sendo utilizado um SqlServer in memory, mas está pre-implementado o serviço de SqlServer no compose, com migrations e seeds (comentado e não testado).


