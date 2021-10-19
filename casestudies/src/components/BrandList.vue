<template>
  <v-container fluid>
    <v-row
      justify="center"
      class="text-center display-2"
      style="margin-top:.5em;"
      >Brands</v-row
    >
    <v-row justify="center">
      <v-col class="title text-center" style="color:red">{{ status }}</v-col>
    </v-row>
    <v-row justify="center">
      <v-col class="text-left display-1">
        <v-select
          :items="Brands"
          item-value="id"
          @input="changeBrand"
          v-model="selectedid"
          item-text="name"
          style="max-height: 50%;"
        ></v-select>
      </v-col>
    </v-row>
    <div v-if="products.length > 0">
      <v-row justify="center" class="text-center headline"
        >Producsts in Brands</v-row
      >
      <v-row justify="center" style="margin-top:1vh;">
        <v-col class="text-left display-2">
          <v-list style="max-height: 60vh;" class="overflow-y-auto">
            <v-list-item-group>
              <v-list-item
                v-for="(item, i) in products"
                :key="i"
                style="border:solid; "
                @click="popDialog(item)"
              >
                <v-col style="width:25%;">
                  <v-img
                    :src="require(`../assets/${item.graphicName}`)"
                    class="my-3"
                    style="height:10vh;width:10vh;"
                    aspect-ratio="1"
                  />
                </v-col>
                <v-col style="width:75%;">
                  <v-list-item-content>
                    <v-list-item-title class="title" v-text="item.productName"
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
        v-if="selectedProducts"
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
          <v-row class="title" style="margin-left:3vw;margin-right:3vw;">
            <v-col>{{ selectedProducts.productName }}</v-col>
            <v-col>
              <v-img
                v-if="selectedProducts.graphicName"
                :src="require(`../assets/${selectedProducts.graphicName}`)"
                height="15vh"
                width="15vh"
                contain
                aspect-ratio="1"
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col>MSRP: {{ selectedProducts.msrp | currency }}</v-col>
          </v-row>
          <v-row justify="center" class="title">Brand Description</v-row>
          <div
            style="margin:3vw;border:none;padding-left:5vw;padding-right:5vw;"
          >
            <v-row>
              <v-col>Description:</v-col>
            </v-row>
            <v-row>
              <v-col>{{ selectedProducts.description }}</v-col>
            </v-row>
            <v-row style="margin-left:3vw;">
              <v-col>Qty:</v-col>
              <v-col>
                <input
                  type="number"
                  maxlength="3"
                  placeholder="enter qty"
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
                <v-btn medium outlined color="default" @click="addToCart"
                  >Add To Cart</v-btn
                >
              </v-col>
              <v-col>
                <v-btn medium outlined color="default" @click="viewCart"
                  >View Cart</v-btn
                >
              </v-col>
            </v-row>
            <v-row
              justify="center"
              align="center"
              style="padding-bottom:5vh;"
              >{{ this.dialogStatus }}</v-row
            >
          </div>
        </v-card>
      </v-dialog>
    </div>

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
  name: "BrandList",
  data() {
    return {
      Brands: [],
      status: {},
      selectedid: 0,
      products: [],
      dialog: false,
      selectedProducts: {},
      dialogStatus: "",
      qty: 0,
      cart: [],
    };
  },
  mixins: [fetcher],
  mounted: async function() {
    try {
      this.status = "fetching Brands from server...";
      this.Brands = await this.$_getdata("brand"); // $_getdata, is in mixin
      this.status = `loaded ${this.Brands.length + 1} Brands`;
      this.Brands.unshift({ name: "Current Brands", id: 0 });
    } catch (err) {
      console.log(err);
      this.status = `Error has occured: ${err.message}`;
    }
    if (sessionStorage.getItem("cart")) {
      this.cart = await JSON.parse(sessionStorage.getItem("cart"));
    }
  },
  methods: {
    changeBrand: async function(brandid) {
      if (brandid > 0) {
        // don't use arrow function here
        try {
          this.status = `fetching items for ${brandid}...`;
          this.products = await this.$_getdata(`product/${brandid}`);
          this.status = `found ${this.products.length} items`;
        } catch (err) {
          console.log(err);
          this.status = `Error has occured: ${err.message}`;
        }
      }
    },
    popDialog: function(item) {
      this.dialogStatus = "";
      this.dialog = !this.dialog;
      this.selectedProducts = item;
    },
    addToCart: function() {
      const index = this.cart.findIndex(
        // is item already on the tray
        (item) => item.id === this.selectedProducts.id
      );
      if (this.qty !== "0") {
        index === -1
          ? this.cart.push({
              qty: parseInt(this.qty),
              item: this.selectedProducts,
            }) // add
          : (this.cart[index] = {
              // replace
              qty: parseInt(this.qty),
              item: this.selectedProducts,
            });
        this.dialogStatus = `${this.qty} item(s) added`;
      } else {
        index === -1 ? null : this.cart.splice(index, 1); // remove
        this.dialogStatus = `item(s) removed`;
      }
      sessionStorage.setItem("cart", JSON.stringify(this.cart));
    },
    viewCart: function() {
      this.$router.push({
        name: "cart",
      });
    },
  },
};
</script>
