import Vue from "vue";
import Router from "vue-router";
import Home from "./components/Home.vue";
import CategoryList from "./components/CategoryList.vue";
import TrayDetails from "./components/TrayDetails.vue";
import Register from "./components/Register.vue";
import Login from "./components/Login.vue";
import Logout from "./components/Logout.vue";
import TrayList from "./components/TrayList.vue";
import MapEx3 from "./components/MapEx3.vue";
import DataUtil from "./components/DataUtil.vue";
import StoreLocator from "./components/StoreLocator.vue";
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
      path: "/categories",
      name: "categories",
      component: CategoryList,
    },
    {
      path: "/tray",
      name: "tray",
      component: TrayDetails,
    },
    { path: "/register", name: "register", component: Register },
    { path: "/login", name: "login", component: Login, props: true },
    { path: "/logout", name: "logout", component: Logout, props: true },
    { path: "/traylist", name: "traylist", component: TrayList },
    {
      path: "/map",
      name: "map",
      component: MapEx3,
    },
    {
      path: "/datautil",
      name: "datautil",
      component: DataUtil,
    },
    {
      path: "/storelocator",
      name: "storelocator",
      component: StoreLocator,
    },
  ],
});
router.beforeEach((to, from, next) => {
  const publicPages = ["/login", "/register", "/logout,", "/map"];
  const authRequired = !publicPages.includes(to.path);
  if (authRequired && !sessionStorage.getItem("user")) {
    next({
      name: "login",
      params: { nextUrl: to.name },
    });
  } else {
    next();
  }
});
