# RachaStats

Projeto dividido em:

- `backend`: API .NET focada na gestao do racha
- `frontend`: app Expo (iOS, Android e Web)

## Backend

```bash
cd backend
dotnet restore RachaStats.sln
dotnet run --project RachaStats/RachaStats.csproj
```

API local: `http://localhost:5094`

Configuracao de usuarios para login:

- O login agora consulta a tabela `AppUsers` no banco de dados
- A senha enviada no login continua no campo `password`
- Antes de comparar, a API gera `SHA256` e compara com `PasswordHash`
- Desenvolvimento: usuarios iniciais podem ser configurados em `backend/RachaStats/appsettings.Development.json` na secao `Auth:SeedUsers`
- Producao: usuarios iniciais podem ser configurados temporariamente com `Auth__SeedUsers__0__Username`, `Auth__SeedUsers__0__Password` e `Auth__SeedUsers__0__Role`
- O bootstrap aplica as migrations pendentes e insere os usuarios iniciais apenas se o `Username` ainda nao existir

Para criar a estrutura manualmente com EF:

```bash
cd backend/RachaStats
dotnet ef database update
```

Endpoints principais:

- `POST /api/Auth/login`
- `POST /api/Auth/refresh`
- `POST /api/Matches/import-whatsapp`
- `GET /api/Matches/{id}`
- `POST /api/Matches/{matchId}/goals`
- `GET /api/Reports/frequency-ranking`
- `GET /api/Reports/goals-ranking`

## Frontend (Expo)

```bash
cd frontend
npm install
npm run start
```

Atalhos úteis:

- `npm run ios`
- `npm run android`
- `npm run web`

## Ambiente publicado

API publicada:

- URL da API: `https://SEU-APP-SERVICE.azurewebsites.net`
- Swagger: `https://SEU-APP-SERVICE.azurewebsites.net/swagger`

Configuracao do frontend para producao:

- A URL de producao esta centralizada no codigo frontend: `https://hashstats-api-dev.azurewebsites.net`
- Reinicie o Expo apos alterar a variavel: `npm run start`
- Para trocar o endpoint publicado no futuro, altere `PRODUCTION_API_BASE_URL` em `frontend/env.ts`

## Fluxo do app

- Login com JWT e refresh automático da sessao.
- Importacao de racha via texto do WhatsApp.
- Dashboard com ranking de presenca e ranking de gols.

## Configuracao de API no frontend

Arquivo: `frontend/env.ts`

- Desenvolvimento volta a usar a logica anterior para backend local
- Expo Go tenta descobrir o IP da maquina automaticamente
- Android Emulator usa `http://10.0.2.2:5094`
- iOS Simulator e Web usam `http://localhost:5094`
- Producao usa `PRODUCTION_API_BASE_URL` fixo no codigo
