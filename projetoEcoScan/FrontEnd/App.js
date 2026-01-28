// FrontEnd/App.js
import React, { useState, useEffect, useRef } from 'react';
import {
  View,
  Text,
  TextInput,
  StyleSheet,
  Alert,
  ActivityIndicator,
  Platform,
  Animated,
  ImageBackground,
  TouchableOpacity,
  Image
} from 'react-native';
import { useFonts } from 'expo-font';
import MainAppContent from './MainAppContent';
import { globalStyles, colors, spacing, typography } from './src/styles/AppStyles';
import AsyncStorage from '@react-native-async-storage/async-storage';

const BACKGROUND_IMAGE = require('./assets/fundo-app.png');
const APP_LOGO_IMAGE = require('./assets/logo-app.png');
const ADMIN_PASSWORD = "Admin";
const USER_ROLE_KEY = '@userRole';
const TEXT_FADE_IN_DURATION = 800;
const SPLASH_HOLD_DURATION = 1500;
const SCREEN_FADE_OUT_DURATION = 700;

export default function App() {
  const [fontsLoaded] = useFonts({
    "Bonega-Bold": require("./assets/fonts/bonega-bold.ttf"),
  });

  const [showSplash, setShowSplash] = useState(true);
  const [currentView, setCurrentView] = useState('loading');
  const [userRole, setUserRole] = useState(null);
  const [adminPassword, setAdminPassword] = useState('');
  const [loginError, setLoginError] = useState('');

  const screenOpacity = useRef(new Animated.Value(1)).current;
  const textOpacity = useRef(new Animated.Value(0)).current;
  const roleSelectionContentOpacity = useRef(new Animated.Value(0)).current;

  useEffect(() => {
    Animated.timing(textOpacity, {
      toValue: 1,
      duration: TEXT_FADE_IN_DURATION,
      useNativeDriver: true,
    }).start(() => {
      setTimeout(() => {
        Animated.timing(screenOpacity, {
          toValue: 0,
          duration: SCREEN_FADE_OUT_DURATION,
          useNativeDriver: true,
        }).start(() => {
          setShowSplash(false);
        });
      }, SPLASH_HOLD_DURATION);
    });

    return () => {
      textOpacity.stopAnimation();
      screenOpacity.stopAnimation();
      roleSelectionContentOpacity.stopAnimation();
    };
  }, []);

  useEffect(() => {
    if (showSplash) return;

    const loadStoredRole = async () => {
      try {
        const storedRole = await AsyncStorage.getItem(USER_ROLE_KEY);
        if (storedRole) {
          setUserRole(storedRole);
          setCurrentView('appContent');
        } else {
          setCurrentView('roleSelection');
          Animated.timing(roleSelectionContentOpacity, {
            toValue: 1,
            duration: 500,
            useNativeDriver: true,
          }).start();
        }
      } catch (e) {
        console.error("Failed to load user role from storage", e);
        setCurrentView('roleSelection');
        Animated.timing(roleSelectionContentOpacity, {
          toValue: 1,
          duration: 500,
          useNativeDriver: true,
        }).start();
      }
    };
    loadStoredRole();
  }, [showSplash]);

  const saveAndSetRole = async (role) => {
    try {
      await AsyncStorage.setItem(USER_ROLE_KEY, role);
      setUserRole(role);
      setCurrentView('appContent');
    } catch (e) {
      console.error("Failed to save user role", e);
      Alert.alert("Erro", "Não foi possível salvar a preferência de perfil.");
      setUserRole(role);
      setCurrentView('appContent');
    }
  };

  const handleRoleSelection = (role) => {
    if (role === 'ADMIN') {
      setCurrentView('adminLogin');
      setLoginError('');
      setAdminPassword('');
    } else if (role === 'USER') {
      saveAndSetRole('USER');
    }
  };

  const handleAdminLogin = () => {
    if (adminPassword === ADMIN_PASSWORD) {
      saveAndSetRole('ADMIN');
      setLoginError('');
    } else {
      setLoginError('Senha incorreta!');
      Alert.alert("Erro de Login", "Senha incorreta!");
    }
    setAdminPassword('');
  };

  const handleLogout = async () => {
    try {
      await AsyncStorage.removeItem(USER_ROLE_KEY);
      setUserRole(null);
      setCurrentView('roleSelection');
      setAdminPassword('');
      setLoginError('');
      roleSelectionContentOpacity.setValue(0);
      Animated.timing(roleSelectionContentOpacity, {
        toValue: 1,
        duration: 500,
        useNativeDriver: true,
      }).start();
    } catch (e) {
      console.error("Failed to clear user role", e);
      Alert.alert("Erro", "Não foi possível limpar as preferências de perfil.");
      setUserRole(null);
      setCurrentView('roleSelection');
      roleSelectionContentOpacity.setValue(0);
      Animated.timing(roleSelectionContentOpacity, {
        toValue: 1,
        duration: 500,
        useNativeDriver: true,
      }).start();
    }
  };

  const goBackToRoleSelectionFromLogin = () => {
    setCurrentView('roleSelection');
    setAdminPassword('');
    setLoginError('');
    roleSelectionContentOpacity.setValue(0);
    Animated.timing(roleSelectionContentOpacity, {
      toValue: 1,
      duration: 500,
      useNativeDriver: true,
    }).start();
  };

  const renderMainContent = () => {
    if (currentView === 'loading') {
      return (
        <View style={styles.container}>
          <ActivityIndicator size="large" color={colors.primary} />
          <Text style={[styles.loadingText]}>Carregando...</Text>
        </View>
      );
    }

    if (currentView === 'roleSelection') {
      return (
        <ImageBackground source={BACKGROUND_IMAGE} style={styles.backgroundImage} resizeMode="cover">
        <Animated.View style={[styles.roleSelectionContent, { opacity: roleSelectionContentOpacity }]}>
          <View style={styles.logoContainer}>
            <Image source={APP_LOGO_IMAGE} style={styles.appLogoImage} resizeMode="contain" />
          </View>

          <Text style={styles.mainTitle}>BEM-VINDO AO</Text>
          <Text style={styles.ecoScannerTitle}>ECO                   SCANNER</Text>
          <Text style={styles.question}>VOCÊ É ADMIN OU USUÁRIO?</Text>

          <TouchableOpacity style={styles.roleButton} onPress={() => handleRoleSelection('ADMIN')}>
            <Text style={styles.roleButtonText}>ADMIN</Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={[styles.roleButton, styles.userRoleButton]}
            onPress={() => handleRoleSelection('USER')}
          >
            <Text style={styles.roleButtonText}>USUÁRIO</Text>
          </TouchableOpacity>

          <Text style={styles.footerText}>© Eco Scan. Todos os direitos reservados</Text>
        </Animated.View>
        </ImageBackground>
      );
    }

    if (currentView === 'adminLogin') {
  return (
    <ImageBackground source={BACKGROUND_IMAGE} style={{flex: 1}} resizeMode="cover">
      <View style={styles.container}>
        <Text style={styles.loginTitle}>Login de Administrador</Text>
        <Text style={styles.label}>Digite a senha para entrar como ADMIN:</Text>
        <TextInput
          style={styles.input}
          placeholder="Senha"
          secureTextEntry
          value={adminPassword}
          onChangeText={setAdminPassword}
          placeholderTextColor={colors.textSecondary}
        />
        {loginError ? <Text style={styles.errorText}>{loginError}</Text> : null}
        <TouchableOpacity style={styles.loginButton} onPress={handleAdminLogin}>
          <Text style={styles.loginButtonText}>Entrar</Text>
        </TouchableOpacity>
        <TouchableOpacity style={[styles.loginButton, styles.backButton]} onPress={goBackToRoleSelectionFromLogin}>
          <Text style={styles.loginButtonText}>Voltar</Text>
        </TouchableOpacity>
      </View>
    </ImageBackground>
  );
}


    if (currentView === 'appContent' && userRole) {
      return <MainAppContent userRole={userRole} onLogoutRequest={handleLogout} />;
    }

    return (
      <View style={styles.container}>
        <Text>Erro inesperado. Por favor, reinicie o aplicativo.</Text>
      </View>
    );
  };

  if (!fontsLoaded) {
    return null; // ou poderia exibir um splash
  }

  return (
    <View style={{ flex: 1, backgroundColor: colors.background }}>
      {showSplash ? (
        <Animated.View style={[styles.splashContainer, { opacity: screenOpacity }]}>
          <Animated.Text style={[styles.splashText, { opacity: textOpacity }]}>
            EcoScan
          </Animated.Text>
        </Animated.View>
      ) : (
          renderMainContent()
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  splashContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#4CAF50',
  },
  splashText: {
    fontSize: 48,
    color: '#FFFFFF',
    fontFamily: "Bonega-Bold", // ✅ fonte aplicada
  },
  container: {
    marginTop: -140,
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: spacing.large,
  },
  loadingText: {
    marginTop: spacing.medium,
    color: colors.textPrimary,
    fontFamily: "Bonega-Bold", // ✅ fonte aplicada
  },
  backgroundImage: {
    flex: 1,
    width: '100%',
    height: '100%',
    justifyContent: 'center',
    alignItems: 'center',
  },
  roleSelectionContent: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    width: '100%',
  },
  logoContainer: {
    position: 'absolute',
    top: Platform.OS === 'ios' ? spacing.large * 2 : spacing.large,
    left: 0,
    right: 0,
    alignItems: 'center',
    zIndex: 1,
  },
  appLogoImage: {
    width: 80,
    height: 100,
  },
  mainTitle: {
    fontSize: typography.fontSizeLabel,
    color: '#2d221c',
    marginBottom: spacing.tiny,
    textAlign: 'center',
    marginTop: spacing.large * -2,
    fontFamily: "System",
  },
  ecoScannerTitle: {
    fontSize: 35,
    color: '#000000',
    marginBottom: spacing.medium,
    textAlign: 'center',
    fontFamily: "Bonega-Bold", // ✅
  },
  question: {
    fontSize: typography.fontSizeMessage,
    color: '#070000',
    marginBottom: spacing.large,
    textAlign: 'center',
    fontFamily: "System",
  },
  roleButton: {
    width: '70%',
    backgroundColor: '#374c01',
    paddingVertical: spacing.medium,
    borderRadius: 25,
    alignItems: 'center',
    justifyContent: 'center',
    marginVertical: spacing.small,
    elevation: 3,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.2,
    shadowRadius: 3,
  },
  userRoleButton: {
    backgroundColor: '#76883e',
  },
  roleButtonText: {
    color: '#ebefbc',
    fontSize: 15,
    letterSpacing: 1,
    fontFamily: "Bonega-Bold", // ✅
  },
  footerText: {
    position: 'absolute',
    bottom: Platform.OS === 'ios' ? spacing.paddingContent : spacing.small,
    fontSize: 12,
    color: '#010100',
    textAlign: 'center',
    width: '70%',
    fontFamily: "System",
  },
  loginTitle: {
    fontSize: 25,
    color: '#010200',
    marginBottom: spacing.extraLarge,
    textAlign: 'center',
    fontFamily: "Bonega-Bold", // ✅
  },
  label: {
    fontSize: typography.fontSizeLabel,
    color: '#09030d',
    marginBottom: spacing.small,
    alignSelf: 'flex-start',
    marginLeft: '10%',
    
  },
  input: {
    width: '80%',
    backgroundColor: '#fbf0b8',
    color: '#627420',
    paddingHorizontal: spacing.inputHorizontalPadding,
    paddingVertical: spacing.inputVerticalPadding,
    borderRadius: spacing.small,
    borderWidth: 1,
    borderColor: '#7a8a41',
    marginBottom: spacing.medium,
    fontSize: typography.fontSizeInput,
  },
  loginButton: {
    width: '80%',
    backgroundColor: '#76883e',
    paddingVertical: spacing.medium,
    borderRadius: 20,
    alignItems: 'center',
    justifyContent: 'center',
    marginVertical: spacing.small,
    elevation: 3,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.2,
    shadowRadius: 3,
  },
  loginButtonText: {
    color: colors.textPrimary,
    fontSize: 14,
    fontFamily: "Bonega-Bold", // ✅
  },
  backButton: {
    backgroundColor: '#374c01',
    marginTop: spacing.medium,
  },
  errorText: {
    color: colors.error,
    fontSize: typography.fontSizeValue,
    marginBottom: spacing.medium,
    textAlign: 'center',
  },
});
