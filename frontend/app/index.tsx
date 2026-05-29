import { useEffect } from 'react';
import { ActivityIndicator, View } from 'react-native';
import { router } from 'expo-router';
import { useAuth } from '../src/auth/AuthContext';
import { colors } from '@/styles/theme';
import { styles } from '@/styles/index.styles';

export default function IndexScreen() {
  const { token, loading } = useAuth();

  useEffect(() => {
    if (loading) {
      return;
    }

    router.replace(token ? '/home' : '/login');
  }, [token, loading]);

  return (
    <View style={styles.container}>
      <ActivityIndicator size="large" color={colors.text} />
    </View>
  );
}
