// FrontEnd/src/components/SuggestionForm.js
import React from 'react';
import { View, Text, TextInput, StyleSheet, Platform, TouchableOpacity } from 'react-native'; // Adicionado TouchableOpacity
import { Picker } from '@react-native-picker/picker';
import { colors, typography, spacing } from '../styles/AppStyles';

// Esta lista deve corresponder às MaterialNameKey "amigáveis" no seu AppDbContext
export const MATERIAL_OPTIONS_FOR_FORM = [
    { label: "Selecione um material*", value: "" },
    { label: "Plástico", value: "Plástico" },
    { label: "Vidro", value: "Vidro" },
    { label: "Papel", value: "Papel" },
    { label: "Papelão", value: "Papelão" },
    { label: "Metal (Aço/Ferro)", value: "Metal" },
    { label: "Alumínio", value: "Alumínio" },
    { label: "Borracha", value: "Borracha" },
    { label: "Orgânico", value: "Orgânico" },
    { label: "Lixo Eletrônico", value: "Eletrônico" },
    // Adicione mais materiais amigáveis conforme necessário
    { label: "Outro (descreva se necessário)", value: "Outro" },
];

const SuggestionForm = ({ suggestionData, onSuggestionDataChange, onSubmit, onCancel }) => {
    return (
        <View style={styles.container}>
            <Text style={styles.title}>Sugerir Novo Produto</Text>
            <Text style={styles.barcodeLabel}>Código de Barras: {suggestionData.barcode}</Text>

            <TextInput
                style={styles.input}
                placeholder="Nome do Produto*"
                value={suggestionData.productName}
                onChangeText={text => onSuggestionDataChange('productName', text)}
                placeholderTextColor={colors.textSecondary}
            />
            
            <Text style={styles.label}>Material Principal*:</Text>
            <View style={styles.pickerContainer}>
                <Picker
                    selectedValue={suggestionData.material}
                    style={styles.picker}
                    onValueChange={(itemValue) => onSuggestionDataChange('material', itemValue)}
                    dropdownIconColor={colors.textPrimary} 
                    prompt="Selecione o Material Principal" // Para Android
                >
                    {MATERIAL_OPTIONS_FOR_FORM.map((opt) => (
                        <Picker.Item 
                            key={opt.value} 
                            label={opt.label} 
                            value={opt.value} 
                            // Tenta usar preto para itens no dropdown Android, primário para outros
                            color={'#070000'} 
                        />
                    ))}
                </Picker>
            </View>

            {/* Campos de Dicas Removidos deste formulário */}

            <View style={styles.buttonContainer}>
                <TouchableOpacity style={styles.customButton} onPress={onSubmit}>
                    <Text style={styles.buttonText}>Enviar Sugestão</Text>
                </TouchableOpacity>
                <TouchableOpacity style={styles.customButton} onPress={onCancel}>
                    <Text style={styles.buttonText}>Cancelar</Text>
                </TouchableOpacity>
            </View>
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        width: '100%',
        height: '78%', 
        padding: spacing.medium,
        backgroundColor: '#76883e', 
        borderRadius: spacing.small,
        marginBottom: spacing.medium,
        borderWidth: 3,
        borderColor: '#273d00',
    },
    title: {
        fontSize: typography.fontSizeSuggestionTitle,
        fontWeight: typography.fontWeightBold,
        color: '#eef5c1', 
        marginBottom: spacing.medium,
        textAlign: 'center',
    },
    barcodeLabel: {
        fontSize: typography.fontSizeValue,
        color: '#0b0600',
        marginBottom: spacing.small,
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
        color:'#070000', 
    },
    
    // Novas partes dos estilos copiadas do SuggestionReviewCard
    buttonContainer: { 
        alignItems: 'center', 
        marginBottom: 100, 
    },
    customButton: {
        alignSelf: 'stretch', 
        height: 55, 
        backgroundColor: '#374c01', 
        paddingVertical: spacing.small, 
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
    buttonText: {
        color: '#ebebc5', 
        fontSize: 11, 
        textAlign: 'center',
        paddingHorizontal: spacing.small, 
        fontWeight: typography.fontWeightBold, 
        fontFamily: "Bonega-Bold",
    },
});

export default SuggestionForm;