import { Text, View } from 'react-native';
import { FrequencyRankingItem } from '../services/reports';
import { styles } from '@/styles/participacoes-frequentes.styles';

const TOP_LIMIT = 10;

type FrequenciaProps = {
  items: FrequencyRankingItem[];
};

export function Frequencia({ items }: FrequenciaProps) {
  const topParticipants = items.slice(0, TOP_LIMIT);

  if (topParticipants.length === 0) {
    return <Text style={styles.empty}>Sem dados de frequencia.</Text>;
  }

  return (
    <>
      {topParticipants.map((item, index) => (
        <View key={item.playerId} style={styles.card}>
          <Text style={styles.rankBadge}>{index + 1}º lugar</Text>
          <Text style={styles.cardTitle}>{item.playerName}</Text>
          <Text style={styles.cardValue}>{item.attendanceCount} presencas</Text>
        </View>
      ))}
    </>
  );
}
