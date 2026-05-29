import { useState } from 'react';
import { Button, Text, View } from 'react-native';
import { FrequencyRankingItem } from '../services/reports';
import { styles } from '@/styles/participacoes-frequentes.styles';

const TOP_HIGHLIGHT_LIMIT = 5;
const INITIAL_VISIBLE_ITEMS = 7;

type RankingPresencaProps = {
  items: FrequencyRankingItem[];
};

export function RankingPresenca({ items }: RankingPresencaProps) {
  const [showAll, setShowAll] = useState(false);

  if (items.length === 0) {
    return <Text style={styles.empty}>Sem dados de presença.</Text>;
  }

  const visibleItems = showAll
    ? items
    : items.slice(0, INITIAL_VISIBLE_ITEMS);

  const shouldShowButton = items.length > INITIAL_VISIBLE_ITEMS;

  return (
    <>
      {visibleItems.map((item, index) => {
        const highlighted = index < TOP_HIGHLIGHT_LIMIT;

        return (
          <View
            key={item.playerId}
            style={[styles.card, highlighted && styles.presenceHighlightCard]}
          >
            <Text style={[styles.rankBadge, highlighted && styles.presenceHighlightText]}>
              {index + 1}º lugar
            </Text>

            <Text style={styles.cardTitle}>{item.playerName}</Text>
            <Text style={styles.cardValue}>{item.attendanceCount} presenças</Text>
          </View>
        );
      })}

      {shouldShowButton && !showAll && (
        <Button
          title="Mostrar mais..."
          onPress={() => setShowAll(true)}
        />
      )}
    </>
  );
}