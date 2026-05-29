import { StyleSheet } from 'react-native';
import { colors, radii } from './theme';

export const styles = StyleSheet.create({
  block: {
    backgroundColor: colors.surface,
    borderWidth: 1,
    borderColor: colors.borderMuted,
    borderRadius: radii.xl,
    marginBottom: 14,
    overflow: 'hidden',
  },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    gap: 12,
    padding: 14,
  },
  headerText: {
    flex: 1,
  },
  blockTitle: {
    color: colors.textBody,
    fontSize: 17,
    fontWeight: '700',
  },
  subtitle: {
    color: colors.textSoft,
    fontSize: 12,
    marginTop: 3,
  },
  indicator: {
    color: colors.textPrimary,
    fontSize: 24,
    fontWeight: '700',
    lineHeight: 26,
    minWidth: 24,
    textAlign: 'center',
  },
  content: {
    borderTopWidth: 1,
    borderTopColor: colors.borderMuted,
    padding: 14,
    paddingTop: 12,
  },
});
