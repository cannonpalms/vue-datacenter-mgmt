﻿<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">{{formTitle}}</span>
            </v-card-title>

            <v-container>
                <v-form v-model="valid">
                    <v-row>
                        <v-col>
                            <v-text-field v-model="newItem.name"
                                          item-text="name"
                                          item-value=""
                                          :return-object="false"
                                          label="Name"
                                          placeholder="Please enter a name for this change plan"
                                          :rules="[rules.nameRules]"
                                          required></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-select v-model="selectedDatacenter"
                                      :items="filteredDatacenters"
                                      item-text="description"
                                      item-value=""
                                      :return-object="false"
                                      label="Datacenter"
                                      placeholder="Please a datacenter to apply this change plan to"
                                      >
                            </v-select>
                        </v-col>
                    </v-row>


                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn @click="close">Cancel</v-btn>
                        <v-btn color="primary" :disabled="!valid" @click="save">Save</v-btn>
                    </v-card-actions>'
                </v-form>
            </v-container>
        </v-card>
    </div>
</template>

<script>
    export default {
        name: 'change-plan-edit',
        inject: ['modelRepository','datacenterRepository','changePlanRepository'], // TODO: replace with change plan data
        props: {
            id: String,
        },
        data: () => {
            return {
                loading: false,
                datacenters: [],
                selectedDatacenter: '',
                newItem: {
                    name: '',
                    datacenterId: '',
                    datacenterName: '',
                    datacenterDescription: '',
                    createdById: '',
                },
                editedIndex: -1,
                rules: {
                    nameRules: v => /^(?=\s*\S).*$/.test(v) || 'Name is required',
                    // TODO: add rule for datacenter
                },
                valid: true,
            };
        },

        async created() {
            this.newItem = typeof this.id === 'undefined'
                ? this.newItem
                : await this.modelRepository.find(this.id); //TODO: replace with change plan data

            this.datacenters = await this.datacenterRepository.list();
        },
        computed: {
            datacenterPermissions() {
                return this.$store.getters.hasDatacenters
            },
            filteredDatacenters() {
                if (!this.datacenterPermissions.includes("All Datacenters")) {
                    var newDatacenters = []
                    for (var i = 0; i < this.datacenters.length; i++) {
                        if (this.datacenterPermissions.includes(this.datacenters[i].description)) {
                            newDatacenters.push(this.datacenters[i]);
                        }
                    }
                    return newDatacenters;
                }
                else {
                    return this.datacenters;
                }
            },
            formTitle() {
                return typeof this.id === 'undefined' ? 'New Change Plan' : 'Edit Change Plan'
            },
        },
        methods: {
            async save() {
                // TODO: save change plan
                /*eslint-disable*/
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);
                this.newItem.datacenterId = searchDatacenter.id;
                this.newItem.datacenterName = searchDatacenter.name;
                this.newItem.datacenterDescription = searchDatacenter.description;
                this.newItem.createdById = this.$store.getters.userId;
                console.log(this.newItem)
                this.changePlanRepository.create(this.newItem);
                this.close();
            },
            async close() {
                this.$router.push({ name: 'change-plan' });
            },
        }
    }
</script>
