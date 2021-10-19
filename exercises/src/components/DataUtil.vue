<template>
  <v-container fluid>
    <v-row justify="center" align="center" class="display-3"
      >Load Utilities</v-row
    >
    <v-row justify="center" class="text-center title" style="color:red">{{
      status
    }}</v-row>
    <v-row justify="center" align="center" style="margin-top:20vh;">
      <v-btn x-large outlined color="indigo" @click="loadCategoriesAndItems"
        >Categories and Items</v-btn
      >
    </v-row>
    <v-row justify="center" align="center" style="margin-top:20vh;">
      <v-btn x-large outlined color="indigo" @click="loadStores">Store</v-btn>
    </v-row>
    <v-footer absolute class="headline">
      <v-col class="text-center" cols="12">
        &copy;{{ new Date().getFullYear() }} — INFO3067
      </v-col>
    </v-footer>
  </v-container>
</template>
<script>
import fetcher from "../mixins/fetcher";
export default {
  name: "DataUtil",
  data() {
    return {
      status: "",
    };
  },
  mixins: [fetcher],
  methods: {
    loadCategoriesAndItems: async function() {
      // don't use arrow function here
      try {
        this.status = "please wait for tables to reload..";
        this.status = await this.$_getdata("data"); // fetchdata is in mixin
      } catch (err) {
        this.status = `Error has occured: ${err.message}`;
      }
    },
    loadStores: async function() {
      // don't use arrow function here
      try {
        this.status = "please wait for stores ";
        this.status = await this.$_getdata("data/loadcsv"); // fetchdata is in mixin
      } catch (err) {
        this.status = `Error has occured: ${err.message}`;
      }
    },
  },
};
</script>
