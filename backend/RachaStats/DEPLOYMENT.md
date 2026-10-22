# Expo Web + ASP.NET Core no App Service

O workflow `.github/workflows/primeiroRacha_hashstats-api-dev.yml` exporta o Expo Web,
publica a API e copia `frontend/dist/.` para `artifacts/publish/wwwroot/`.
O mesmo diretorio e enviado ao App Service existente `hashstats-api-dev`.
PR valida o pacote; deploy ocorre somente na main em push ou disparo manual.
A web usa API same-origin; Android/iOS mantem a URL HTTPS absoluta.

## Inicializacao do banco

As duas opcoes abaixo sao independentes e false quando ausentes. O arquivo
`appsettings.Production.json` tambem declara ambas como false. Em outros ambientes,
a inicializacao agora exige opt-in explicito.

| Configuracao | Variavel de ambiente | Efeito quando true |
| --- | --- | --- |
| DatabaseInitialization:ApplyMigrations | DatabaseInitialization__ApplyMigrations | Aplica migrations antes do seed |
| DatabaseInitialization:SeedUsers | DatabaseInitialization__SeedUsers | Insere usuarios configurados em Auth:SeedUsers que ainda nao existem |

Com ambas false, a inicializacao nao resolve o DbContext nem conecta ao banco.
O seed nao aplica migrations. Com seed habilitado e sem usuarios configurados,
nenhum DbContext e resolvido. Com apenas seed habilitado, o schema deve existir.
Falhas de migrations ou seed habilitados interrompem a inicializacao.

Para uma execucao intencional, configure apenas as opcoes necessarias como true
em um ambiente controlado, com banco e permissoes revisados. Remova os overrides
ou retorne ambos a false antes da inicializacao normal. As opcoes nao sao uma
barreira contra administradores que as habilitem explicitamente em producao.
`dotnet ef database update` continua sendo uma operacao administrativa explicita,
independente desses controles; nao faz parte do workflow de build.

## Configuracao Azure a revisar antes de publicar

Manter a conexao atual via `ConnectionStrings__DefaultConnection` no App Service,
e JWT e eventuais credenciais de seed em configuracao segura, nunca no frontend
ou em logs. Nenhum recurso novo e necessario. Confirmar ambiente Production,
runtime .NET 10 e ausencia de overrides true para as duas opcoes.
O publish profile existente precisa estar disponivel no GitHub; o workflow
falha explicitamente nos eventos de deploy caso o secret esteja vazio. PR nao
exige esse secret. O workflow legado de Static Web Apps
esta desativado.

Revisar arquivos de configuracao versionados e dados sensiveis antes de aprovar
o pacote. Nao ativar seed com usuarios de desenvolvimento em producao.
As opcoes de inicializacao nao alteram CORS, JWT, controllers ou migrations.
Controllers que acessam dados ainda precisam da conexao Azure SQL operacional.

## Validacao local segura

Builds e exportacoes nao iniciam a API:

```bash
npm --prefix frontend run build:web
dotnet build RachaStats.slnx --configuration Release
dotnet publish backend/RachaStats/RachaStats.csproj -c Release -o /tmp/rachastats-publish
```

Copiar o conteudo de dist para wwwroot do pacote, como faz o workflow. Qualquer
teste que inicie a API deve substituir a conexao por um destino local isolado
e manter ambas as opcoes false. Nao iniciar usando a conexao Azure para testar.
Validar raiz, deep links, API autenticada, Swagger e 404 de API/assets antes do
deploy autorizado. Nenhum teste local substitui a validacao pos-deploy.

## Segredos e desenvolvimento local

Valores removidos dos JSONs rastreados foram preservados em
`appsettings.Local.json`, ignorado pelo Git, com permissoes locais restritas.
Somente Development carrega esse arquivo; ambiente e argumentos de linha de
comando prevalecem. A factory do EF segue a mesma regra e usa Production quando
nenhum ambiente e informado. Para ferramentas locais, definir explicitamente
`DOTNET_ENVIRONMENT=Development`. Nao executar migrations como validacao.
Em outra maquina, criar o arquivo local de forma segura ou fornecer variaveis
de ambiente; nenhuma credencial de exemplo foi adicionada ao repositorio.

No Azure App Service configurar antes da publicacao:

| Nome | Conteudo |
| --- | --- |
| ASPNETCORE_ENVIRONMENT | Production |
| ConnectionStrings__DefaultConnection | Conexao segura ao Azure SQL atual |
| Jwt__Key | Nova chave aleatoria forte, ao menos 32 bytes para HS256 |
| Jwt__Issuer | Emissor esperado, consistente com a API |
| Jwt__Audience | Audience esperada, consistente com os clientes |
| DatabaseInitialization__ApplyMigrations | false |
| DatabaseInitialization__SeedUsers | false |

Nao configurar usuarios de seed em producao para a publicacao normal. Caso seja
necessario seed intencional, fornecer `Auth__SeedUsers__0__Username`,
`Auth__SeedUsers__0__Password` e `Auth__SeedUsers__0__Role` via configuracao segura,
habilitar somente a opcao de seed no ambiente controlado e remover os valores
apos a operacao autorizada. Nenhuma dessas operacoes foi executada aqui.
Manter segredos fora de EXPO_PUBLIC e dos workflows. Publish profile fica somente
no GitHub Secret ja referenciado pelo workflow.

Os valores anteriormente versionados devem ser tratados como expostos. Planejar
rotacao coordenada da senha SQL, chave JWT, credenciais de seed e credenciais de
publicacao caso presentes em artefatos antigos. Rotacao de JWT invalida tokens
existentes; usuarios ja inseridos no banco nao tem senha alterada por editar JSON.
Nada foi rotacionado automaticamente. Limpar o working tree nao remove segredos
do historico Git ou de artefatos previamente publicados.

`publish/` e ZIPs legados ainda sao rastreados apesar do novo .gitignore. Sua
remocao do controle de versao e eventual limpeza de historico precisam de revisao
separada; nao foram removidos nesta tarefa. MSBuild exclui esses artefatos e
appsettings.Local/Development do novo pacote de publicacao.
