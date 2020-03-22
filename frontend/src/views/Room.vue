<template>
  <div>
    <h1>Uno Mirimba - Sala de Jogo</h1>
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
      <div>
        Jogadores:
        <br />
        <br />
        <div v-for="(item, index) in state.publicPlayersState" :key="'publicPlayerState'+index">
          <b>
            <span>{{item.userName}}</span>
            <span v-if="!item.isOnline" style="color:red">[Offline]</span>
          </b>
          <div>Cartas na mão: {{item.handCardsCount}}</div>
          <br />
          <br />
        </div>
      </div>

      <div v-if="!state.isGameStarted">
        <button
          :disabled="!canStartNewGame()"
          @click.prevent="onClickStartNewGame()"
        >Iniciar novo jogo</button>
        <div v-if="!canStartNewGame()">É necessário no mínio dois jogadores.</div>
      </div>
      <div v-else>
        <div>
            Baralho: {{state.deckCount}}
            <button @click="onClickGetFromDeck()" :disabled="state.deckCount == 0">Puxar carta pra mão</button>
            </div>
        <br/><br/>
        <div>Mesa: <button class="card">{{state.boardCards[0]}}</button></div><br/><br/>
        <div>Histórico: {{state.boardCards}}</div><br/><br/>
        <div>
          Cartas da mão:
          <button v-for="(item, index) in state.handCards" :key="'handCard'+index" class="card">{{item}}</button>
        </div>
      </div>
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
      
    onGameHubMessage(data) {
      this.messages.push(data);
    },
    onGameHubUpdate(data) {
      console.log("Update", data);
      this.state = data;
    },
    canJoinRoom() {
      return this.userName && !this.loadingJoinRoom;
    },
    canStartNewGame() {
      return this.state.publicPlayersState.length > 1;
    },

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
    onClickStartNewGame() {
      gameHub.startNewGame();
    },
    onClickGetFromDeck(){
        gameHub.getFromDeckToPlayerHandCards();
    },
    
  }
};
</script>

<style scoped>
.card{
    padding:20px;
}
</style>