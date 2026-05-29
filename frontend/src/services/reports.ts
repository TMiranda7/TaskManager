import { api } from './api';

export type FrequencyRankingItem = {
  playerId: string;
  playerName: string;
  attendanceCount: number;
};

export type GoalsRankingItem = {
  playerId: string;
  playerName: string;
  goalsCount: number;
};

export async function getFrequencyRanking(): Promise<FrequencyRankingItem[]> {
  return api.get<FrequencyRankingItem[]>('/api/Reports/frequency-ranking', true);
}

export async function getGoalsRanking(): Promise<GoalsRankingItem[]> {
  return api.get<GoalsRankingItem[]>('/api/Reports/goals-ranking', true);
}
