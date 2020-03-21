<template>
  <div>
    <h1>Sala de jogo</h1>
    <!-- <small>{{roomId}}</small> -->

    <div v-if="!isConnected">
      <div>Qual o seu nome?</div>
      <input type="text" v-model="userName" />
      <br />
      <button
        :disabled="!userName || loadingJoinRoom"
        @click.prevent="onClickJoinRoom()"
      >Entrar na sala</button>
      <div v-if="loadingJoinRoom">Entrando...</div>
    </div>

    <div v-if="isConnected">
      <input type="text" v-model="message" />
      <button @click="onClickSend()">Enviar</button>

      <ul>
        <li v-for="(item, index) in messages" :key="'msg'+index">
          <b>{{item.user}}:</b>
          {{item.message}}
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
import GameHub from "../core/GameHub";
import config from "../config";

var gameHub = undefined;

export default {
  data() {
    return {
      isConnected: false,
      loadingJoinRoom: false,
      roomId: undefined,
      userName: undefined,
      message: undefined,
      messages: []
    };
  },
  async created() {
    this.roomId = this.$route.params.roomId;

    gameHub = new GameHub(config.API_URL + "/gameHub", this.onGameHubMessage);

  },
  methods: {
    onClickSend() {
      gameHub.sendMessage(this.message);
      this.message = undefined;
    },
    async onClickJoinRoom() {
      this.loadingJoinRoom = true;

      await gameHub.joinRoom(this.roomId, this.userName);

      this.loadingJoinRoom = false;
      this.isConnected = true;
    },
    onGameHubMessage(data) {
      this.messages.push(data);
    }
  }
};
</script>