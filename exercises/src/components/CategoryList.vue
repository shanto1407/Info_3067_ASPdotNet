/**\ Color: https://vuetifyjs.com/en/styles/colors/ */
<template>
  <v-container fluid>
    <v-row
      justify="center"
      class="text-center display-2"
      style="margin-top:.5em;"
      >Categories</v-row
    >
    <v-row justify="center">
      <v-col class="title text-center" style="color:red">{{ status }}</v-col>
    </v-row>
    <v-row justify="center">
      <v-col class="text-left display-1">
        <v-select
          :items="categories"
          item-value="id"
          @input="changeCategory"
          v-model="selectedid"
          item-text="name"
          style="max-height: 50%;"
        ></v-select>
      </v-col>
    </v-row>
    <div v-if="menuitems.length > 0">
      <v-row justify="center" class="text-center headline"
        >Items in Category</v-row
      >
      <v-row justify="center" style="margin-top:1vh;">
        <v-col class="text-left display-2">
          <v-list style="max-height: 60vh;" class="overflow-y-auto">
            <v-list-item-group>
              <v-list-item
                v-for="(item, i) in menuitems"
                :key="i"
                style="border:solid;"
                @click="popDialog(item)"
              >
                <v-col style="width:25%;">
                  <v-img
                    :src="require('../assets/burger.jpg')"
                    class="my-3"
                    style="height:10vh;width:10vh;"
                    aspect-ratio="1"
                  />
                </v-col>
                <v-col style="width:75%;">
                  <v-list-item-content>
                    <v-list-item-title class="title" v-text="item.description"
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
        v-if="selectedMenuItem"
        justify="center"
        align="center"
      >
        <v-card>
          <v-row>
            <v-spacer></v-spacer>
            <v-btn
              text
              @click="dialog = false"
              style="font-size:XX-large;margin:1vw;"
              >X</v-btn
            >
          </v-row>
          <v-row
            class="title"
            justify="center"
            align="center"
            style="margin-left:3vw;margin-right:3vw;"
            >{{ selectedMenuItem.description }}</v-row
          >
          <v-row>
            <v-img
              :src="require('../assets/burger.jpg')"
              height="15vh"
              width="15vh"
              contain
              aspect-ratio="1"
            />
          </v-row>
          <v-row justify="center" class="title">Nutrional Information</v-row>
          <div
            style="margin:3vw;border:ridge;padding-left:5vw;padding-right:5vw;"
          >
            <v-row>
              <v-col>Protein:</v-col>
              <v-col>{{ selectedMenuItem.protein }}</v-col>
              <v-col>Calories:</v-col>
              <v-col>{{ selectedMenuItem.calories }}</v-col>
            </v-row>
            <v-row>
              <v-col>Carbs:</v-col>
              <v-col>{{ selectedMenuItem.carbs }}</v-col>
              <v-col>Chol:</v-col>
              <v-col>{{ selectedMenuItem.cholesterol }}</v-col>
            </v-row>
            <v-row>
              <v-col>Fat:</v-col>
              <v-col>{{ selectedMenuItem.fat }}</v-col>
              <v-col>Fibre:</v-col>
              <v-col>{{ selectedMenuItem.fibre }}</v-col>
            </v-row>
            <v-row>
              <v-col>Salt:</v-col>
              <v-col>{{ selectedMenuItem.salt }}</v-col>
              <v-col></v-col>
              <v-col></v-col>
            </v-row>
          </div>
          <v-row style="margin-left:3vw;">
            <v-col>Qty:</v-col>
            <v-col>
              <input
                type="number"
                maxlength="3"
                placeholder="1"
                size="3"
                style="width: 15vw;border-bottom:solid;text-align:right"
                v-model="qty"
              />
            </v-col>
            <v-col cols="7"></v-col>
          </v-row>
          <v-row
            justify="center"
            align="center"
            style="margin-bottom:2vh;margin-left:3vw;"
          >
            <v-col>
              <v-btn medium outlined color="default" @click="addToTray"
                >Add To Tray</v-btn
              >
            </v-col>
            <v-col>
              <v-btn medium outlined color="default" @click="viewTray"
                >View Tray</v-btn
              >
            </v-col>
          </v-row>

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
export default {
  name: "CategoryList",
  data() {
    return {
      categories: [],
      status: {},
      selectedid: 0,
      menuitems: [],
      dialog: false,
      selectedMenuItem: {},
      dialogStatus: "",
      qty: 0,
      tray: [],
    };
  },
  mixins: [fetcher],
  mounted: async function() {
    try {
      this.status = "fetching categories from server...";
      this.categories = await this.$_getdata("category"); // $_getdata is in mixin
      this.status = `loaded ${this.categories.length + 1} categories`;
      this.categories.unshift({ name: "Current Categories", id: 0 });
    } catch (err) {
      console.log(err);
      this.status = `Error has occured: ${err.message}`;
    }
    if (sessionStorage.getItem("tray")) {
      this.tray = await JSON.parse(sessionStorage.getItem("tray"));
    }
  },
  methods: {
    changeCategory: async function(categoryid) {
      if (categoryid > 0) {
        // don't use arrow function here
        try {
          this.status = `fetching items for ${categoryid}...`;
          this.menuitems = await this.$_getdata(`menuitem/${categoryid}`);
          this.status = `found ${this.menuitems.length} items`;
        } catch (err) {
          console.log(err);
          this.status = `Error has occured: ${err.message}`;
        }
      }
    },
    popDialog: function(item) {
      this.dialogStatus = "";
      this.dialog = !this.dialog;
      this.selectedMenuItem = item;
    },
    addToTray: function() {
      const index = this.tray.findIndex(
        // is item already on the tray
        (item) => item.id === this.selectedMenuItem.id
      );
      if (this.qty !== "0") {
        index === -1
          ? this.tray.push({
              qty: parseInt(this.qty),
              item: this.selectedMenuItem,
            }) // add
          : (this.tray[index] = {
              // replace
              qty: parseInt(this.qty),
              item: this.selectedMenuItem,
            });
        this.dialogStatus = `${this.qty} item(s) added`;
      } else {
        index === -1 ? null : this.tray.splice(index, 1); // remove
        this.dialogStatus = `item(s) removed`;
      }
      sessionStorage.setItem("tray", JSON.stringify(this.tray));
    },
    viewTray: function() {
      this.$router.push({
        name: "tray",
      });
    },
  },
};
</script>
