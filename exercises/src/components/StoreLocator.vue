<template>
  <v-container>
    <v-row align="center" justify="center">
      <v-col class="title text-center"
        >Enter Postal Code To See Nearest Location</v-col
      >
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
        >Show Map</v-btn
      >
    </v-row>
    <v-row justify="center">
      <div
        id="map"
        ref="map"
        style="margin-top:2vh;margin-bottom:5vh;height:250px;width:300px;min-height:300px;"
      ></div>
    </v-row>
    <v-row
      class="display-1"
      justify="center"
      align="center"
      style="margin-top:2vh;color:purple"
      >{{ status }}</v-row
    >
  </v-container>
</template>
<script>
export default {
  data() {
    return {
      status: "",
      address: "N5Y-5R6",
      valid: true,
      map: null,
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
            let myLatLng = new window.google.maps.LatLng(this.lat, this.lng);
            let options = {
              zoom: 10,
              center: myLatLng,
              mapTypeId: window.google.maps.MapTypeId.ROADMAP,
            };
            this.map = new window.google.maps.Map(this.$refs["map"], options);
            let center = this.map.getCenter();
            this.map.setCenter(center);
            window.google.maps.event.trigger(this.map, "resize");
          }
        });
      } catch (err) {
        console.log(err);
      }
    },
  },
};
</script>
