<template>
  <div>
    <h1>Sala de jogo</h1>
    <!-- <small>{{roomId}}</small> -->

    <div v-if="!isConnected">
      <div>Qual o seu nome?</div>
      <input type="text" v-model="userName" @keyup.enter="onClickJoinRoom()" />
      <br />
      <button :disabled="!this.canJoinRoom()" @click.prevent="onClickJoinRoom()">Entrar na sala</button>
      <div v-if="loadingJoinRoom">Entrando...</div>
    </div>

    <div v-if="isConnected">
      <!-- <input type="text" v-model="message" />
      <button @click="onClickSend()">Enviar</button>

      <ul>
        <li v-for="(item, index) in messages" :key="'msg'+index">
          <b>{{item.user}}:</b>
          {{item.message}}
        </li>
      </ul>-->
      Jogadores:
      <div
        v-for="(item, index) in state.publicPlayersState"
        :key="'publicPlayerState'+index"
      >{{item.userName}} (Cartas na mão: {{item.handCardsCount}} )</div>
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
      messages: [],
      state: {
        publicPlayersState: []
      }
    };
  },
  async created() {
    this.roomId = this.$route.params.roomId;

    //Nome de usuário utilizado anteriormente
    var lastUserName = window.localStorage.getItem("userName");
    if (lastUserName) {
      this.userName = lastUserName;
    }

    gameHub = new GameHub(
      config.API_URL + "/gameHub",
      this.onGameHubMessage,
      this.onGameHubUpdate
    );
  },
  methods: {
    onClickSend() {
      gameHub.sendMessage(this.message);
      this.message = undefined;
    },
    async onClickJoinRoom() {
      if (!this.canJoinRoom()) {
        return;
      }

      this.loadingJoinRoom = true;

      //Salvando userName no localStorage
      window.localStorage.setItem("userName", this.userName);

      await gameHub.joinRoom(this.roomId, this.userName);

      this.loadingJoinRoom = false;
      this.isConnected = true;
    },
    onGameHubMessage(data) {
      this.messages.push(data);
    },
    onGameHubUpdate(data) {
      console.log("Update", data);
      this.state = data;
    },
    canJoinRoom() {
      return this.userName && !this.loadingJoinRoom;
    }
  }
};
</script>