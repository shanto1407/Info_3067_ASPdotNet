import Vue from "vue";
import Router from "vue-router";
import Home from "./components/Home.vue";
import BrandList from "./components/BrandList.vue";
import CartDetails from "./components/CartDetails.vue";
import Register from "./components/Register.vue";
import Login from "./components/Login.vue";
import Logout from "./components/Logout.vue";
import OrderList from "./components/OrderList.vue";
import Map from "./components/Map.vue";
Vue.use(Router);
export const router = new Router({
  mode: "history",
  base: process.env.BASE_URL,
  routes: [
    {
      path: "/",
      name: "home",
      component: Home,
    },
    {
      path: "/brands",
      name: "brands",
      component: BrandList,
    },
    {
      path: "/cart",
      name: "cart",
      component: CartDetails,
    },
    { path: "/register", name: "register", component: Register },
    { path: "/login", name: "login", component: Login, props: true },
    { path: "/logout", name: "logout", component: Logout, props: true },
    { path: "/orderlist", name: "orderlist", component: OrderList },
    { path: "/map", name: "map", component: Map },
  ],
});
router.beforeEach((to, from, next) => {
  const publicPages = ["/login", "/register", "/logout"];
  const authRequired = !publicPages.includes(to.path);
  if (authRequired && !sessionStorage.getItem("customer")) {
    next({
      name: "login",
      params: { nextUrl: to.name },
    });
  } else {
    next();
  }
});
