// FrontEnd/src/components/ProductDisplay.js
import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import InfoRow from './InfoRow';
import { colors, typography, spacing } from '../styles/AppStyles';

const ProductDisplay = ({ productInfo }) => {
    if (!productInfo) return null;

    return (
        <View style={styles.container}>
            <Text style={styles.title}>{productInfo.productName}</Text>
            <InfoRow style={styles.texto} label="Código:" value={productInfo.barcode} />
            <InfoRow label="Material Principal:" value={productInfo.material} />
            <InfoRow label="Dicas de Descarte:" value={productInfo.disposalTips} />
            <InfoRow label="Reciclagem:" value={productInfo.recyclingInfo} />
            <InfoRow label="Impacto/Sustentabilidade:" value={productInfo.sustainabilityImpact} />
            <InfoRow label="Fonte:" value={productInfo.dataSource} />
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        marginTop: spacing.marginBottomSection,
        padding: spacing.medium,
        backgroundColor: '#76883e',
        borderRadius: spacing.small,
        borderWidth: 3,
        borderColor: '#2a4100',
        width: '100%',
    },
    title: {
        fontSize: typography.fontSizeProductTitle,
        fontWeight: typography.fontWeightBold,
        color: '#eaecc4',
        marginBottom: spacing.medium,
        textAlign: 'center',
    },
});

export default ProductDisplay;