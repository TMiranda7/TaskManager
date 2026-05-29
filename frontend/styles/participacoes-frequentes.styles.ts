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
  hint: {
    color: colors.textSoft,
    fontSize: 13,
    marginBottom: 10,
  },
  card: {
    backgroundColor: colors.surfaceInput,
    borderWidth: 1,
    borderColor: colors.borderInput,
    borderRadius: radii.md,
    padding: 12,
    marginBottom: 8,
  },
  cardPressed: {
    borderColor: colors.primary,
    opacity: 0.92,
  },
  cardDisabled: {
    opacity: 0.65,
  },
  presenceHighlightCard: {
    borderColor: colors.primary,
    backgroundColor: '#102A25',
  },
  presenceHighlightText: {
    color: colors.primary,
  },
  cardTitle: {
    color: '#F8FAFC',
    fontSize: 15,
    fontWeight: '600',
  },
  cardValue: {
    color: colors.textSoft,
    marginTop: 2,
  },
  cardStats: {
    color: colors.textSubtle,
    fontSize: 12,
    marginTop: 6,
  },
  rankBadge: {
    color: colors.textSubtle,
    fontSize: 13,
    fontWeight: '700',
    marginBottom: 2,
  },
});
