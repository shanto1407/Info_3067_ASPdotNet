<template>
  <v-container>
    <v-row align="center" justify="center">
      <v-col class="title text-center">Map Exercise #1</v-col>
    </v-row>
    <v-row align="center" justify="center">
      <v-col class="title text-center">Get Lat and Lng From Google</v-col>
    </v-row>
    <v-row justify="center">
      <v-col>
        <v-text-field
          color="primary"
          label="Enter address"
          v-model="address"
          required
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-btn
        @click="getLatAndLng()"
        :class="{ 'purple darken-4 white--text': valid, disabled: !valid }"
        >Get lat/lng</v-btn
      >
    </v-row>
    <v-row justify="center" style="margin-top:4vh"
      >LAT:{{ lat }} - LNG:{{ lng }}</v-row
    >
  </v-container>
</template>
<script>
export default {
  data() {
    return {
      lat: 0.0,
      lng: 0.0,
      address: "N5Y-5R6",
      valid: true,
    };
  },
  methods: {
    getLatAndLng() {
      try {
        // A service for converting between an address to LatLng
        let geocoder = new window.google.maps.Geocoder();
        geocoder.geocode({ address: this.address }, (results, status) => {
          if (status === window.google.maps.GeocoderStatus.OK) {
            // only if google gives us the OK
            this.lat = results[0].geometry.location.lat();
            this.lng = results[0].geometry.location.lng();
          }
        });
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>
