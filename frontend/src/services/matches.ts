import { api } from './api';

export type JogadorFormPlayer = {
  id: string;
  name: string;
  goals: number;
  hasInfraction: boolean;
};

export type MatchDetailPlayer = {
  id: string;
  name: string;
  invitedBy?: string | null;
  goals: number;
};

export type MatchDetail = {
  id: number;
  date: string;
  createdAt: string;
  editableUntil: string;
  isEditable: boolean;
  players: MatchDetailPlayer[];
};

export type RegisterMatchGoalsRequest = {
  matchId: number;
  playerId: string;
  goals: number;
};

type MatchDetailPlayerApi = {
  id?: string;
  Id?: string;
  name?: string;
  Name?: string;
  invitedBy?: string | null;
  InvitedBy?: string | null;
  goals?: number;
  Goals?: number;
};

type MatchDetailApiResponse = {
  id?: number;
  Id?: number;
  date?: string;
  Date?: string;
  createdAt?: string;
  CreatedAt?: string;
  editableUntil?: string;
  EditableUntil?: string;
  isEditable?: boolean;
  IsEditable?: boolean;
  players?: MatchDetailPlayerApi[];
  Players?: MatchDetailPlayerApi[];
};

function mapPlayer(player: MatchDetailPlayerApi): MatchDetailPlayer {
  return {
    id: player.id ?? player.Id ?? '',
    name: player.name ?? player.Name ?? '',
    invitedBy: player.invitedBy ?? player.InvitedBy ?? null,
    goals: player.goals ?? player.Goals ?? 0,
  };
}

export async function importMatchFromWhatsapp(rawText: string): Promise<{ id: number }> {
  return api.post<{ id: number }>('/api/Matches/import-whatsapp', { rawText }, true);
}

export async function getMatchById(matchId: number): Promise<MatchDetail> {
  const data = await api.get<MatchDetailApiResponse>(`/api/Matches/${matchId}`, true);
  const players = data.players ?? data.Players ?? [];
  const createdAt = data.createdAt ?? data.CreatedAt ?? new Date().toISOString();
  const editableUntil = data.editableUntil ?? data.EditableUntil ?? createdAt;

  return {
    id: data.id ?? data.Id ?? matchId,
    date: data.date ?? data.Date ?? new Date().toISOString(),
    createdAt,
    editableUntil,
    isEditable: data.isEditable ?? data.IsEditable ?? false,
    players: players.map(mapPlayer),
  };
}

export async function registerMatchGoals(
  matchId: number,
  payload: RegisterMatchGoalsRequest,
): Promise<void> {
  await api.post(`/api/Matches/${matchId}/goals`, payload, true);
}
