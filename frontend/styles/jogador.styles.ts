import { StyleSheet } from 'react-native';
import { colors, radii } from './theme';

export const styles = StyleSheet.create({
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
  cardTitle: {
    color: '#F8FAFC',
    fontSize: 15,
    fontWeight: '600',
  },
  cardMeta: {
    color: colors.textSoft,
    marginTop: 4,
    fontSize: 13,
  },
  modalOverlay: {
    flex: 1,
    backgroundColor: 'rgba(0, 0, 0, 0.65)',
    justifyContent: 'center',
    padding: 24,
  },
  modalCard: {
    backgroundColor: colors.surface,
    borderRadius: radii.xl,
    borderWidth: 1,
    borderColor: colors.borderMuted,
    padding: 20,
    gap: 16,
  },
  playerName: {
    color: colors.textPrimary,
    fontSize: 20,
    fontWeight: '800',
    textAlign: 'center',
  },
  fieldRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    gap: 12,
  },
  fieldLabelInline: {
    flex: 1,
    color: colors.textBody,
    fontSize: 14,
    fontWeight: '600',
  },
  stepper: {
    flexDirection: 'row',
    alignItems: 'center',
    borderWidth: 1,
    borderColor: colors.borderInput,
    borderRadius: radii.md,
    backgroundColor: colors.surfaceInput,
    overflow: 'hidden',
  },
  stepperButton: {
    width: 40,
    height: 40,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#152436',
  },
  stepperButtonDisabled: {
    opacity: 0.45,
  },
  stepperButtonText: {
    color: colors.textPrimary,
    fontSize: 20,
    fontWeight: '700',
    lineHeight: 22,
  },
  stepperInput: {
    width: 44,
    height: 40,
    textAlign: 'center',
    color: colors.textPrimary,
    fontSize: 16,
    fontWeight: '700',
    paddingVertical: 0,
    paddingHorizontal: 4,
  },
  closeButton: {
    backgroundColor: colors.primary,
    borderRadius: radii.md,
    paddingVertical: 12,
    alignItems: 'center',
  },
  closeButtonDisabled: {
    opacity: 0.6,
  },
  closeButtonText: {
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
  empty: {
    color: colors.textSoft,
  },
});
