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

## Fluxo do app

- Login com JWT e refresh automático da sessao.
- Importacao de racha via texto do WhatsApp.
- Dashboard com ranking de presenca e ranking de gols.

## Configuracao de API no frontend

Arquivo: `frontend/env.ts`

- **Celular fisico (Expo Go):** o app detecta automaticamente o IP do computador pela conexao com o Metro. Celular e PC precisam estar na mesma rede Wi-Fi, e o backend deve estar rodando (`dotnet run`).
- **iOS Simulator / Web:** `http://localhost:5094`
- **Android Emulator:** `http://10.0.2.2:5094`
- **Override manual:** crie `frontend/.env` com `EXPO_PUBLIC_API_URL=http://SEU_IP:5094` e reinicie o Expo (`npm run start`).
