using System.Text.RegularExpressions;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Matches.Requests;
using TaskManager.Application.Matches.Responses;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using Match = TaskManager.Domain.Entities.Match;

namespace TaskManager.Application.Matches;

public class ImportMatchService : IImportMatchService
{
    private readonly IMatchRepository _matchRepository;
    
    public ImportMatchService( IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<MatchDetailResponse?> GetByIdAsync(Guid id)
    {
        var match = await _matchRepository.GetByIdWithAttendancesAsync(id); 
 
        if (match == null)
            return null;
        
        return new MatchDetailResponse
        {
            Id = match.Id,
            Date = match.Date,
            Players = match.Attendances.Select(a => new PlayerResponse
            {
                Name = a.Player.Name,
                InvitedBy = a.InvitedByPlayer != null
                    ? a.InvitedByPlayer.Name
                    : null
            }).ToList()
        };
    }

    public async Task<Guid> ImportAsync(ImportMatchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RawText))
            throw new ArgumentException("Texto do Whatsapp obrigatório", nameof(request.RawText));
        
        var players = ExtractPlayer(request.RawText);

        var jogo = new Match
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now,
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
        
        return jogo.Id ;
    }

    public async Task RegisterGoalsAsync(Guid matchId, RegisterGoalRequest request)
    {
        if (request.Goals < 0)
            throw new BusinessException("A quantidade de gols não pode ser negativa.");

        var match = await _matchRepository.GetByIdAsync(matchId);

        if (match == null)
            throw new NotFoundException("Racha não encontrado.");

        var player = await _matchRepository.GetPlayerByIdAsync(request.PlayerId);

        if (player == null)
            throw new NotFoundException("Jogador não encontrado.");

        var attendance =  await _matchRepository.GetAttendanceAsync(matchId, request.PlayerId);

        if (attendance == null)
            throw new BusinessException("Esse jogador não está presente nesse racha.");

        var stat = await _matchRepository.GetMatchStatAsync(matchId, request.PlayerId);

        if (stat == null)
        {
            stat = new MatchStat
            {
                Id = Guid.NewGuid(),
                MatchId = matchId,
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

    private List<ImportedPlayer> ExtractPlayer(string rawText)
    {
        var list = new List<ImportedPlayer>();       
        
        var pattern = @"\d+\s*-\s*([^\(\n]+)(?:\(([^)]+)\))?";
        
        var lines = rawText.Split('\n');
        
        foreach (var line in lines)
        {
            var players = Regex.Match(line, pattern);

            if (!players.Success)
                continue;

            string name = players.Groups[1].Value.Trim();

            string? invitedBy = players.Groups[2].Success ? players.Groups[2].Value.Trim() : null;

            if (string.IsNullOrWhiteSpace(name))
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