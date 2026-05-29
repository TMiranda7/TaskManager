import { useEffect, useState } from 'react';
import {
  ActivityIndicator,
  Pressable,
  RefreshControl,
  ScrollView,
  Text,
  TextInput,
  View,
} from 'react-native';
import { router } from 'expo-router';
import { useAuth } from '../src/auth/AuthContext';
import { AccordionSection } from '@/src/components/AccordionSection';
import { Artilharia } from '@/src/components/Artilharia';
import { Frequencia } from '@/src/components/Frequencia';
import { ListaUltimoRacha } from '@/src/components/ListaUltimoRacha';
import { RankingPresenca } from '@/src/components/RankingPresenca';
import { getFrequencyRanking, getGoalsRanking, FrequencyRankingItem, GoalsRankingItem } from '../src/services/reports';
import { getMatchById, importMatchFromWhatsapp, MatchDetail } from '../src/services/matches';
import { colors } from '@/styles/theme';
import { styles } from '@/styles/home.styles';

export default function HomeScreen() {
  const { token, signOut } = useAuth();
  const [frequencyRanking, setFrequencyRanking] = useState<FrequencyRankingItem[]>([]);
  const [goalsRanking, setGoalsRanking] = useState<GoalsRankingItem[]>([]);
  const [lastImportedMatch, setLastImportedMatch] = useState<MatchDetail | null>(null);
  const [rawText, setRawText] = useState('');
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [importing, setImporting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadDashboard = async (silent = false) => {
    try {
      if (!silent) {
        setLoading(true);
      }

      const [frequency, goals] = await Promise.all([getFrequencyRanking(), getGoalsRanking()]);
      setFrequencyRanking(frequency);
      setGoalsRanking(goals);
      setError(null);
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Falha ao carregar dados do racha.';
      setError(message);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useEffect(() => {
    if (!token) {
      router.replace('/login');
      return;
    }

    loadDashboard();
  }, [token]);

  const handleRefresh = async () => {
    setRefreshing(true);
    await loadDashboard(true);
  };

  const handleSignOut = async () => {
    await signOut();
    router.replace('/login');
  };

  const handleStatsSaved = async () => {
    try {
      if (lastImportedMatch) {
        const detail = await getMatchById(lastImportedMatch.id);
        setLastImportedMatch(detail);
      }

      await loadDashboard(true);
      setError(null);
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Falha ao atualizar estatisticas.';
      setError(message);
    }
  };

  const handleImport = async () => {
    if (!rawText.trim()) {
      setError('Cole o texto do WhatsApp para importar o racha.');
      return;
    }

    try {
      setImporting(true);
      const imported = await importMatchFromWhatsapp(rawText);
      const detail = await getMatchById(imported.id);
      setLastImportedMatch(detail);
      setRawText('');
      await loadDashboard(true);
      setError(null);
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Falha ao importar racha.';
      setError(message);
    } finally {
      setImporting(false);
    }
  };

  if (loading) {
    return (
      <View style={styles.centerContainer}>
        <ActivityIndicator size="large" color={colors.textPrimary} />
      </View>
    );
  }

  return (
    <ScrollView
      style={styles.container}
      refreshControl={
        <RefreshControl refreshing={refreshing} onRefresh={handleRefresh} tintColor={colors.textPrimary} />
      }
    >
      <View style={styles.header}>
        <View>
          <Text style={styles.title}>RachaStats</Text>
          <Text style={styles.subtitle}>Gerência do seu Racha</Text>
        </View>
        <Pressable style={styles.signOutButton} onPress={handleSignOut}>
          <Text style={styles.signOutText}>Sair</Text>
        </Pressable>
      </View>

      <View style={styles.block}>
        <Text style={styles.blockTitle}>Importar lista semanal do racha</Text>
        <TextInput
          value={rawText}
          onChangeText={setRawText}
          placeholder={'Exemplo: 14/05/2026\n1 - Thiago (Joao)'}
          placeholderTextColor={colors.textPlaceholder}
          multiline
          numberOfLines={6}
          textAlignVertical="top"
          style={styles.textArea}
        />
        <Pressable style={styles.primaryButton} onPress={handleImport} disabled={importing}>
          <Text style={styles.primaryButtonText}>{importing ? 'Importando...' : 'Importar racha'}</Text>
        </Pressable>
      </View>

      {lastImportedMatch ? (
        <View style={styles.block}>
          <Text style={styles.blockTitle}>Ultimo racha importado</Text>
          <Text style={styles.meta}>Data: {new Date(lastImportedMatch.date).toLocaleString('pt-BR')}</Text>
          <Text style={styles.meta}>Jogadores: {lastImportedMatch.players.length}</Text>
        </View>
      ) : null}

      {error ? <Text style={styles.error}>{error}</Text> : null}

      <AccordionSection title="Artilharia" subtitle="Top 3 goleadores">
        <Artilharia items={goalsRanking} />
      </AccordionSection>

      <AccordionSection title="Ranking de Presença" subtitle="Top 5 com destaque">
        <RankingPresenca items={frequencyRanking} />
      </AccordionSection>

      <AccordionSection
        key={lastImportedMatch?.id ?? 'sem-ultimo-racha'}
        title="Lista do Último Racha"
        subtitle={
          lastImportedMatch
            ? `matchId ${lastImportedMatch.id} · ${lastImportedMatch.players.length} jogadores`
            : 'Importe uma lista para carregar a partida'
        }
        defaultOpen={!!lastImportedMatch}
      >
        <ListaUltimoRacha
          items={lastImportedMatch?.players ?? []}
          matchId={lastImportedMatch?.id ?? null}
          editable={lastImportedMatch?.isEditable ?? false}
          editableUntil={lastImportedMatch?.editableUntil}
          onStatsSaved={handleStatsSaved}
        />
      </AccordionSection>


    </ScrollView>
  );
}
