import Vue from 'vue'
import './plugins/axios'
import App from './App.vue'
import vuetify from './plugins/vuetify';
import VuetifyDialog from "vuetify-dialog";
import axios from 'axios';
import router from './router';
import serviceContainer from './service-container';
import JsonCSV from 'vue-json-csv'
import store from './store/store'
import VueResizeText from 'vue-resize-text';


Vue.component('downloadCsv', JsonCSV)
Vue.config.productionTip = false;
Vue.use(vuetify);
Vue.use(VuetifyDialog);
Vue.use(VueResizeText);
Vue.config.productionTip = false;

axios.defaults.baseURL = '/api';

new Vue({
  el: '#app',
  router,
  template: '<App/>',
  components: {App},
  vuetify,
  provide: serviceContainer,
  store,
  render: h => h(App),
}).$mount('#app')
 