﻿/* eslint-disable no-unused-vars, no-console */


import Vue from 'vue'
import Vuex from 'vuex'
import auth from "../auth"

Vue.use(Vuex)

export default new Vuex.Store({
    /*Stores Overall State of the App
     Documentation: https://vuex.vuejs.org/guide/state.html
     Props --> State
     */
    state: {
        clicked: false,             // for clicking network neighborhood
        itemId: '',                 // for detail and edit views
        page: '',                   // to keep track of current page
        dialogVisible: false,       // to show or hide a dialog
        dialogType: null,           // specify dialog type
        updateData: false,          // for updating tables
        username: '',               // for showing who is signed in and saving who decommissioned
        myPermissions: [],          // for tracking user permissions
        myDatacenters: [],          // for tracking datacenter permissions
        changePlan: false,          // for showing changeplan snackbar
        changePlanName: '',         // for showing name of changeplan being edited in snackbar
        changePlanId: '',
        changePlanDatacenterName: '',   // for limiting the change plan assets to a datacenter
        changePlanDatacenterDescription: '',
        changePlanDatacenterId: '',
        userId: auth.id(),          // for querying the backend for the changeplans belonging to a particular user
    },

    /*Defines Computed Properties for our Store
     Documentation: https://vuex.vuejs.org/guide/getters.html
     */
    getters: {
        getPermissions: state => {
            return state.myPermissions
        },
        hasModelPermission: state => {
            return state.myPermissions.includes("model")
        },
        hasAssetPermission: state => {
            return state.myPermissions.includes("asset")
        },
        hasPowerPermission: state => {
            return state.myPermissions.includes("power")
        },
        hasAuditPermission: state => {
            return state.myPermissions.includes("audit")
        },
        isAdmin: state => {
            return state.myPermissions.includes("admin")
        },
        hasDatacenters: state => {
            return state.myDatacenters
        },
        isChangePlan: state => {
            return state.changePlan
        },
        changePlan: state => {
            var ret = {
                name: state.changePlanName,
                id: state.changePlanId,
                datacenterName: state.changePlanDatacenterName,
                datacenterDescription: state.changePlanDatacenterDescription,
                datacenterId: state.changePlanDatacenterId,
            };
            console.log(ret);
            return ret;
        },
        userId: state => {
            console.log(state.userId);
            return state.userId
        },
    },

    /*Defines functions that Change the App State
     Documentation: https://vuex.vuejs.org/guide/mutations.html
     Events --> Mutations
     */
    mutations: {
        openDialog(state, componentName) {
            state.dialogVisible = true;
            state.dialogType = componentName;
        },
        closeDialog(state) {
            state.dialogVisible = false;
        },
        setId(state, id) {
            state.itemId = id;
        },
        changePage(state, name) {
            state.page, name;
        },
        SAVE_USER(state, username) {
            state.username = username;
        },
        SAVE_ROLES(state, roles) {
            state.myPermissions = roles;
        },
        SAVE_PERMISSIONS(state, datacenters) {
            state.myDatacenters = datacenters;
        },
        SAVE_USER_ID(state, id) {
            state.userId = id;
        },
        START_CHANGE_PLAN(state, changePlan) {
            state.changePlanName = changePlan.name;
            state.changePlan = true;
            state.changePlanDatacenterName = changePlan.datacenterName;
            state.changePlanDatacenterDescription = changePlan.datacenterDescription;
            state.changePlanDatacenterId = changePlan.datacenterId;
            state.changePlanId = changePlan.changePlanId;
        },
        CURR_CHANGE_PLAN(state, changePlan) {
            state.changePlanName = changePlan.name;
            state.changePlanDatacenterName = changePlan.datacenterName;
            state.changePlanDatacenterDescription = changePlan.datacenterDescription;
            state.changePlanDatacenterId = changePlan.datacenterId;
            state.changePlanId = changePlan.changePlanId;
        },
        END_CHANGE_PLAN(state) {
            state.changePlan = false;
        },
    },

    /*Used to Commit Mutations
     Documentation: https://vuex.vuejs.org/guide/actions.html 
     AJAX(axios?) --> Actions
     */
    actions: {
        loadUsername({ commit }) {
            commit('SAVE_USER', auth.username());
        },
        loadPermissions({ commit }) {
            Vue.axios.get(`/users/${auth.id()}/roles`).then(response => {
                commit('SAVE_ROLES', response.data);
            }).catch(error => {
                throw new Error(`API ${error}`);
            });
        },
        loadPermissionDatacenters({ commit }) {
            commit('SAVE_PERMISSIONS', auth.permissions());
        },
        // Provides the user id from the cookie to send for a changeplan
        loadUserId({ commit }) {
            commit('SAVE_USER_ID', auth.id());
            console.log(auth.id());
        },
        startChangePlan({ commit }, changePlan) {
            console.log(changePlan);
            commit('START_CHANGE_PLAN', changePlan);
        },
        saveChangePlan({ commit }, changePlan) {
            console.log(changePlan);
            commit('CURR_CHANGE_PLAN', changePlan);
        },
        endChangePlan({ commit }) {
            commit('END_CHANGE_PLAN');
        },
    },
})
