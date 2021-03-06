﻿/* eslint-disable no-unused-vars, no-console */
import assetRepository from '@/repositories/asset';
import store from '@/store/store';

const getContrast50 = (hexcolor) => {
    hexcolor = hexcolor.replace('#', '');
    return (parseInt(hexcolor, 16) > 0xffffff / 2) ? 'black' : 'white';
}

export default {
    /* Function to create text and styling for each blade */
    async createSlots(assetId) {
        // call asset API
        var asset = await assetRepository.find(assetId, store.getters.changePlanId)
        var assetBlades = asset.blades;

        // create base slots
        var blades = []

        for (var i = 0; i < 14; i++) {
            var blade = {
                value: false,
                text: '',
                id: '',
                style: { backgroundColor: 'white' },
            };
            blades.push(blade);
        }

        // fill slots with blade data
        var numBlades = Object.keys(assetBlades).length;
        for (var j = 0; j < numBlades; j++) {
            var slot = assetBlades[j].chassisSlot - 1;
            var backgroundColor = assetBlades[j].customDisplayColor;
            const textColor = getContrast50(backgroundColor);
                
            blades[slot].value = true;
            blades[slot].text = assetBlades[j].assetNumber;
            blades[slot].id = assetBlades[j].id;
            blades[slot].style = { color: textColor, backgroundColor: backgroundColor };
        }
        return blades;
    },
}