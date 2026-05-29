import { StyleSheet } from 'react-native';
import { colors, radii } from './theme';

export const styles = StyleSheet.create({
  centerContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: colors.background,
  },
  container: {
    flex: 1,
    backgroundColor: colors.background,
    paddingTop: 62,
    paddingHorizontal: 16,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 14,
    gap: 12,
  },
  title: {
    color: colors.textPrimary,
    fontSize: 30,
    fontWeight: '800',
  },
  subtitle: {
    color: colors.textSubtle,
    fontSize: 14,
  },
  signOutButton: {
    backgroundColor: colors.danger,
    borderRadius: radii.md,
    paddingVertical: 8,
    paddingHorizontal: 14,
  },
  signOutText: {
    color: colors.text,
    fontWeight: '700',
  },
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
  textArea: {
    minHeight: 120,
    borderRadius: radii.md,
    borderWidth: 1,
    borderColor: colors.borderInput,
    backgroundColor: colors.surfaceInput,
    color: '#F1F5F9',
    padding: 12,
    marginBottom: 10,
  },
  primaryButton: {
    backgroundColor: colors.primary,
    borderRadius: radii.md,
    paddingVertical: 12,
    alignItems: 'center',
  },
  primaryButtonText: {
    color: colors.text,
    fontWeight: '700',
  },
  meta: {
    color: '#A3B2C7',
    marginBottom: 4,
  },
  error: {
    color: colors.error,
    marginBottom: 14,
  },
});
