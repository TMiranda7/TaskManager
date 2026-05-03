using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Matches.Requests;
using TaskManager.Application.Matches.Responses;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;
using Match = TaskManager.Domain.Entities.Match;
using MatchExpression = System.Text.RegularExpressions.Match;

namespace TaskManager.Application.Matches;

public class ImportMatchService : IImportMatchService
{
    private readonly AppDbContext _context;
    
    public ImportMatchService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MatchDetailResponse?> GetByIdAsync(Guid id)
    {
        var match = await _context.Matches
            .Include(m => m.Attendances).ThenInclude(a => a.Player)
            .Include(m => m.Attendances).ThenInclude(a => a.InvitedByPlayer)
            .FirstOrDefaultAsync(m => m.Id == id);  
 
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
        
        var players = ExtratedPlayer(request.RawText);

        var jogo = new Match
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now,
            RawText = request.RawText,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Matches.Add(jogo);

        foreach (var player in players)
        {
            var jogadorExiste = _context.Players.FirstOrDefault(x => x.Name.ToLower() == player.Name.ToLower());

            if (jogadorExiste == null)
            {
                jogadorExiste = new Player
                {
                    Id = new Guid(),
                    Name = player.Name,
                    CreatedAt = DateTime.UtcNow
                };
                
                _context.Players.Add(jogadorExiste);
            }
            
            Player? invitedByPlayer = null;

            if (!string.IsNullOrWhiteSpace(player.InvitedBy))
            {
                invitedByPlayer = await _context.Players.FirstOrDefaultAsync(x => x.Name == player.InvitedBy);
                if (invitedByPlayer == null)
                {
                    invitedByPlayer = new Player
                    {
                        Id = new Guid(),
                        Name = player.InvitedBy,
                        CreatedAt = DateTime.UtcNow
                    };
                    
                    _context.Players.Add(invitedByPlayer);
                }
            }

            var presenca = new Attendance
            {
                Id = Guid.NewGuid(),
                MatchId = jogo.Id,
                PlayerId = jogadorExiste.Id,
                IsGoalkeeper = player.IsGoalkeeper
            };
            _context.Attendances.Add(presenca);
        }
        
        await _context.SaveChangesAsync();
        
        return jogo.Id ;
    }

    public async Task RegisterGoalsAsync(Guid matchId, RegisterGoalRequest request)
    {
        if (request.Goals < 0)
            throw new BusinessException("A quantidade de gols não pode ser negativa.");

        var match = await _context.Matches
            .FirstOrDefaultAsync(x => x.Id == matchId);

        if (match == null)
            throw new NotFoundException("Racha não encontrado.");

        var player = await _context.Players
            .FirstOrDefaultAsync(x => x.Id == request.PlayerId);

        if (player == null)
            throw new NotFoundException("Jogador não encontrado.");

        var attendance = await _context.Attendances
            .FirstOrDefaultAsync(x => x.MatchId == matchId && x.PlayerId == request.PlayerId);

        if (attendance == null)
            throw new BusinessException("Esse jogador não está presente nesse racha.");

        var stat = await _context.MatchStats
            .FirstOrDefaultAsync(x => x.MatchId == matchId && x.PlayerId == request.PlayerId);

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

            _context.MatchStats.Add(stat);
        }
        else
        {
            stat.Goals = request.Goals;
        }

        await _context.SaveChangesAsync();
    }

    private List<ImportedPlayer> ExtratedPlayer(string rawText)
    {
        var list = new List<ImportedPlayer>();       
        
        var pattern = @"\d+\s*-\s*([^\(\n]+)(?:\(([^)]+)\))?";
        
        var matches =Regex.Matches(rawText, pattern);

        foreach (MatchExpression match in matches)
        {
            var name = match.Groups[1].Value.Trim().ToLower();
            var invitedBy = match.Groups[2].Success
                ? match.Groups[2].Value.Trim().ToLower()
                : null;

            if (!string.IsNullOrWhiteSpace(name))
            {
                list.Add(new ImportedPlayer
                {
                    Name = name,
                    InvitedBy = invitedBy,
                    IsGoalkeeper = false
                });
            }
        }
        
        return list;
    }
}