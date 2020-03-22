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
            <span :class="{'current-user': item.userName == userName}">{{item.userName}}</span>
            <span v-if="!item.isOnline" style="color:red">[Offline]</span>
          </b>
          <div v-if="state.isGameStarted">Cartas na mão: {{item.handCardsCount}}</div>
          <br />
          <br />
        </div>
      </div>

      <!-- <div v-if="!state.isGameStarted"> -->
      <div>
        <button
          :disabled="!canStartNewGame()"
          @click.prevent="onClickStartNewGame()"
        >Iniciar novo jogo</button>
        <div v-if="!canStartNewGame()">É necessário no mínio dois jogadores.</div>
        <br />
        <br />
      </div>
      <div v-if="state.isGameStarted">
        <div>
          Baralho: {{state.deckCount}}
          <button
            @click.prevent="onClickGetFromDeck()"
            :disabled="state.deckCount == 0"
          >Puxar carta pra mão</button>
        </div>
        <br />
        <br />
        <div>
          Mesa:
          <button
            class="card"
            @click.prevent="onClickGetFromBoard()"
            v-if="state.boardCards && state.boardCards.length > 0"
          >{{state.boardCards[0]}}</button>
        </div>
        <br />
        <br />
        <div>
          Histórico: {{state.boardCards}}
          <button
            @click.prevent="onClickClearHistory()"
            :disabled="!(state.boardCards && state.boardCards.length > 1)"
          >Retornar o histórico antigo para o baralho</button>
        </div>
        <br />
        <br />
        <div>
          Cartas da mão:
          <button
            v-for="(item, index) in state.handCards"
            :key="'handCard'+index"
            class="card"
            @click.prevent="onClickFromHandToBoard(item)"
          >{{item}}</button>
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
      //console.log("Update", data);
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
    onClickGetFromDeck() {
      gameHub.getFromDeck();
    },
    onClickGetFromBoard() {
      gameHub.getFromBoard();
    },
    onClickFromHandToBoard(card) {
      gameHub.fromHandToBoard(card);
    },
    onClickClearHistory() {
      gameHub.clearBoardPastHistory();
    }
  }
};
</script>

<style scoped>
.card {
  padding: 20px;
}

.current-user{
    color:blue;
}
</style>