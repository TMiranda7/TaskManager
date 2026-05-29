import { Pressable, Text, View } from 'react-native';
import { useState } from 'react';
import { JogadorFormModal } from './JogadorFormModal';
import type { JogadorFormPlayer, MatchDetailPlayer } from '../services/matches';
import { styles } from '@/styles/participacoes-frequentes.styles';

type ListaUltimoRachaProps = {
  items: MatchDetailPlayer[];
  matchId: number | null;
  editable: boolean;
  editableUntil?: string;
  onStatsSaved: () => void;
};

export function ListaUltimoRacha({
  items,
  matchId,
  editable,
  editableUntil,
  onStatsSaved,
}: ListaUltimoRachaProps) {
  const [modalVisible, setModalVisible] = useState(false);
  const [selectedPlayer, setSelectedPlayer] = useState<JogadorFormPlayer | null>(null);
  const [pressedId, setPressedId] = useState<string | null>(null);
  const [localInfractions, setLocalInfractions] = useState<Record<string, boolean>>({});

  const handlePress = (item: MatchDetailPlayer) => {
    if (!editable) {
      return;
    }

    setSelectedPlayer({
      id: item.id,
      name: item.name,
      goals: item.goals,
      hasInfraction: localInfractions[item.id] ?? false,
    });
    setModalVisible(true);
  };

  const handleSaved = (hasInfraction: boolean) => {
    if (!selectedPlayer) {
      return;
    }

    setLocalInfractions((current) => ({
      ...current,
      [selectedPlayer.id]: hasInfraction,
    }));
    onStatsSaved();
  };

  if (items.length === 0) {
    return <Text style={styles.empty}>Nenhum racha importado.</Text>;
  }

  return (
    <>
      <Text style={styles.hint}>
        {editable
          ? 'Toque no jogador para registrar gols da partida.'
          : 'Prazo de edição encerrado. Dados disponíveis apenas para ranking e histórico.'}
      </Text>
      {editableUntil ? (
        <Text style={styles.hint}>
          Editável até {new Date(editableUntil).toLocaleString('pt-BR')}.
        </Text>
      ) : null}

      {items.map((item, index) => {
        const hasInfractionSaved = item.id in localInfractions;
        const hasInfraction = localInfractions[item.id] ?? false;
        const pressed = pressedId === item.id;
        const showStats = item.goals > 0 || hasInfractionSaved;

        return (
          <Pressable
            key={item.id}
            style={[styles.card, pressed && styles.cardPressed, !editable && styles.cardDisabled]}
            onPress={() => handlePress(item)}
            onPressIn={() => editable && setPressedId(item.id)}
            onPressOut={() => setPressedId(null)}
            disabled={!editable}
          >
            <Text style={styles.rankBadge}>{index + 1}º na lista</Text>
            <Text style={styles.cardTitle}>{item.name}</Text>
            {item.invitedBy ? (
              <Text style={styles.cardValue}>Convidado por {item.invitedBy}</Text>
            ) : null}
            {showStats ? (
              <Text style={styles.cardStats}>
                Partida atual: {item.goals} gols ·{' '}
                {hasInfractionSaved
                  ? hasInfraction
                    ? 'Com infracao'
                    : 'Sem infracao'
                  : 'Infracao nao informada'}
              </Text>
            ) : null}
          </Pressable>
        );
      })}

      <JogadorFormModal
        visible={modalVisible}
        matchId={matchId}
        editable={editable}
        player={selectedPlayer}
        onClose={() => setModalVisible(false)}
        onSaved={handleSaved}
      />
    </>
  );
}
