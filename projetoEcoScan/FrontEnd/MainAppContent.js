// FrontEnd/MainAppContent.js
import React, { useState, useEffect } from 'react';
import {
    View,
    Text,
    Alert,
    Linking,
    ActivityIndicator,
    ScrollView,
    Platform,
    StyleSheet,
    TouchableOpacity,
    TextInput,
    ImageBackground,
    Image
} from 'react-native';
import { Camera as CameraUtils, CameraView, CameraType as ActualCameraType } from 'expo-camera';

import ProductDisplay from './src/componentes/ProductDisplay';
import SuggestionForm from './src/componentes/SuggestionForm';
import SuggestionReviewCard from './src/componentes/SuggestionReviewCard';
import { globalStyles, cameraScreenStyles, colors, spacing, typography } from './src/styles/AppStyles';

const BACKGROUND_IMAGE = require('./assets/fundo-app.png');


const ExpoCameraComponent = CameraView;
const requestCameraPermissions = CameraUtils.requestCameraPermissionsAsync;
const ExpoCameraType = ActualCameraType;

const BACKEND_URL = 'http://IP DA MAQUINA:5291'; // CONFIRME SEU IP E PORTA

const initialNewProductSuggestionDataState = {
    barcode: '', productName: '', material: '',
};

const initialReviewFormDataState = {
    barcode: '',
    productName: '',
    material: '',
};

export default function MainAppContent({ userRole, onLogoutRequest }) {
    const [showReviewSection, setShowReviewSection] = useState(false);
    const [pendingSuggestions, setPendingSuggestions] = useState([]);
    const [selectedSuggestionForReview, setSelectedSuggestionForReview] = useState(null);
    const [reviewFormData, setReviewFormData] = useState(initialReviewFormDataState);
    const [hasPermission, setHasPermission] = useState(null);
    const [cameraActive, setCameraActive] = useState(false);
    const [scannedData, setScannedData] = useState(null);
    const [productInfo, setProductInfo] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [userMessage, setUserMessage] = useState(null);
    const [showSuggestionForm, setShowSuggestionForm] = useState(false);
    const [newProductSuggestionData, setNewProductSuggestionData] = useState(initialNewProductSuggestionDataState);
    const [barcodeInput, setBarcodeInput] = useState('');
    const [showManualInput, setShowManualInput] = useState(false);

    useEffect(() => {
        const getCameraPermissions = async () => {
            try {
                if (typeof requestCameraPermissions !== 'function') {
                    Alert.alert("Erro Config.", "Permissão câmera não encontrada.");
                    setHasPermission(false); return;
                }
                const { status } = await requestCameraPermissions();
                setHasPermission(status === 'granted');
                if (status !== 'granted') Alert.alert("Permissão Necessária", "Câmera é necessária para escanear.");
            } catch (err) {
                Alert.alert("Erro Permissão", "Erro ao solicitar permissão da câmera.");
                setHasPermission(false);
            }
        };
        getCameraPermissions();
    }, []);

    const resetUIState = (keepLastScan = false) => {
        setProductInfo(null);
        if (!keepLastScan) setScannedData(null);
        setError(null);
        setUserMessage(null);
        setShowSuggestionForm(false);
        setNewProductSuggestionData(initialNewProductSuggestionDataState);
        setSelectedSuggestionForReview(null);
        setReviewFormData(initialReviewFormDataState);
        setBarcodeInput('');
    };

    const fetchProductInformation = async (barcode) => {
        const requestUrl = `${BACKEND_URL}/api/productinfo?barcode=${barcode}`;
        const response = await fetch(requestUrl);
        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`Erro do servidor: ${response.status} - ${errorText}`);
        }
        return await response.json();
    };

    const submitNewProductSuggestion = async (dataToSubmit) => {
        const payload = {
            barcode: dataToSubmit.barcode,
            productName: dataToSubmit.productName,
            material: dataToSubmit.material,
        };
        const response = await fetch(`${BACKEND_URL}/api/suggestions`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload),
        });
        const responseText = await response.text();
        if (!response.ok) {
            try {
                const errorData = JSON.parse(responseText);
                throw new Error(errorData?.message || errorData?.title || `Erro: ${response.status}`);
            } catch {
                throw new Error(responseText || `Erro: ${response.status}`);
            }
        }
        return JSON.parse(responseText);
    };

    const fetchPendingSuggestions = async () => {
        setIsLoading(true);
        try {
            const response = await fetch(`${BACKEND_URL}/api/suggestions`);
            if (!response.ok) throw new Error(`Erro ao buscar sugestões: ${response.status}`);
            const data = await response.json();
            setPendingSuggestions(data);
            setError(null);
        } catch (e) {
            setError(`Falha ao buscar sugestões: ${e.message}`);
            setPendingSuggestions([]);
            Alert.alert("Erro", `Não foi possível carregar as sugestões pendentes.`);
        } finally {
            setIsLoading(false);
        }
    };

    const approveSuggestionOnBackend = async (suggestionId, approvalData) => {
        const payloadForApproval = {
            barcode: approvalData.barcode,
            productName: approvalData.productName,
            material: approvalData.material,
        };
        const response = await fetch(`${BACKEND_URL}/api/suggestions/${suggestionId}/approve`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payloadForApproval),
        });
        const responseText = await response.text();
        if (!response.ok) {
            try {
                const errorData = JSON.parse(responseText);
                throw new Error(errorData?.message || errorData?.title || `Erro: ${response.status}`);
            } catch {
                throw new Error(responseText || `Erro: ${response.status}`);
            }
        }
        return JSON.parse(responseText);
    };

    const processBarcode = async (barcode) => {
        setIsLoading(true);
        resetUIState(true); // Limpa UI mas mantém o scannedData (barcode) para referência
        setScannedData(barcode);
        try {
            const ecoResponse = await fetchProductInformation(barcode);
            if (ecoResponse.suggestionNeeded) {
                let initialProductName = '';
                if (ecoResponse.dataSource.startsWith("API_") && ecoResponse.productName && !["Nome não fornecido pela API", "Produto não encontrado", "Produto não cadastrado no banco de dados"].includes(ecoResponse.productName)) {
                    initialProductName = ecoResponse.productName;
                }
                setNewProductSuggestionData({ ...initialNewProductSuggestionDataState, barcode: barcode, productName: initialProductName });
                setShowSuggestionForm(true);
                setUserMessage(ecoResponse.dataSource === "Not_Found_Everywhere" ? "Produto não encontrado. Ajude-nos cadastrando!" : "Informações incompletas. Sugira os detalhes.");
            } else {
                setProductInfo(ecoResponse);
            }
        } catch (e) {
            setError(`Falha ao buscar dados: ${e.message}.`);
            setNewProductSuggestionData({ ...initialNewProductSuggestionDataState, barcode: barcode });
            setShowSuggestionForm(true);
            setUserMessage("Erro ao buscar. Tente cadastrar o produto.");
        } finally {
            setIsLoading(false);
        }
    };

    const handleBarcodeScanned = async ({ data }) => {
        if (isLoading) return;
        setCameraActive(false);
        await processBarcode(data);
    };

    const handleManualBarcodeSubmit = async () => {
        if (isLoading) return;
        if (!barcodeInput.trim()) {
            Alert.alert("Campo Vazio", "Por favor, digite um código de barras.");
            return;
        }
        await processBarcode(barcodeInput.trim());
    };

    const handleNewProductSuggestionSubmit = async () => {
        if (!newProductSuggestionData.productName || !newProductSuggestionData.material) {
            Alert.alert("Campos Obrigatórios", "Nome do Produto e Material são obrigatórios."); return;
        }
        setIsLoading(true); setError(null); setUserMessage(null);
        try {
            const result = await submitNewProductSuggestion(newProductSuggestionData);
            Alert.alert("Sucesso!", result.message || "Obrigado pela colaboração!");
            setShowSuggestionForm(false); resetUIState(); // Limpa tudo após sucesso
        } catch (e) {
            setError(`Falha ao enviar: ${e.message}`); Alert.alert("Erro ao Enviar", `Detalhes: ${e.message}`);
        } finally { setIsLoading(false); }
    };

    const handleNewProductSuggestionDataChange = (field, value) => {
        setNewProductSuggestionData(prev => ({ ...prev, [field]: value }));
    };

    const handleToggleReviewSection = () => {
        const openingReview = !showReviewSection;
        setShowReviewSection(openingReview);
        if (openingReview) {
            resetUIState(); // Limpa a UI principal
            setShowManualInput(false); // Garante que input manual não esteja visível
            fetchPendingSuggestions();
        } else { // Fechando a seção de review
            setPendingSuggestions([]);
            setSelectedSuggestionForReview(null);
            setReviewFormData(initialReviewFormDataState);
        }
    };

    const handleSelectSuggestionForReview = (suggestion) => {
        setSelectedSuggestionForReview(suggestion);
        setReviewFormData({
            barcode: suggestion.barcode,
            productName: suggestion.productName,
            material: suggestion.material,
        });
    };

    const handleReviewFormChange = (field, value) => {
        setReviewFormData(prev => ({ ...prev, [field]: value }));
    };

    const handleApproveSuggestion = async () => {
        if (!selectedSuggestionForReview || !reviewFormData) return;
        if (!reviewFormData.productName || !reviewFormData.material) {
            Alert.alert("Campos Obrigatórios", "Nome e Material são obrigatórios para aprovar."); return;
        }
        setIsLoading(true);
        try {
            await approveSuggestionOnBackend(selectedSuggestionForReview.id, reviewFormData);
            Alert.alert("Sucesso", "Sugestão aprovada e produto processado!");
            setSelectedSuggestionForReview(null); setReviewFormData(initialReviewFormDataState);
            fetchPendingSuggestions(); // Atualiza a lista
        } catch (e) {
            Alert.alert("Erro ao Aprovar", `Detalhes: ${e.message}`);
        } finally { setIsLoading(false); }
    };

    const cancelReview = () => {
        setSelectedSuggestionForReview(null); setReviewFormData(initialReviewFormDataState);
    };

    const openScanner = () => {
        if (hasPermission === null) { Alert.alert("Aguarde", "Verificando permissões..."); return; }
        if (hasPermission === false) {
            Alert.alert("Sem Permissão", "Habilite a câmera nas configurações.",
                [{ text: "Configurações", onPress: () => Linking.openSettings() }, { text: "Cancelar" }]
            ); return;
        }
        resetUIState(); // Limpa UI antes de abrir a câmera
        setCameraActive(true);
    };

    const cancelCurrentAction = () => { // Usado para cancelar formulário de sugestão ou scan
        setCameraActive(false);
        setShowSuggestionForm(false);
        resetUIState(); // Reseta a UI principal
    };

    const toggleManualInput = () => {
        setShowManualInput(!showManualInput);
        if (!showManualInput) { // Se estiver abrindo o input manual
            resetUIState(); // Limpa outros elementos da UI (produto, erro, msg)
        }
    };

    if (hasPermission === null && !cameraActive) {
        return (
            <ImageBackground source={BACKGROUND_IMAGE} style={styles.backgroundImage} resizeMode="cover">
                <View style={globalStyles.fullScreenContainer}>
                    <ActivityIndicator size="large" color={colors.loadingIndicator} />
                    <Text style={globalStyles.messageText}>Solicitando permissão da câmera...</Text>
                </View>
            </ImageBackground>
        );
    }

    if (cameraActive && hasPermission) {
        return (
            <View style={globalStyles.fullScreenContainer}>
                <ExpoCameraComponent
                    style={cameraScreenStyles.camera}
                    type={ExpoCameraType?.back}
                    onBarcodeScanned={isLoading ? undefined : handleBarcodeScanned}
                    barcodeScannerSettings={{
                        barcodeTypes: ["ean13", "ean8", "qr", "upc_a", "upc_e", "code128", "code39", "code93", "codabar", "itf", "pdf417", "aztec", "datamatrix"],
                    }}
                />
                <View style={cameraScreenStyles.overlay}>
                    <Text style={cameraScreenStyles.overlayText}>Aponte para o código de barras</Text>
                </View>
                <View style={cameraScreenStyles.fixedBottomButtonContainer}>
                    <TouchableOpacity style={styles.cancelScanButton} onPress={cancelCurrentAction}>
                        <Text style={styles.cancelScanButtonText}>Cancelar Scan</Text>
                    </TouchableOpacity>
                </View>
            </View>
        );
    }

    return (
        <ImageBackground source={BACKGROUND_IMAGE} style={styles.backgroundImage} resizeMode="cover">
            <View style={styles.overlayContent}>
                <View style={styles.headerContainer}>
                    <TouchableOpacity onPress={onLogoutRequest} style={styles.logoutButton}>
                        <Text style={styles.logoutButtonText}>Modificar Acesso</Text>
                    </TouchableOpacity>
                    
                </View>

                <ScrollView contentContainerStyle={styles.scrollContentCenter}>
                    <View style={globalStyles.contentContainer}>
                        <Text style={styles.currentUserRoleText}>Perfil: {userRole}</Text>

                        {userRole === 'ADMIN' && (
                            <View style={styles.actionButtonContainer}>
                                <TouchableOpacity
                                    style={[
                                        styles.mainButton,
                                        showReviewSection ? styles.goBackToScanButton : { backgroundColor: colors.warning }
                                    ]}
                                    onPress={handleToggleReviewSection}
                                >
                                    <Text style={styles.mainButtonText}>
                                        {showReviewSection ? "Voltar ao Scan" : "Analisar Sugestões"}
                                    </Text>
                                </TouchableOpacity>
                            </View>
                        )}

                        {showReviewSection && userRole === 'ADMIN' ? (
                            <View style={globalStyles.adminSectionContainer}>
                                <Text style={globalStyles.adminSectionTitle}>Sugestões Pendentes</Text>
                                {isLoading && pendingSuggestions.length === 0 && !selectedSuggestionForReview && <ActivityIndicator size="small" color={colors.loadingIndicator} />}
                                {selectedSuggestionForReview && reviewFormData ? (
                                    <SuggestionReviewCard
                                        suggestion={selectedSuggestionForReview}
                                        reviewData={reviewFormData}
                                        onReviewDataChange={handleReviewFormChange}
                                        onApprove={handleApproveSuggestion}
                                        onCancelReview={cancelReview}
                                        approveButtonStyle={[styles.mainButton, styles.approveButton]}
                                        approveButtonTextStyle={styles.mainButtonText}
                                        cancelReviewButtonStyle={[styles.mainButton, styles.cancelButton]}
                                        cancelReviewButtonTextStyle={styles.mainButtonText}
                                    />
                                ) : (
                                    !isLoading && pendingSuggestions.length > 0 ? (
                                        pendingSuggestions.map(sugg => (
                                            <View key={sugg.id} style={globalStyles.suggestionListItem}>
                                                <Text style={globalStyles.suggestionText}>ID: {sugg.id} - {sugg.productName}</Text>
                                                <Text style={globalStyles.suggestionTextSmall}>Barcode: {sugg.barcode}</Text>
                                                <Text style={globalStyles.suggestionTextSmall}>Material: {sugg.material}</Text>
                                                <TouchableOpacity style={[styles.mainButton, styles.reviewApproveButton]} onPress={() => handleSelectSuggestionForReview(sugg)}>
                                                    <Text style={styles.mainButtonText}>Analisar/Aprovar</Text>
                                                </TouchableOpacity>
                                            </View>
                                        ))
                                    ) : (
                                        !isLoading && <Text style={globalStyles.messageText}>Nenhuma sugestão pendente.</Text>
                                    )
                                )}
                                {error && <Text style={{color: colors.error, textAlign: 'center', marginTop: 10, width: '100%'}}>{error}</Text>}
                            </View>
                        ) : (
                            <>
                                {showManualInput && (
                                    <View style={styles.manualInputContainer}>
                                        <TextInput
                                            style={styles.manualInput}
                                            placeholder="Digite o código de barras"
                                            keyboardType="numeric"
                                            value={barcodeInput}
                                            onChangeText={setBarcodeInput}
                                            placeholderTextColor={colors.textSecondary}
                                            onSubmitEditing={handleManualBarcodeSubmit}
                                        />
                                        <TouchableOpacity
                                            style={[styles.mainButtonSmall, { backgroundColor: '#374c01'}, isLoading || !barcodeInput.trim() ? styles.disabledButton : {}]}
                                            onPress={handleManualBarcodeSubmit}
                                            disabled={isLoading || !barcodeInput.trim()}
                                        >
                                            <Text style={styles.mainButtonTextSmall}>Buscar</Text>
                                        </TouchableOpacity>
                                    </View>
                                )}

                                {!showSuggestionForm && (
                                    <>
                                        {!showManualInput && (
                                            <View style={styles.actionButtonContainer}>
                                                <TouchableOpacity
                                                    style={[
                                                        styles.mainButton,
                                                        // A cor do botão Escanear Outro/Código será dinâmica
                                                        (productInfo || error || userMessage) ? styles.scanAnotherButton : styles.accentButton,
                                                        hasPermission === false || isLoading ? styles.disabledButton : {}
                                                    ]}
                                                    onPress={openScanner}
                                                    disabled={hasPermission === false || isLoading}
                                                >
                                                    <Text style={styles.mainButtonText}>
                                                        {(productInfo || error || userMessage) ? "Escanear Outro" : "Escanear Código"}
                                                    </Text>
                                                </TouchableOpacity>
                                            </View>
                                        )}
                                        <View style={styles.actionButtonContainer}>
                                            <TouchableOpacity
                                                style={[
                                                    styles.mainButton,
                                                    // O botão Digitar Código terá uma cor fixa diferente
                                                    showManualInput ? styles.cancelManualInputButton : styles.manualInputButton, // Usar estilos diferentes aqui
                                                    isLoading ? styles.disabledButton : {}
                                                ]}
                                                onPress={toggleManualInput}
                                                disabled={isLoading}
                                            >
                                                <Text style={styles.mainButtonText}>
                                                    {showManualInput ? "Cancelar Digitação" : "Digitar Código"}
                                                </Text>
                                            </TouchableOpacity>
                                        </View>
                                    </>
                                )}

                                {isLoading && <ActivityIndicator size="large" color={colors.loadingIndicator} style={globalStyles.loadingIndicator} />}

                                {!isLoading && !showSuggestionForm && (
                                    <>
                                        {userMessage && <Text style={[globalStyles.messageText, globalStyles.userMessageEmphasis]}>{userMessage}</Text>}
                                        {error && (
                                            <View style={globalStyles.errorBox}>
                                                <Text style={globalStyles.errorText}>Erro:</Text>
                                                <Text style={globalStyles.errorTextDetails}>{error}</Text>
                                            </View>
                                        )}
                                    </>
                                )}

                                {showSuggestionForm && !isLoading && (
                                    <SuggestionForm
                                        suggestionData={newProductSuggestionData}
                                        onSuggestionDataChange={handleNewProductSuggestionDataChange}
                                        onSubmit={handleNewProductSuggestionSubmit}
                                        onCancel={cancelCurrentAction}
                                        submitButtonStyle={[styles.mainButton, styles.approveButton]}
                                        submitButtonTextStyle={styles.mainButtonText}
                                        cancelButtonStyle={[styles.mainButton, styles.cancelButton]}
                                        cancelButtonTextStyle={styles.mainButtonText}
                                    />
                                )}

                                {productInfo && !isLoading && !showSuggestionForm && (
                                    <ProductDisplay productInfo={productInfo} />
                                )}

                                {!isLoading && !productInfo && !showSuggestionForm && hasPermission === false && (
                                    <View style={globalStyles.centeredMessageContainer}>
                                        <Text style={globalStyles.messageText}>Permissão para câmera negada.</Text>
                                        <TouchableOpacity style={[styles.mainButton, styles.secondaryButton]} onPress={() => Linking.openSettings().catch(err => console.warn("Não foi possível abrir config:", err))}>
                                            <Text style={styles.mainButtonText}>Abrir Configurações</Text>
                                        </TouchableOpacity>
                                    </View>
                                )}
                            </>
                        )}
                    </View>
                </ScrollView>
            </View>
        </ImageBackground>
    );
}

const styles = StyleSheet.create({
    backgroundImage: {
        flex: 1,
        width: '100%',
        height: '100%',
    },
    overlayContent: {
        flex: 1,
        marginBottom: 120, // Mantém o espaçamento inferior para evitar que o conteúdo fique muito embaixo
    },
    headerContainer: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        paddingHorizontal: spacing.medium,
        paddingTop: Platform.OS === 'ios' ? spacing.large * 2 : spacing.large, // Aumenta um pouco o padding superior
        paddingBottom: spacing.small,
        backgroundColor: 'transparent',
    },
    logoutButton: {
        paddingVertical: spacing.small,
        paddingHorizontal: spacing.medium,
    },
    logoutButtonText: {
        color: '#000',
        fontSize: typography.fontSizeRegular,
        fontWeight: typography.fontWeightMedium,

    },
    appLogoMainContent: {
        width: 60,
        height: 60,
    },
    scrollContentCenter: {
        flexGrow: 1,
        justifyContent: 'center', // Centraliza verticalmente o conteúdo quando não há scroll
        alignItems: 'center', // Centraliza horizontalmente o conteúdo
        paddingVertical: spacing.large,
    },
    currentUserRoleText: {
        fontSize: typography.fontSizeSmall,
        color: '#000100',
        textAlign: 'center',
        marginBottom: spacing.medium,
        fontFamily: "System", // Mantido System
    },
    manualInputContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-between',
        marginBottom: spacing.medium,
        width: '90%',
        alignSelf: 'center',
    },
    manualInput: {
        flex: 1,
        height: 44,
        borderColor: '#758837',
        borderWidth: 1,
        borderRadius: spacing.small,
        paddingHorizontal: spacing.inputHorizontalPadding,
        marginRight: spacing.small,
        backgroundColor: '#f0eaba',
        color: '#536819',
        fontSize: typography.fontSizeInput,
    },
    actionButtonContainer: {
        width: '100%',
        alignItems: 'center',
        marginVertical: spacing.small,
    },
    // Estilos de botões, consistentes com App.js
    mainButton: {
        width: 280,
        height: 60,
        paddingHorizontal: 20,
        paddingVertical: spacing.medium,
        borderRadius: 25,
        alignItems: 'center',
        justifyContent: 'center',
        elevation: 3,
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.2,
        shadowRadius: 3,
    },
    mainButtonText: {
        color: '#ebefbc',
        fontSize: 14,
        letterSpacing: 1,
        fontFamily: "Bonega-Bold",
    },
    mainButtonSmall: {
        paddingVertical: spacing.small,
        paddingHorizontal: spacing.medium,
        borderRadius: 20,
        alignItems: 'center',
        backgroundColor: '#76883e',
        justifyContent: 'center',
        elevation: 2,
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 1 },
        shadowOpacity: 0.1,
        shadowRadius: 2,
        
    },
    mainButtonTextSmall: {
        color: '#e5ecb9',
        fontSize: typography.fontSizeRegular,
        fontWeight: typography.fontWeightMedium,
        
    },
    accentButton: {
        backgroundColor: '#374c01',
    },
    secondaryButton: {
        backgroundColor: '#76883e',
    },
    cancelButton: {
        backgroundColor: '#374c01',
    },
    approveButton: {
        backgroundColor: colors.primary,
    },
    scanAnotherButton: { // NOVO: Cor para "Escanear Outro" (quando já tem info de produto ou erro)
        backgroundColor: '#374c01', // Exemplo: uma cor secundária
    },
    manualInputButton: { // NOVO: Cor para "Digitar Código" (quando não está ativo)
        backgroundColor: '#76883e', // Exemplo: uma cor diferente
    },
    cancelManualInputButton: { // NOVO: Cor para "Cancelar Digitação" (quando está ativo)
        backgroundColor: '#374c01', // Exemplo: cor de cancelamento ou outra cor distinta
    },
    actionSuggestionButton: {
        backgroundColor: colors.secondary, // Este estilo é usado para o botão "Analisar/Aprovar" dentro da card de revisão
        width: 180,
        height: 60,
        paddingHorizontal: spacing.medium,
        marginTop: spacing.small,
        alignSelf: 'center',
    },

    reviewApproveButton: { // NOVO ESTILO PARA O BOTÃO NA LISTA DE SUGESTÕES PENDENTES
        alignSelf: 'stretch', 
        height: 55, 
        backgroundColor: '#374c01', 
        paddingVertical: spacing.small, // Padding interno do botão
        borderRadius: 25,
        alignItems: 'center',
        justifyContent: 'center',
        marginVertical: spacing.small, // Espaçamento entre os botões
        elevation: 3,
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.2,
        shadowRadius: 3, 
    },
    disabledButton: {
        backgroundColor: '#cccccc',
        opacity: 0.7,
    },
    cancelScanButton: {
        width: 300,
        height: 45,
        paddingHorizontal: 15,
        paddingVertical: spacing.small,
        borderRadius: 25,
        alignItems: 'center',
        justifyContent: 'center',
        backgroundColor: '#d22e2e',
        elevation: 3,
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.2,
        shadowRadius: 3,
    },
    cancelScanButtonText: {
        color: '#ebefbc',
        fontSize: typography.fontSizeRegular,
        letterSpacing: 1,
        fontFamily: "Bonega-Bold",
    },
    // NOVO ESTILO PARA O BOTÃO "VOLTAR AO SCAN"
    goBackToScanButton: {
        backgroundColor: '#76883e', // Uma cor de fundo verde escura ou a de sua preferência
    },
});