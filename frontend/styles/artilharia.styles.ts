import { StyleSheet } from 'react-native';
import { colors, radii } from './theme';

export const styles = StyleSheet.create({
  block: {
    backgroundColor: colors.surface,
    borderWidth: 1,
    borderColor: colors.borderMuted,
    borderRadius: radii.xl,
    padding: 14,
    marginBottom: 14,
  },
  blockTitle: {
    color: colors.textBody,
    fontSize: 17,
    fontWeight: '700',
    marginBottom: 10,
  },
  empty: {
    color: colors.textSoft,
  },
  card: {
    backgroundColor: colors.surfaceInput,
    borderWidth: 1,
    borderColor: colors.borderInput,
    borderRadius: radii.md,
    padding: 12,
    marginBottom: 8,
  },
  cardGold: {
    backgroundColor: '#1A1608',
    borderColor: colors.gold,
    borderWidth: 2,
  },
  rankBadge: {
    color: colors.textSubtle,
    fontSize: 13,
    fontWeight: '700',
    marginBottom: 2,
  },
  rankBadgeGold: {
    color: colors.gold,
  },
  cardTitle: {
    color: '#F8FAFC',
    fontSize: 15,
    fontWeight: '600',
  },
  cardTitleGold: {
    color: colors.gold,
    fontSize: 17,
    fontWeight: '800',
  },
  cardValue: {
    color: colors.textSoft,
    marginTop: 2,
  },
  cardValueGold: {
    color: '#E8D48B',
    fontWeight: '600',
  },
  championLabel: {
    color: colors.gold,
    fontSize: 11,
    fontWeight: '700',
    letterSpacing: 0.6,
    marginBottom: 4,
    textTransform: 'uppercase',
  },
});
