// define a mixin object
export default {
  data() {
    return {
      SERVERURL: `https://localhost:44361/api/`,
      //SERVERURL: `/api/`,
    };
  },
  methods: {
    // don't use arrow function here
    $_getdata: async function(apicall) {
      let payload = {};
      let headers = this.buildHeaders();
      try {
        let response = await fetch(`${this.SERVERURL}${apicall}`, {
          method: "GET",
          headers: headers,
        });
        payload = await response.json();
        console.log(payload);
      } catch (err) {
        console.log(err);
        payload = err;
      }
      return payload;
    },
    $_postdata: async function(apicall, data) {
      let payload = JSON.stringify(data);
      let headers = {};
      if (apicall === "register" || apicall === "login") {
        headers = { "Content-Type": "application/json; charset=utf-8" };
      } else {
        headers = this.buildHeaders();
      }

      try {
        let response = await fetch(`${this.SERVERURL}${apicall}`, {
          method: "POST",
          headers: headers, //{ "Content-Type": "application/json; charset=utf-8" },
          body: payload,
        });
        payload = await response.json();
      } catch (error) {
        payload = error;
      }
      return payload;
    },
    buildHeaders: function() {
      const myHeaders = new Headers();
      const customer = JSON.parse(sessionStorage.getItem("customer"));
      myHeaders.append("Content-Type", "application/json");
      myHeaders.append("Authorization", "Bearer " + customer.token);
      return myHeaders;
    },
    $_buildRouteWithParams: function(...data) {
      let routeParams = "";
      data.map((param) => (routeParams += `${param}/`)); // assume api route is first element
      return routeParams;
    },
  },
};
