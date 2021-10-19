<template>
  <v-container>
    <v-row justify="center">
      <v-col class="display-1 text-center">Cart Contents</v-col>
    </v-row>
    <v-row>
      <v-col justify="center" align="center">
        <v-img
          :src="require('../assets/cart.png')"
          style="height:12vh;width:12vh;"
          aspect-ratio="1"
        />
      </v-col>
    </v-row>
    <v-row style="margin:2vw;">{{ status }}</v-row>
    <div v-if="this.status !== 'cart cleared'">
      <v-simple-table
        style="max-height: 32vh;margin-bottom:5vh;"
        class="overflow-y-auto"
      >
        <tbody>
          <tr>
            <td class="text-left">#</td>
            <td class="text-left">Name</td>
            <td class="text-left">Qty</td>
            <td class="text-left">Price</td>
            <td class="text-left">Ext</td>
          </tr>
          <tr v-for="cartItem in cart" :key="cartItem.id">
            <td>{{ cartItem.item.id }}</td>
            <td>{{ cartItem.item.productName }}</td>
            <td>{{ cartItem.qty }}</td>
            <td>{{ price | currency }}</td>
            <td>{{ ext | currency }}</td>
          </tr>
        </tbody>
      </v-simple-table>

      <v-simple-table
        class="text-left"
        style="margin-left:50vw; border-top:2vw;justify:right;"
      >
        <thead>
          <tr>
            <th>Sub Totals:</th>
            <th>{{ total | currency }}</th>
          </tr>
          <tr>
            <th>tax</th>
            <th>{{ tax | currency }}</th>
          </tr>
        </thead>
      </v-simple-table>
      <v-simple-table
        class="text-left"
        style="margin-left:50vw; border-top:2vw;justify:right;"
      >
        <thead>
          <tr>
            <th>Cart Total</th>
            <th>{{ totalAfterTax | currency }}</th>
          </tr>
        </thead>
      </v-simple-table>
      <v-col
        class="text-left"
        style="margin-left:50vw; border-top:2vw;justify:right;"
      >
        <tbody>
          <tr>
            <td>
              <v-btn medium outlined color="primary" @click="clearCart"
                >Clear Cart</v-btn
              >
            </td>
          </tr>
          <tr></tr>
        </tbody>
        <tbody>
          <td>
            <v-btn medium color="primary" class="text--white" @click="addOrder"
              >Save</v-btn
            >
          </td>
        </tbody>
      </v-col>
    </div>
  </v-container>
</template>
>
<script>
import fetcher from "../mixins/fetcher";
export default {
  name: "CartDetails",
  data() {
    return {
      id: 0,
      name: 0,
      price: 0,
      ext: 0,
      status: "",
      total: 0,
      tax: 0,
      totalAfterTax: 0,
    };
  },
  beforeMount: function() {
    if (sessionStorage.getItem("cart")) {
      this.cart = JSON.parse(sessionStorage.getItem("cart"));
      this.cart.map((cartItem) => {
        this.id = cartItem.item.id;
        this.name = cartItem.item.productName;
        this.price = cartItem.item.msrp;
        this.ext = cartItem.item.msrp * cartItem.qty;
        this.total += cartItem.item.msrp * cartItem.qty;
        this.totalAfterTax += cartItem.item.msrp * cartItem.qty * 1.13;
        this.tax += cartItem.item.msrp * cartItem.qty * 0.13;
      });
    } else {
      this.cart = [];
    }
  },
  mixins: [fetcher],
  methods: {
    clearCart: function() {
      sessionStorage.removeItem("cart");
      this.cart = [];
      this.status = "cart cleared";
    },
    addOrder: async function() {
      let customer = JSON.parse(sessionStorage.getItem("customer"));
      let order = JSON.parse(sessionStorage.getItem("cart"));
      try {
        this.status = "sending cart info to server";
        let orderHelper = { email: customer.email, selections: order };
        let payload = await this.$_postdata("order", orderHelper); // in mixin
        if (payload.indexOf("not") > 0) {
          this.status = payload;
        } else {
          this.clearCart();
          this.status = payload;
        }
      } catch (err) {
        console.log(err);
        this.status = `Error add Order: ${err}`;
      }
    },
  },
};
</script>
