import { StyleSheet } from 'react-native';
import { colors, radii } from './theme';

export const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: colors.background,
    padding: 24,
  },
  content: {
    flex: 1,
    justifyContent: 'center',
    gap: 12,
  },
  title: {
    color: colors.text,
    fontSize: 34,
    fontWeight: '800',
  },
  subtitle: {
    color: colors.textSecondary,
    marginBottom: 8,
  },
  input: {
    borderWidth: 1,
    borderColor: colors.border,
    borderRadius: radii.lg,
    paddingHorizontal: 14,
    paddingVertical: 12,
    color: colors.text,
    backgroundColor: colors.surfaceElevated,
  },
  button: {
    marginTop: 8,
    borderRadius: radii.lg,
    backgroundColor: colors.primary,
    paddingVertical: 14,
    alignItems: 'center',
  },
  buttonText: {
    color: colors.text,
    fontWeight: '700',
    fontSize: 16,
  },
  tip: {
    color: colors.textMuted,
    textAlign: 'center',
    marginTop: 8,
  },
  rights: {
    color: colors.textMuted,
    fontSize: 12,
    textAlign: 'center',
    paddingBottom: 8,
  },
});
