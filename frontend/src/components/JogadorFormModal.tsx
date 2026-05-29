import { useEffect, useState } from 'react';
import {
  ActivityIndicator,
  Alert,
  Modal,
  Pressable,
  Switch,
  Text,
  TextInput,
  View,
} from 'react-native';
import { JogadorFormPlayer, registerMatchGoals } from '../services/matches';
import { colors } from '@/styles/theme';
import { styles } from '@/styles/jogador.styles';

type JogadorFormModalProps = {
  visible: boolean;
  matchId: number | null;
  editable: boolean;
  player: JogadorFormPlayer | null;
  onClose: () => void;
  onSaved: (hasInfraction: boolean) => void;
};

function GoalsStepper({
  value,
  onChange,
}: {
  value: number;
  onChange: (value: number) => void;
}) {
  const decrement = () => onChange(Math.max(0, value - 1));
  const increment = () => onChange(value + 1);

  const handleChangeText = (text: string) => {
    const digits = text.replace(/\D/g, '');
    if (!digits) {
      onChange(0);
      return;
    }

    onChange(Math.max(0, Number.parseInt(digits, 10)));
  };

  return (
    <View style={styles.stepper}>
      <Pressable
        style={[styles.stepperButton, value === 0 && styles.stepperButtonDisabled]}
        onPress={decrement}
        disabled={value === 0}
        accessibilityLabel="Diminuir gols"
      >
        <Text style={styles.stepperButtonText}>−</Text>
      </Pressable>

      <TextInput
        style={styles.stepperInput}
        value={String(value)}
        onChangeText={handleChangeText}
        keyboardType="number-pad"
        maxLength={2}
        selectTextOnFocus
      />

      <Pressable
        style={styles.stepperButton}
        onPress={increment}
        accessibilityLabel="Aumentar gols"
      >
        <Text style={styles.stepperButtonText}>+</Text>
      </Pressable>
    </View>
  );
}

export function JogadorFormModal({
  visible,
  matchId,
  editable,
  player,
  onClose,
  onSaved,
}: JogadorFormModalProps) {
  const [goals, setGoals] = useState(0);
  const [hasInfraction, setHasInfraction] = useState(false);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (!visible || !player) {
      return;
    }

    setGoals(player.goals);
    setHasInfraction(player.hasInfraction);
  }, [visible, player]);

  const handleDismiss = () => {
    if (saving) {
      return;
    }

    onClose();
  };

  const handleSave = async () => {
    if (!player || saving) {
      return;
    }

    if (!matchId) {
      Alert.alert(
        'Racha nao importado',
        'Importe a lista semanal do racha antes de registrar os gols.',
      );
      return;
    }

    if (!editable) {
      Alert.alert(
        'Prazo encerrado',
        'A lista do último racha só pode ser editada até 24 horas após a importação.',
      );
      return;
    }

    try {
      setSaving(true);
      await registerMatchGoals(matchId, {
        matchId,
        playerId: player.id,
        goals,
      });
      onSaved(hasInfraction);
      onClose();
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Falha ao salvar estatisticas do jogador.';
      Alert.alert('Erro', message);
    } finally {
      setSaving(false);
    }
  };

  return (
    <Modal
      visible={visible && !!player}
      transparent
      animationType="fade"
      onRequestClose={handleDismiss}
    >
      <Pressable style={styles.modalOverlay} onPress={handleDismiss}>
        <Pressable style={styles.modalCard} onPress={(event) => event.stopPropagation()}>
          <Text style={styles.playerName}>{player?.name}</Text>

          <View style={styles.fieldRow}>
            <Text style={styles.fieldLabelInline}>Gols no racha</Text>
            <GoalsStepper value={goals} onChange={setGoals} />
          </View>

          <View style={styles.fieldRow}>
            <Text style={styles.fieldLabelInline}>Cometeu infracao no Racha?</Text>
            <Switch
              value={hasInfraction}
              onValueChange={setHasInfraction}
              trackColor={{ false: colors.borderInput, true: '#0E6B4A' }}
              thumbColor={hasInfraction ? colors.text : '#CBD5E1'}
              ios_backgroundColor={colors.borderInput}
            />
          </View>

          <Pressable
            style={[styles.closeButton, saving && styles.closeButtonDisabled]}
            onPress={handleSave}
            disabled={saving}
          >
            {saving ? (
              <ActivityIndicator color="#FFFFFF" />
            ) : (
              <Text style={styles.closeButtonText}>Salvar e fechar</Text>
            )}
          </Pressable>
        </Pressable>
      </Pressable>
    </Modal>
  );
}
