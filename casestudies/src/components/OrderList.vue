<template>
  <v-container fluid>
    <v-row
      justify="center"
      class="text-center headline indigo--text"
      style="margin-top:2vh;"
      >Previous Orders</v-row
    >
    <v-row justify="center">
      <v-col class="title text-center" style="color:red">{{ status }}</v-col>
    </v-row>
    <div v-if="orders.length > 0">
      <v-row justify="center" style="background-color:red;margin:3vw;">
        <v-col class="text-left headline" cols="4">Id</v-col>
        <v-col cols="8" class="text-left headline">Date</v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-list
            style="max-height: 50vh;margin-top:-3vh;"
            class="overflow-y-auto"
          >
            <v-list-item-group>
              <v-list-item
                @click="popDialog(order)"
                v-for="(order, i) in orders"
                :key="i"
                style="border:solid;border-color:red;margin-left:3vw;margin-right:3vw;"
              >
                <v-col cols="3">
                  <v-list-item-content>
                    <v-list-item-title class="subtitle-2" v-text="order.id"
                      >></v-list-item-title
                    >
                  </v-list-item-content>
                </v-col>
                <v-col cols="10">
                  <v-list-item-content>
                    <v-list-item-title
                      class="sub-title2 text-left"
                      v-text="formatDate(order.dateCreated)"
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
        v-if="selectedOrder"
        justify="center"
        align="center"
      >
        <v-card height="560">
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
            style="margin-left:3vw;margin-right:3vw;color:green"
            >Order - {{ selectedOrder.id }} :
            {{ formatDate(selectedOrder.orderDate) }}</v-row
          >
          <v-row>
            <v-col justify="center" align="center">
              <v-img
                :src="require('../assets/cart.png')"
                style="height:12vh;width:12vh;padding:0;"
                aspect-ratio="1"
              />
            </v-col>
          </v-row>
          <div style="margin:0vw;">
            <v-row
              justify="center"
              style="margin-top:5vh;background-color:red;margin:3vw;"
            >
              <v-col class="text-left" cols="3">Name</v-col>
              <v-col class="text-left" cols="3">MSRP</v-col>
              <v-col class="text-left" cols="1">S</v-col>
              <v-col class="text-left" cols="1">O</v-col>
              <v-col class="text-left" cols="1">B</v-col>
              <v-col class="text-left" cols="3">Ext</v-col>
            </v-row>
          </div>
          <v-row
            v-for="(detail, i) in details"
            :key="i"
            style="margin-bottom:0;margin-top:2vh;margin:3vw;"
          >
            <v-col class="caption" cols="3">{{ detail.productName }}</v-col>
            <v-col class="caption" cols="3">{{ detail.productPrice }}</v-col>
            <v-col class="caption" cols="1">{{ detail.qtySold }}</v-col>
            <v-col class="caption" cols="1">{{ detail.qtyOrder }}</v-col>
            <v-col class="caption" cols="1">{{ detail.qtyBackOrder }}</v-col>
            <v-col class="caption" cols="3">{{
              (detail.productPrice * detail.qtySold) | currency
            }}</v-col>
          </v-row>
          <v-simple-table
            class="text-left"
            style="margin-left:30vw; border-top:2vw;justify:right;"
          >
            <thead>
              <tr>
                <th>Sub Totals:</th>
                <th>{{ selectedOrder.orderAmount | currency }}</th>
              </tr>
              <tr>
                <th>tax</th>
                <th>{{ (selectedOrder.orderAmount * 0.13) | currency }}</th>
              </tr>
              <tr>
                <th style="bold">Total After Tax</th>
                <th>{{ (selectedOrder.orderAmount * 1.13) | currency }}</th>
              </tr>
            </thead>
          </v-simple-table>
        </v-card>
      </v-dialog>
    </div>
  </v-container>
</template>
<script>
import fetcher from "../mixins/fetcher";
import datertn from "../mixins/datertn";
export default {
  name: "OrderList",
  data() {
    return {
      orders: [],
      status: {},
      details: [],
      selectedOrder: {},
      dialog: false,
      dialogStatus: {},
    };
  },
  mixins: [fetcher, datertn],
  beforeMount: async function() {
    try {
      let customer = JSON.parse(sessionStorage.getItem("customer"));
      this.status = "fetching orders from server...";
      let route = this.$_buildRouteWithParams(
        "order",
        customer.email.trimEnd()
      );
      this.orders = await this.$_getdata(route.slice(0, -1)); // in mixin
      this.status = `loaded ${this.orders.length} orders`;
    } catch (err) {
      console.log(err);
      this.status = `Error has occured: ${err.message}`;
    }
  },
  methods: {
    selectOrder: async function(orderid) {
      if (orderid > 0) {
        // don't use arrow function here
        try {
          let customer = JSON.parse(sessionStorage.getItem("customer"));
          this.status = `fetching details for order ${orderid}...`;
          let route = this.$_buildRouteWithParams(
            "order",
            orderid,
            customer.email.trimEnd()
          );
          this.details = await this.$_getdata(route.slice(0, -1)); // remove end /
          //this.status = `found order ${orderid} details`;
        } catch (err) {
          console.log(err);
          this.status = `Error has occured: ${err.message}`;
        }
      }
    },
    popDialog: async function(order) {
      this.dialogStatus = "";
      this.dialog = !this.dialog;
      this.selectedOrder = order;
      await this.selectOrder(order.id);
    },
  },
};
</script>
