<template>
  <v-container>
    <v-row justify="center">
      <v-col class="display-1 text-center">Tray Contents</v-col>
    </v-row>
    <v-row>
      <v-col justify="center" align="center">
        <v-img
          :src="require('../assets/tray.png')"
          style="height:8vh;width:12vh;"
          aspect-ratio="1"
        />
      </v-col>
    </v-row>
    <v-row style="margin:2vw;">{{ status }}</v-row>
    <div v-if="this.status !== 'tray cleared'">
      <v-simple-table>
        <thead>
          <tr>
            <th class="text-left">Qty</th>
            <th class="text-left">Description</th>
          </tr>
        </thead>
      </v-simple-table>
      <v-simple-table
        style="max-height: 12vh;margin-bottom:5vh;"
        class="overflow-y-auto"
      >
        <tbody>
          <tr v-for="trayItem in tray" :key="trayItem.id">
            <td>{{ trayItem.qty }}</td>
            <td>{{ trayItem.item.description }}</td>
          </tr>
        </tbody>
      </v-simple-table>
      <v-simple-table style="border:solid;border-width:thin;">
        <thead>
          <tr>
            <th colspan="4" class="text-left title">Tray Totals</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>Protein:</td>
            <td>{{ proTot }}</td>
            <td>Calories:</td>
            <td>{{ calTot }}</td>
          </tr>
          <tr>
            <td>Carbs:</td>
            <td>{{ carbTot }}</td>
            <td>Chol:</td>
            <td>{{ cholTot }}</td>
          </tr>
          <tr>
            <td>Fat:</td>
            <td>{{ fatTot }}</td>
            <td>Fibre:</td>
            <td>{{ fbrTot }}</td>
          </tr>
          <tr>
            <td>Salt:</td>
            <td>{{ saltTot }}</td>
            <td>
              <v-btn medium outlined color="default" @click="clearTray"
                >Clear Tray</v-btn
              >
            </td>
            <td>
              <v-btn medium color="primary" class="text--white" @click="addTray"
                >Save</v-btn
              >
            </td>
          </tr>
        </tbody>
      </v-simple-table>
    </div>
  </v-container>
</template>
<script>
import fetcher from "../mixins/fetcher";
export default {
  name: "TrayDetails",
  data() {
    return {
      fbrTot: 0,
      calTot: 0,
      saltTot: 0,
      fatTot: 0,
      cholTot: 0,
      proTot: 0,
      carbTot: 0,
      tray: [],
      status: "",
    };
  },
  beforeMount: function() {
    if (sessionStorage.getItem("tray")) {
      this.tray = JSON.parse(sessionStorage.getItem("tray"));
      this.tray.map((trayItem) => {
        this.fbrTot += trayItem.item.fibre * trayItem.qty;
        this.calTot += trayItem.item.calories * trayItem.qty;
        this.saltTot += trayItem.item.salt * trayItem.qty;
        this.fatTot += trayItem.item.fat * trayItem.qty;
        this.cholTot += trayItem.item.cholesterol * trayItem.qty;
        this.proTot += trayItem.item.protein * trayItem.qty;
        this.carbTot += trayItem.item.carbs * trayItem.qty;
      });
    } else {
      this.tray = [];
    }
  },
  mixins: [fetcher],
  methods: {
    clearTray: function() {
      sessionStorage.removeItem("tray");
      this.tray = [];
      this.status = "tray cleared";
    },
    addTray: async function() {
      let user = JSON.parse(sessionStorage.getItem("user"));
      let tray = JSON.parse(sessionStorage.getItem("tray"));
      try {
        this.status = "sending tray info to server";
        let trayHelper = { email: user.email, selections: tray };
        let payload = await this.$_postdata("tray", trayHelper); // in mixin
        if (payload.indexOf("not") > 0) {
          this.status = payload;
        } else {
          this.clearTray();
          this.status = payload;
        }
      } catch (err) {
        console.log(err);
        this.status = `Error add tray: ${err}`;
      }
    },
  },
};
</script>
