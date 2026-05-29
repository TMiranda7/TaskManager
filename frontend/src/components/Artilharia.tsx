import { Text, View } from 'react-native';
import { GoalsRankingItem } from '../services/reports';
import { styles } from '@/styles/artilharia.styles';

const TOP_LIMIT = 3;

type ArtilhariaProps = {
  items: GoalsRankingItem[];
};

export function Artilharia({ items }: ArtilhariaProps) {
  const topScorers = items.slice(0, TOP_LIMIT);

  return (
    <>
      {topScorers.length === 0 ? (
        <Text style={styles.empty}>Sem dados de gols.</Text>
      ) : (
        topScorers.map((item, index) => {
          const isChampion = index === 0;

          return (
            <View key={item.playerId} style={[styles.card, isChampion && styles.cardGold]}>
              {isChampion ? <Text style={styles.championLabel}>Artilheiro</Text> : null}
              <Text style={[styles.rankBadge, isChampion && styles.rankBadgeGold]}>
                {index + 1}º lugar
              </Text>
              <Text style={[styles.cardTitle, isChampion && styles.cardTitleGold]}>
                {item.playerName}
              </Text>
              <Text style={[styles.cardValue, isChampion && styles.cardValueGold]}>
                {item.goalsCount} gols
              </Text>
            </View>
          );
        })
      )}
    </>
  );
}
