<template>
  <div>
    <h1>Sala de jogo</h1>Nome:
    <input type="text" v-model="userName" />
    <br />
    <input type="text" v-model="message" />
    <button @click="onClickSend()">Enviar</button>

    <ul>
        <li v-for="(item, index) in messages" :key="'msg'+index"><b>{{item.user}}:</b> {{item.message}}</li>
    </ul>
  </div>
</template>

<script>
import { HubConnectionBuilder } from "@aspnet/signalr";
import axios from "axios";

import config from "../config";

var connection = undefined;

export default {
  data() {
    return {
      userName: undefined,
      message: undefined,
      messages: []
    };
  },
  async created() {
    

    var response = await axios.get(config.API_URL + "/values");
    console.log("resp",response);

    connection = new HubConnectionBuilder()
      .withUrl(config.API_URL + "/gameHub")
      .build();

    connection.on("ReceiveMessage", data => {
      //console.log("received",data);
      this.messages.push(data);
    });

    connection.start(); //.then(() => connection.invoke("send", "Hello"));
    
  },
  methods: {
    onClickSend() {
      if (!connection) {
        return;
      }

      connection.invoke("Send", this.userName, this.message);
      this.message = undefined;
    }
  }
};
</script>