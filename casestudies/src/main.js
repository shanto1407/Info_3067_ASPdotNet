import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import VueCurrencyFilter from "vue-currency-filter";
import { router } from "./router";

Vue.config.productionTip = false;

Vue.use(VueCurrencyFilter, {
  symbol: "$",
  thousandsSeparator: ",",
  fractionCount: 2,
  fractionSeparator: ".",
  symbolPosition: "front",
  symbolSpacing: false,
});

new Vue({
  vuetify,
  router,
  render: (h) => h(App),
}).$mount("#app");
