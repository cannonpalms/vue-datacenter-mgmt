<template>
    <div v-if="!loading">
        <v-card flat>
            <ChangePlanBar></ChangePlanBar>
            <v-card-title>
                <span class="headline">Asset Details</span>
            </v-card-title>
            <v-card-text>
                <v-row v-if="isDecommissioned" >
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Time Decommissioned</v-label>
                        <v-card-text> {{asset.dateDecommissioned}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Decommissioned By</v-label>
                        <v-card-text> {{asset.decommissioner}} </v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model Vendor</v-label>
                        <v-card-text> {{asset.vendor}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model</v-label>
                        <v-card-text>
                            <router-link :to="{ name: 'model-details', params: { id: asset.modelId } }"> {{ asset.modelNumber }} </router-link>
                        </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Host Name</v-label>
                        <v-card-text> {{asset.hostname}} </v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label v-if="type==='offline'">Offline Storage Site</v-label>
                        <v-label v-else>Datacenter</v-label>
                        <v-card-text> {{asset.datacenter}} </v-card-text>
                    </v-col>
                    <v-col v-if="type!='offline'">
                        <v-label>Location</v-label>
                        <v-card-text v-if="!isBlade"> Rack {{asset.rack}}, Rack Position {{asset.rackPosition}} </v-card-text>
                        <v-card-text v-else> Chassis {{asset.chassisHostname}}, Slot {{asset.chassisSlot}} </v-card-text>
                    </v-col>
                    <v-col v-if="isBlade">
                        <v-label>Power Status</v-label>
                        <v-card-text>{{ bladePowerStatus }}</v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Asset Number</v-label>
                        <v-card-text> {{asset.assetNumber}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Owner Username</v-label>
                        <v-card-text v-if="ownerPresent"> {{asset.owner}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Comment</v-label>
                        <v-textarea :value="asset.comment" disabled>  </v-textarea>
                    </v-col>
                </v-row>
                <v-container>
                    <v-row>
                        <v-col cols="12" sm="6" md="3">
                            <v-row>
                                <v-label>CPU </v-label>
                                <!--TODO: v-if data is different from model data-->
                                <v-tooltip bottom v-if="asset.customCpu !== assetModel.cpu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn class="pb-4"
                                               color="primary"
                                               icon
                                               v-on="on"
                                               :to="{ name: 'model-details', params: { id: asset.modelId } }">
                                            <v-icon>
                                                mdi-open-in-new
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <span>Go to model details to view original attribute</span>
                                </v-tooltip>
                            </v-row>

                            <v-card-text v-if="asset.customCpu !== assetModel.cpu"> {{asset.customCpu}} </v-card-text>
                            <v-card-text v-else> {{assetModel.cpu}} </v-card-text>
                        </v-col>
                        <v-col cols="12" sm="6" md="3">
                            <v-row>
                                <v-label>Memory </v-label>
                                <!--TODO: v-if data is different from model data-->
                                <v-tooltip bottom v-if="asset.customMemory !== assetModel.memory">
                                    <template v-slot:activator="{ on }">
                                        <v-btn class="pb-4"
                                               color="primary"
                                               icon
                                               v-on="on"
                                               :to="{ name: 'model-details', params: { id: asset.modelId } }">
                                            <v-icon>
                                                mdi-open-in-new
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <span>Go to model details to view original attribute</span>
                                </v-tooltip>
                            </v-row>
                            <v-card-text v-if="asset.customMemory !== assetModel.memory"> {{asset.customMemory}} </v-card-text>
                            <v-card-text v-else> {{assetModel.memory}} </v-card-text>

                        </v-col>
                        <v-col cols="12" sm="6" md="3">
                            <v-row>
                                <v-label>Storage </v-label>
                                <!--TODO: v-if data is different from model data-->
                                <v-tooltip bottom v-if="asset.customStorage !== assetModel.storage">
                                    <template v-slot:activator="{ on }">
                                        <v-btn class="pb-4"
                                               color="primary"
                                               icon
                                               v-on="on"
                                               :to="{ name: 'model-details', params: { id: asset.modelId } }">
                                            <v-icon>
                                                mdi-open-in-new
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <span>Go to model details to view original attribute</span>
                                </v-tooltip>
                            </v-row>
                            <v-card-text v-if="asset.customStorage !== assetModel.storage"> {{asset.customStorage}} </v-card-text>
                            <v-card-text v-else> {{assetModel.storage}} </v-card-text>
                        </v-col>
                        <v-col cols="12" sm="6" md="3">
                            <v-row>
                                <v-label>Display Color </v-label>
                                <!--TODO: v-if data is different from model data-->
                                <v-tooltip bottom v-if="asset.customDisplayColor !== assetModel.displayColor">
                                    <template v-slot:activator="{ on }">
                                        <v-btn class="pb-4"
                                               color="primary"
                                               icon
                                               v-on="on"
                                               :to="{ name: 'model-details', params: { id: asset.modelId } }">
                                            <v-icon>
                                                mdi-open-in-new
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <span>Go to model details to view original attribute</span>
                                </v-tooltip>
                            </v-row>
                            <v-card-text v-if="asset.customDisplayColor !== assetModel.displayColor">
                                {{asset.customDisplayColor}}
                                <v-icon class="mr-2"
                                        :color=asset.customDisplayColor>
                                    mdi-circle
                                </v-icon>
                            </v-card-text>
                            <v-card-text v-else>
                                {{assetModel.displayColor}}
                                <v-icon class="mr-2"
                                        :color=assetModel.displayColor>
                                    mdi-circle
                                </v-icon>
                            </v-card-text>
                        </v-col>
                    </v-row>
                </v-container>
                
                <!--NEW port detail page so we can exclude altogether for different kinds of assets-->
                <!--TODO: make show connections false for offline assets-->
                <PortDetails v-if="!isBlade"
                             :asset="asset" 
                             :id="id" 
                             :type="type"></PortDetails> 
                <!--Blade Chassis Diagram -->
                <div v-if="isBlade || isChassis">
                    <v-label>Blade Diagram</v-label>
                    <v-card-text>
                        <blade-diagram :type="type"
                                       :chassisId="asset.chassisId"
                                       :assetId="asset.id"
                                       :mountType="mountType"></blade-diagram>
                    </v-card-text>
                    <v-card-text v-if="isBlade">
                        <v-btn color="primary"
                               outlined
                               @click="toChassisDetails">View Blade Chassis Details</v-btn>
                    </v-card-text>
                </div>
            </v-card-text>

            <v-spacer />

            <!--Back button to return to main page-->
            <v-spacer></v-spacer>
            <a href="javascript:history.go(-1)">Go Back</a>

        </v-card>
    </div>

</template>

<style>
    .v-label {
        font-size: 20px;
    }
    .p {
        font-size: 15px;
    }
</style>

<script>
    import ChangePlanBar from "@/components/ChangePlanStatusBar"
    import PortDetails from '@/components/AssetPortDetails'
    import BladeDiagram from '@/components/BladeDiagram'

    export default {
        name: 'asset-details',
        inject: ['assetRepository', 'modelRepository', 'rackRepository'],
        item: null,
        components: {
            ChangePlanBar,
            PortDetails,
            BladeDiagram
        },
        props: {
            id: String,
            cpId: String,
            type: String,
        },
        data() {
            return {
                loading: false,
                asset: {
                    id:'',
                    hostname: '',
                    rack: '',
                    rackPosition: '',
                    owner: '',
                    comment: '',
                    vendor: '',
                    modelNumber: '',
                    customCpu: '',
                    customMemory: '',
                    customStorage: '',
                    customDisplayColor: '',
                },
                assetModel: '',
                ownerPresent: true, // in case the asset does not have an owner, don't need null pointer bc not a required field.
                isDecommissioned: false,
                mountType: '',
                bladePowerStatus: null
            };
        },
        computed: {
            isBlade() {
                return this.mountType === 'blade'
            },
            isChassis() {
                return this.mountType === 'chassis'
            }
        },
        created() {
            this.initialize();
        },
        beforeRouteUpdate(to, from, next) {
            /*eslint-disable*/
            this.id = to.params.id;
            this.$route.params.id = to.params.id;
            this.initialize();

            console.log(from);
            next()
        },
        methods: {
            changePlanId() {
                if (this.$store.getters.isChangePlan)
                    return this.$store.getters.changePlan.id;
            },
            async initialize() {
                // TODO: get customizable asset information when integrating
                /*eslint-disable*/
                if (!this.loading) this.loading = true;
                const asset = await this.assetRepository.find(this.id, this.changePlanId())
                asset.powerPorts.forEach(port => port.status = undefined);
                asset.networkPorts.sort((a, b) => a.number - b.number);
                asset.powerPorts.sort((a, b) => a.number - b.number);

                if (asset.mountType === 'blade') {
                    this.bladePowerStatus = (await this.assetRepository.getPowerPortState(asset.id)).powerPorts[0].status === 0
                        ? 'on'
                        : 'off';
                }
                    

                const model = await this.modelRepository.find(asset.modelId);
                this.mountType = model.mountType;
                this.assetModel = model;
                console.log(this.mountType)

                this.asset = asset;
                
                if (this.asset.owner === undefined) {
                    this.ownerPresent = false;
                }
                if (this.asset.networkPortGraph !== undefined) {
                    this.isDecommissioned = true;
                }

                this.loading = false;
            },
            toChassisDetails() {
                console.log("new route to chassis")
                this.$router.push({ name: 'asset-details', params: { type: this.type, id: this.asset.chassisId } })
            }
        }
    }
</script>