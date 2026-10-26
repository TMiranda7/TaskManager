import { useEffect } from 'react';
import { ActivityIndicator, View } from 'react-native';
import { router, useRootNavigationState } from 'expo-router';
import { useAuth } from '../src/auth/AuthContext';
import { colors } from '@/styles/theme';
import { styles } from '@/styles/index.styles';

export default function IndexScreen() {
  const { token, loading } = useAuth();
  const rootNavigationState = useRootNavigationState();

  useEffect(() => {
    if (loading || !rootNavigationState?.key) {
      return;
    }

    router.replace(token ? '/home' : '/login');
  }, [token, loading, rootNavigationState?.key]);

  return (
    <View style={styles.container}>
      <ActivityIndicator size="large" color={colors.text} />
    </View>
  );
}
