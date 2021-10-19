<template>
  <v-container fluid>
    <v-row
      justify="center"
      class="text-center headline indigo--text"
      style="margin-top:2vh;"
      >Saved Trays</v-row
    >
    <v-row justify="center">
      <v-col class="title text-center" style="color:red">{{ status }}</v-col>
    </v-row>
    <div v-if="trays.length > 0">
      <v-row justify="center" style="background-color:silver;margin:3vw;">
        <v-col class="text-center headline" cols="2">Id</v-col>
        <v-col cols="6" class="text-center headline">Date</v-col>
        <v-col cols="4" class="text-left headline">Calories</v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-list
            style="max-height: 50vh;margin-top:-3vh;"
            class="overflow-y-auto"
          >
            <v-list-item-group>
              <v-list-item
                @click="popDialog(tray)"
                v-for="(tray, i) in trays"
                :key="i"
                style="border:solid;border-color:purple;margin-left:3vw;margin-right:3vw;"
              >
                <v-col cols="3">
                  <v-list-item-content>
                    <v-list-item-title class="subtitle-2" v-text="tray.id"
                      >></v-list-item-title
                    >
                  </v-list-item-content>
                </v-col>
                <v-col cols="6">
                  <v-list-item-content>
                    <v-list-item-title
                      class="sub-title2 text-left"
                      v-text="formatDate(tray.dateCreated)"
                      >></v-list-item-title
                    >
                  </v-list-item-content>
                </v-col>
                <v-col cols="3">
                  <v-list-item-content>
                    <v-list-item-title
                      class="subtitle-2"
                      v-text="tray.totalCalories"
                      >></v-list-item-title
                    >
                  </v-list-item-content>
                </v-col>
              </v-list-item>
            </v-list-item-group>
          </v-list>
        </v-col>
      </v-row>
      <v-dialog
        v-model="dialog"
        v-if="selectedTray"
        justify="center"
        align="center"
      >
        <v-card>
          <v-row>
            <v-spacer></v-spacer>
            <v-btn
              text
              @click="dialog = false"
              style="font-size:XX-large;margin:2vw;"
              >X</v-btn
            >
          </v-row>
          <v-row
            class="headline"
            justify="center"
            align="center"
            style="margin-left:3vw;margin-right:3vw;color:purple"
            >Tray - {{ selectedTray.id }} - Details</v-row
          >
          <v-row>
            <v-col justify="center" align="center">
              <v-img
                :src="require('../assets/tray.png')"
                style="height:7vh;width:10vh;padding:0;"
                aspect-ratio="1"
              />
            </v-col>
          </v-row>
          <div style="margin:2vw;">
            <v-row
              justify="center"
              style="margin-top:2vh;background-color:silver;margin:3vw;"
            >
              <v-col class="text-center title" cols="3">Qty</v-col>
              <v-col cols="9" class="text-center title">Description</v-col>
            </v-row>
            <v-row
              v-for="(detail, i) in details"
              :key="i"
              style="margin-bottom:0;margin-top:-2vh;padding-right:3vw;"
            >
              <v-col cols="3" class="text-center">{{ detail.qty }}</v-col>
              <v-col cols="9">{{ detail.description }}</v-col>
            </v-row>
          </div>
          <div style="margin:3vw;padding-left:5vw;padding-right:5vw">
            <v-row style="margin-bottom:0;margin-top:-2vh;">
              <v-col cols="9" class="text-right">Total Calories:</v-col>
              <v-col cols="3" class="text-right">{{
                selectedTray.totalCalories
              }}</v-col>
            </v-row>
            <v-row style="margin-bottom:0;margin-top:-2vh;">
              <v-col cols="9" class="text-right">Total Fat:</v-col>
              <v-col cols="3" class="text-right">{{
                selectedTray.totalFat
              }}</v-col>
            </v-row>
            <v-row style="margin-bottom:0;margin-top:-2vh;">
              <v-col cols="9" class="text-right">Total Salt:</v-col>
              <v-col cols="3" class="text-right">{{
                selectedTray.totalSalt
              }}</v-col>
            </v-row>
            <v-row style="margin-bottom:0;margin-top:-2vh;">
              <v-col cols="9" class="text-right">Total Cholesterol:</v-col>
              <v-col cols="3" class="text-right">{{
                selectedTray.totalCholesterol
              }}</v-col>
            </v-row>
          </div>
          <v-row justify="center" align="center" style="padding-bottom:5vh;">{{
            this.dialogStatus
          }}</v-row>
        </v-card>
      </v-dialog>
    </div>
  </v-container>
</template>
<script>
import fetcher from "../mixins/fetcher";
import datertn from "../mixins/datertn";
export default {
  name: "TrayList",
  data() {
    return {
      trays: [],
      status: {},
      details: [],
      selectedTray: {},
      dialog: false,
      dialogStatus: {},
    };
  },
  mixins: [fetcher, datertn],
  beforeMount: async function() {
    try {
      let user = JSON.parse(sessionStorage.getItem("user"));
      this.status = "fetching trays from server...";
      let route = this.$_buildRouteWithParams("tray", user.email.trimEnd());
      this.trays = await this.$_getdata(route.slice(0, -1)); // in mixin
      this.status = `loaded ${this.trays.length + 1} trays`;
    } catch (err) {
      console.log(err);
      this.status = `Error has occured: ${err.message}`;
    }
  },
  methods: {
    selectTray: async function(trayid) {
      if (trayid > 0) {
        // don't use arrow function here
        try {
          let user = JSON.parse(sessionStorage.getItem("user"));
          this.status = `fetching details for tray ${trayid}...`;
          let route = this.$_buildRouteWithParams(
            "tray",
            trayid,
            user.email.trimEnd()
          );
          this.details = await this.$_getdata(route.slice(0, -1)); // remove end /
          this.status = `found tray ${trayid} details`;
        } catch (err) {
          console.log(err);
          this.status = `Error has occured: ${err.message}`;
        }
      }
    },
    popDialog: async function(tray) {
      this.dialogStatus = "";
      this.dialog = !this.dialog;
      this.selectedTray = tray;
      await this.selectTray(tray.id);
    },
  },
};
</script>
