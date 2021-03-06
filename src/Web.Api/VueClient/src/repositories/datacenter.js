﻿/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/datacenters';
export default {
    find(id) {
        return axios.get(`${resource}/${id}`)
            .then(response => {
                return response.data;
            });
    },
    networkPorts(id) {
        return axios.get(`${resource}/${id}/networkports`)
            .then(response => { return response.data; }).catch(error => error);
    },
    chassis(id, changePlanId) {
        var query = {
            assetId: id,
            changePlanId: changePlanId
        };
        return axios.get(`${resource}/chassis`, { params: query })
            .then(response => {
            return response.data;
        }).catch(error => error);
    },
    openNetworkPorts(id) {
        return axios.get(`${resource}/${id}/networkports/open`)
            .then(response => response.data);
    },
    list() {
        return axios.get(`${resource}`, { params: { pageSize: 2000000000 } })
            .then(response => {
                return response.data.data;
            });
    },
    listOffline() {
        return axios.get(`${resource}/offline`, { params: { pageSize: 2000000000 } })
            .then(response => {
                return response.data.data;
            });
    },
    create(item) {
        return axios.post(`${resource}`, item).then(response => response.data).catch(error => error);
    },
    update(item) {
        return axios.put(`${resource}`, item).then(response => response.data).catch(error => error);
    },
    delete(item) {
        return axios.delete(`${resource}/${item.id}`)
            .then(response => {
                return response.data;
            }).catch(error => error);
    },

}