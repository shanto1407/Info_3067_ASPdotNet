<template>
  <v-container fluid>
    <v-row justify="center" align="center" class="display-3">Brands</v-row>
    <v-row justify="center" class="text-center title" style="color:red">{{
      status
    }}</v-row>
    <v-row justify="center" align="center" style="margin-top:20vh;">
      <v-btn x-large outlined color="indigo" v-on:click="loadData"
        >Reset Tables</v-btn
      >
    </v-row>
    <v-footer absolute class="haeadline">
      <v-col class="text-center" cols="12">
        &copy;{{ new Date().getFullYear() }} — INFO3067
      </v-col>
    </v-footer>
  </v-container>
</template>
<script>
export default {
  name: "DataUtil",
  data() {
    return {
      status: "",
    };
  },
  methods: {
    loadData: async function() {
      // don't use arrow function here
      try {
        this.status = "please wait for tables to reload..";
        let response = await fetch(`https://localhost:44361/api/data`);
        this.status = await response.json();
      } catch (err) {
        this.status = `Error has occured: ${err.message}`;
      }
    },
  },
};
</script>
