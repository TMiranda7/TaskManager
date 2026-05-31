import { useState } from 'react';
import { Alert, Pressable, Text, TextInput, View } from 'react-native';
import { router } from 'expo-router';
import { useAuth } from '../src/auth/AuthContext';
import { colors } from '@/styles/theme';
import { styles } from '@/styles/login.styles';

export default function LoginScreen() {
  const { signIn } = useAuth();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);

  const handleLogin = async () => {
    if (!username || !password) {
      Alert.alert('Campos obrigatorios', 'Informe usuario e senha.');
      return;
    }

    try {
      setLoading(true);
      await signIn(username, password);
      router.replace('/home');
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Falha ao autenticar.';
      Alert.alert('Erro de login', message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.content}>
        <Text style={styles.title}>RachaStats</Text>
        <Text style={styles.subtitle}>Nosso Racha é muito Melhor!</Text>

        <TextInput
          value={username}
          onChangeText={setUsername}
          placeholder="Usuario"
          placeholderTextColor={colors.textMuted}
          style={styles.input}
          autoCapitalize="none"
        />

        <TextInput
          value={password}
          onChangeText={setPassword}
          placeholder="Senha"
          placeholderTextColor={colors.textMuted}
          style={styles.input}
          secureTextEntry
        />

        <Pressable style={styles.button} onPress={handleLogin} disabled={loading}>
          <Text style={styles.buttonText}>{loading ? 'Entrando...' : 'Entrar'}</Text>
        </Pressable>

        <Text style={styles.tip}>Versao : beta</Text>
      </View>

      <Text style={styles.rights}>todos os direitos reservados™</Text>
    </View>
  );
}
