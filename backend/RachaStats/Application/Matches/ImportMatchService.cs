using System.Globalization;
using System.Text.RegularExpressions;
using RachaStats.Application.Common.Exceptions;
using RachaStats.Application.Matches.Requests;
using RachaStats.Application.Matches.Responses;
using RachaStats.Domain.Entities;
using RachaStats.Domain.Repositories;
using Match = RachaStats.Domain.Entities.Match;

namespace RachaStats.Application.Matches;

public class ImportMatchService : IImportMatchService
{
    private readonly IMatchRepository _matchRepository;
    
    public ImportMatchService( IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<MatchDetailResponse?> GetByIdAsync(int matchId)
    {
        var match = await _matchRepository.GetByMatchIdWithAttendancesAsync(matchId);
 
        if (match == null)
            return null;

        var editableUntil = match.CreatedAt.AddHours(24);
        
        return new MatchDetailResponse
        {
            Id = MatchIdHelper.GetJulianMatchId(match.Date),
            Date = match.Date,
            CreatedAt = match.CreatedAt,
            EditableUntil = editableUntil,
            IsEditable = DateTime.UtcNow <= editableUntil,
            Players = match.Attendances.Select(a =>
            {
                var stat = match.MatchStats.FirstOrDefault(s => s.PlayerId == a.PlayerId);

                return new PlayerResponse
                {
                    Id = a.PlayerId,
                    Name = a.Player.Name,
                    InvitedBy = a.InvitedByPlayer != null
                        ? a.InvitedByPlayer.Name
                        : null,
                    Goals = stat?.Goals ?? 0,
                };
            }).ToList()
        };
    }

    public async Task<int> ImportAsync(ImportMatchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RawText))
            throw new ArgumentException("Texto do Whatsapp obrigatório", nameof(request.RawText));
        
        var matchDate = ExtractMatchDate(request.RawText);
        var matchId = MatchIdHelper.GetJulianMatchId(matchDate);
        var existingMatch = await _matchRepository.GetByDateAsync(matchDate);

        if (existingMatch != null)
            throw new BusinessException("Já existe um racha importado para essa data.");

        var players = ExtractPlayer(request.RawText);

        var jogo = new Match
        {
            Id = Guid.NewGuid(),
            Date = matchDate,
            RawText = request.RawText,
            CreatedAt = DateTime.UtcNow
        };

        await _matchRepository.AddMatchAsync(jogo);

        foreach (var player in players)
        {
            var jogadorExiste = await _matchRepository.GetPlayerByNameAsync(player.Name);

            if (jogadorExiste == null)
            {
                jogadorExiste = new Player
                {
                    Id = Guid.NewGuid(),
                    Name = player.Name,
                    CreatedAt = DateTime.UtcNow
                };

                await _matchRepository.AddPlayerAsync(jogadorExiste);
            }
            
            Player? invitedByPlayer = null;

            if (!string.IsNullOrWhiteSpace(player.InvitedBy))
            {
                invitedByPlayer = await _matchRepository.GetPlayerByNameAsync(player.InvitedBy);
                if (invitedByPlayer == null)
                {
                    invitedByPlayer = new Player
                    {
                        Id = Guid.NewGuid(),
                        Name = player.InvitedBy,
                        CreatedAt = DateTime.UtcNow
                    };
                    
                    await _matchRepository.AddPlayerAsync(invitedByPlayer);
                }
            }

            var presenca = new Attendance
            {
                Id = Guid.NewGuid(),
                MatchId = jogo.Id,
                PlayerId = jogadorExiste.Id,
                IsGoalkeeper = player.IsGoalkeeper,
                InvitedByPlayerId = invitedByPlayer?.Id
            };
            await _matchRepository.AddAttendanceAsync(presenca);
        }
        
        await _matchRepository.SaveChangesAsync();
        
        return matchId;
    }

    public async Task RegisterGoalsAsync(int matchId, RegisterGoalRequest request)
    {
        if (request.Goals < 0)
            throw new BusinessException("A quantidade de gols não pode ser negativa.");

        if (request.MatchId != matchId)
            throw new BusinessException("matchId da rota e do payload devem ser iguais.");

        var match = await _matchRepository.GetByMatchIdAsync(matchId);

        if (match == null)
            throw new NotFoundException("Racha não encontrado.");

        if (DateTime.UtcNow > match.CreatedAt.AddHours(24))
            throw new BusinessException("A lista do último racha só pode ser editada até 24 horas após a importação.");

        var player = await _matchRepository.GetPlayerByIdAsync(request.PlayerId);

        if (player == null)
            throw new NotFoundException("Jogador não encontrado.");

        var attendance =  await _matchRepository.GetAttendanceAsync(match.Id, request.PlayerId);

        if (attendance == null)
            throw new BusinessException("Esse jogador não está presente nesse racha.");

        var stat = await _matchRepository.GetMatchStatAsync(match.Id, request.PlayerId);

        if (stat == null)
        {
            stat = new MatchStat
            {
                Id = Guid.NewGuid(),
                MatchId = match.Id,
                PlayerId = request.PlayerId,
                Goals = request.Goals,
                IsHighlight = false
            };

            await _matchRepository.AddMatchStatAsync(stat);
        }
        else
        {
            stat.Goals = request.Goals;
        }

        await _matchRepository.SaveChangesAsync();
    }

    private DateTime ExtractMatchDate(string rawText)
    {
        var match = Regex.Match(rawText, @"(?<!\d)(\d{1,2})[\/.-](\d{1,2})[\/.-](\d{2,4})(?!\d)");

        if (!match.Success)
            throw new BusinessException("Informe a data do racha no arquivo importado.");

        var day = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var month = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        var year = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

        if (year < 100)
            year += 2000;

        try
        {
            return new DateTime(year, month, day);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new BusinessException("A data do racha informada no arquivo é inválida.");
        }
    }

    private List<ImportedPlayer> ExtractPlayer(string rawText)
    {
        var list = new List<ImportedPlayer>();
        var importedPlayerNames = new HashSet<string>();
        
        var pattern = @"\d+\s*-\s*([^\(\n]+)(?:\(([^)]+)\))?";
        
        var lines = rawText.Split('\n');
        
        foreach (var line in lines)
        {
            var players = Regex.Match(line, pattern);

            if (!players.Success)
                continue;

            string name = PlayerNameNormalizer.Normalize(players.Groups[1].Value);

            string? invitedBy = players.Groups[2].Success
                ? PlayerNameNormalizer.Normalize(players.Groups[2].Value)
                : null;

            if (string.IsNullOrWhiteSpace(name))
                continue;

            if (string.IsNullOrWhiteSpace(invitedBy))
                invitedBy = null;

            if (!importedPlayerNames.Add(name))
                continue;
            
            list.Add(new ImportedPlayer
            {
                Name = name,
                InvitedBy = invitedBy,
                IsGoalkeeper = false
            });
        }
        
        return list;
    }
}
