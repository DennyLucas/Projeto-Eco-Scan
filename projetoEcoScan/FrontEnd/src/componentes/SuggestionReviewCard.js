// FrontEnd/src/components/SuggestionReviewCard.js
import React from 'react';
import { View, Text, TextInput, StyleSheet, Platform, TouchableOpacity } from 'react-native';
import { Picker } from '@react-native-picker/picker';
import { colors, typography, spacing } from '../styles/AppStyles';
import { MATERIAL_OPTIONS_FOR_FORM as MATERIAL_OPTIONS_FOR_REVIEW } from './SuggestionForm';

const toStr = (v) => (v === null || v === undefined ? '' : String(v));

const SuggestionReviewCard = ({ suggestion, reviewData, onReviewDataChange, onApprove, onCancelReview }) => {
    if (!suggestion || !reviewData) return null;

    return (
        <View style={styles.cardContainer}>
            <Text style={styles.cardTitle}>Analisar e Aprovar Sugestão</Text>

            <Text style={styles.infoText}>
                <Text style={styles.bold}>ID da Sugestão: </Text>
                {toStr(suggestion.id)}
            </Text>

            <Text style={styles.infoText}>
                <Text style={styles.bold}>Barcode: </Text>
                {toStr(suggestion.barcode)}
            </Text>

            <Text style={styles.label}>Nome do Produto (confirmar/editar):</Text>
            <TextInput
                style={styles.input}
                value={toStr(reviewData.productName)}                       // garante string
                onChangeText={(text) => onReviewDataChange('productName', text)}
                placeholder="Nome do Produto"
                placeholderTextColor={colors.textSecondary}
            />

            <Text style={styles.label}>Material Principal (confirmar/editar):</Text>
            <View style={styles.pickerContainer}>
                <Picker
                    selectedValue={reviewData.material}
                    style={styles.picker}
                    onValueChange={(itemValue) => onReviewDataChange('material', itemValue)}
                    dropdownIconColor={colors.textPrimary}
                    prompt="Selecione o Material Principal"
                >
                    {MATERIAL_OPTIONS_FOR_REVIEW.map((opt) => (
                        <Picker.Item
                            key={toStr(opt.value)}
                            label={toStr(opt.label)}
                            value={opt.value}
                            color={'#070000'}
                        />
                    ))}
                </Picker>
            </View>

            <View style={styles.buttonContainer}>
                <TouchableOpacity style={styles.customButton} onPress={onApprove}>
                    <Text style={styles.buttonText}>Aprovar e Cadastrar/Atualizar</Text>
                </TouchableOpacity>
                <TouchableOpacity style={styles.customButton} onPress={onCancelReview}>
                    <Text style={styles.buttonText}>Cancelar Análise</Text>
                </TouchableOpacity>
            </View>
        </View>
    );
};

const styles = StyleSheet.create({
    cardContainer: {
        width: '90%',
        height: '80%', 
        padding: spacing.medium,
        backgroundColor: '#76883e', 
        borderRadius: spacing.small,
        marginBottom: spacing.medium,
        borderWidth: 2,
        borderColor: '#273d00', 
    },
    cardTitle: {
        fontSize: typography.fontSizeSuggestionTitle,
        fontWeight: typography.fontWeightBold,
        color: '#eef5c1', 
        marginBottom: spacing.medium,
        textAlign: 'center',
    },
    infoText: {
        fontSize: typography.fontSizeValue,
        color: '#0b0600',
        marginBottom: spacing.small,
    },
    bold: {
        fontWeight: typography.fontWeightBold,
    },
    label: {
        fontSize: typography.fontSizeLabel, 
        color: '#0b0600',
        marginTop: spacing.small,
        marginBottom: spacing.small / 2,
    },
    input: {
        backgroundColor: '#e6e8c0',
        color: '#0b0600',
        paddingHorizontal: spacing.inputHorizontalPadding,
        paddingVertical: spacing.inputVerticalPadding,
        borderRadius: spacing.small / 2,
        marginBottom: spacing.marginBottomItem,
        borderWidth: 2,
        borderColor: '#2c4300',
        fontSize: typography.fontSizeInput,
    },

    buttonContainer: { // Container para centralizar e agrupar os botões customizados
        alignItems: 'center', 
        width: '100%', 
        marginBottom: 100, // Espaçamento maior antes dos botões
    },
    customButton: {
        alignSelf: 'stretch', 
        height: 55, // Use uma altura fixa em vez de porcentagem para mais consistência, ou 'auto'
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
    buttonText: {
        color: '#ebebc5', // Cor do texto do botão
        fontSize: 11, // Tamanho da fonte do texto do botão
        textAlign: 'center',
        paddingHorizontal: spacing.small, // Para garantir que o texto não "grude" nas bordas
        fontWeight: typography.fontWeightBold, // Adicionado negrito para o texto do botão
        fontFamily: "Bonega-Bold",
    },

    pickerContainer: {
        borderColor: '#2c4300',
        borderWidth: 2,
        borderRadius: spacing.small / 2,
        marginBottom: spacing.marginBottomItem,
        backgroundColor: '#e6e8c0',
    },
    picker: {
        marginBottom: 95,
        height: Platform.OS === 'ios' ? 120 : 50,
        width: '100%',
        
    },
});

export default SuggestionReviewCard;