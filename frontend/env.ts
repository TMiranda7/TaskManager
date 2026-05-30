import Constants from 'expo-constants';
import { Platform } from 'react-native';

const API_PORT = 5094;
const PRODUCTION_API_BASE_URL = 'https://hashstats-api-dev.azurewebsites.net';

function hostFromDevServerUri(uri: string | undefined): string | null {
  if (!uri) {
    return null;
  }

  const withoutScheme = uri.replace(/^[a-z]+:\/\//i, '');
  const host = withoutScheme.split('/')[0]?.split(':')[0];

  if (!host || host === 'localhost' || host === '127.0.0.1') {
    return null;
  }

  return host;
}

function getDevMachineHost(): string | null {
  const expoGoDebuggerHost =
    Constants.manifest2?.extra?.expoGo?.debuggerHost ??
    Constants.expoGoConfig?.debuggerHost;

  const fromDebugger = hostFromDevServerUri(expoGoDebuggerHost);
  if (fromDebugger) {
    return fromDebugger;
  }

  return hostFromDevServerUri(Constants.expoConfig?.hostUri);
}

function resolveDevelopmentApiBaseUrl(): string {
  const devHost = getDevMachineHost();
  if (devHost) {
    return `http://${devHost}:${API_PORT}`;
  }

  if (Platform.OS === 'android') {
    return `http://10.0.2.2:${API_PORT}`;
  }

  return `http://localhost:${API_PORT}`;
}

function resolveProductionApiBaseUrl(): string {
  return PRODUCTION_API_BASE_URL.replace(/\/$/, '');
}

export const API_BASE_URL = __DEV__
  ? resolveDevelopmentApiBaseUrl()
  : resolveProductionApiBaseUrl();

if (__DEV__) {
  console.log('[RachaStats] API_BASE_URL:', API_BASE_URL);
}
