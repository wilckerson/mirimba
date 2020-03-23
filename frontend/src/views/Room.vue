<template>
  <div class="game-room">
   
    <!-- <small>{{roomId}}</small> -->

    <div v-if="!isConnected">
       <h4 class="pt-3">Uno Mirimba - Sala de Jogo</h4>

      <div class="mt-5">Qual o seu nome?</div>
      <input
        type="text"
        v-model="userName"
        @keyup.enter="onClickJoinRoom()"
        class="form-control w-auto d-inline-block"
      />
      <br />
      <button
        :disabled="!this.canJoinRoom()"
        @click.prevent="onClickJoinRoom()"
        class="btn btn-success mt-3"
      >Entrar na sala</button>
      <div v-if="loadingJoinRoom" class="mt-2">Entrando...</div>
    </div>

    <div v-if="isConnected && !state.isGameStarted">
       <h4 class="pt-3">Uno Mirimba - Sala de Jogo</h4>

      Jogadores:
      <br />
      <br />
      <div v-for="(item, index) in state.publicPlayersState" :key="'publicPlayerState'+index">
        <b>
          <span :class="{'current-user': item.userName == userName}">{{item.userName}}</span>
          <span v-if="!item.isOnline" style="color:red">[Offline]</span>
        </b>
        <br />
        <br />
      </div>

       <div>
        <button
          :disabled="!canStartNewGame()"
          @click.prevent="onClickStartNewGame()"
          class="btn btn-success mt-3"
        >Iniciar novo jogo</button>
        <div v-if="!canStartNewGame()" class="mt-2">É necessário no mínio dois jogadores.</div>
        <br />
        <br />
      </div>
    </div>

    

    <div v-if="isConnected && state.isGameStarted" style="height:100%;">
      <div class="row">
        <div class="col">
            <h4 class="pt-3">Uno Mirimba </h4>
        </div>
        <div class="col">
           <button
          :disabled="!canStartNewGame()"
          @click.prevent="onClickStartNewGame()"
          class="btn btn-success mt-3"
        >Iniciar novo jogo</button>
        <div v-if="!canStartNewGame()" class="mt-2">É necessário no mínio dois jogadores.</div>
        </div>
      </div>
      <div class="board-container">
        <div
          v-for="(item, index) in state.publicPlayersState"
          :key="'player'+index"
          :class="'player player-'+(index+1)"
        >
          <div class="player-handcard-container">
            <img src="/img/ico_handcards.png" class="player-handcard-img" />
            <div class="player-handcard-number">{{item.handCardsCount}}</div>
          </div>
          <div class="player-name">
            <span :class="{'current-user': item.userName == userName}">{{item.userName}}</span>
            <span v-if="!item.isOnline" class="text-danger">[Offline]</span>
          </div>
        </div>

        <div
          v-for="(item, index) in getBoardcardStack()"
          :key="'boardcard'+index"
          :class="'boardcard-'+(getBoardcardStack().length-index)"
        >
          <div
            class="handcard boardcard"
            @click="onClickBoardCard(item,getBoardcardStack().length-index)"
          >
            <div :class="'handcard-content '+item"></div>
          </div>
        </div>
      </div>
      <div class="deck-container">
        <img src="/img/uno_deck_back.png" height="100%" @click.prevent="onClickGetFromDeck()" />
        <div>{{state.deckCount}}</div>
      </div>

      <div class="handcards-container m-2 mt-4">
        <div
          class="handcard"
          v-for="(item, index) in state.handCards"
          :key="'handCard'+index"
          @click.prevent="onClickFromHandToBoard(item)"
        >
          <div :class="'handcard-content '+item"></div>
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
    getBoardcardStack() {
      return (this.state.boardCards || []).slice(0, 7).reverse();
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
      if (this.state.deckCount == 0) {
        return;
      }
      gameHub.getFromDeck();
    },
    onClickGetFromBoard() {
      gameHub.getFromBoard();
    },
    onClickFromHandToBoard(card) {
      gameHub.fromHandToBoard(card);
    },
    onClickClearHistory() {
      // if (this.state.boardCards && this.state.boardCards.length > 1) {
      //   return;
      // }

      gameHub.clearBoardPastHistory();
    },
    onClickBoardCard(item, index) {
      //console.log("c", item, index);

      if (index > 1) {
        this.onClickClearHistory();
      } else {
        this.onClickGetFromBoard();
      }
    }
  }
};
</script>


<style lang="less" scoped>
.game-room {
  background: rgb(53, 109, 51);
  background: radial-gradient(
    circle,
    rgba(53, 109, 51, 1) 0%,
    rgba(39, 71, 38, 1) 100%
  );
  height: 100%;
  color: white;
}

.deck-container {
  /* background: red; */
  position: absolute;
  width: 17%;
  height: 17%;
  right: 17%;
  top: 50%;
}

.deck-container img {
  cursor: pointer;
}

.board-container {
  /* height: 61.8033%; */
  height: 65%;
  /* border: 1px solid red; */
}

.handcards-container {
  /* height: 30%; */
  white-space: nowrap;
  overflow-x: auto;
  /* border: 1px solid yellow; */
}

.handcard {
  max-width: 100px;
  width: 20%;
  display: inline-block;
  margin-right: -25px;
  /* margin-right: -5%; */
  /* border: 1px solid red;   */
  cursor: pointer;
}

.handcard-content {
  padding-top: 150%;
  background: url(/img/UNO_cards_deck.png) 0 0;
  background-size: 1392%;
}

.boardcard {
  max-width: 80px;
  width: 16%;
  display: block;
  margin-right: auto;
  margin-left: auto;
}

.boardcard-stack {
  /* margin-top: 36vh; */
  text-align: center;
  position: absolute;
  top: 40%;
  width: 100%;
  /* background-color: yellow; */
}

.boardcard-1 {
  position: absolute;
  top: 38%;
  width: 100%;
  transform: rotateZ(5deg);
}

.boardcard-2 {
  position: absolute;
  top: 37%;
  width: 100%;
  transform: rotateZ(-37deg);
  margin-left: -6px;
}
.boardcard-3 {
  position: absolute;
  top: 35%;
  width: 100%;
  transform: rotateZ(41deg);
}
.boardcard-4 {
  position: absolute;
  top: 33.5%;
  width: 100%;
  transform: rotateZ(-13deg);
}
.boardcard-5 {
  position: absolute;
  top: 30%;
  width: 100%;
  transform: rotateZ(43deg);
}
.boardcard-6 {
  position: absolute;
  top: 29%;
  width: 100%;
  transform: rotateZ(-8deg);
}
.boardcard-7 {
  position: absolute;
  top: 27%;
  width: 100%;
  transform: rotateZ(20deg);
}

.player {
  width: 30%;

  /* background-color: red; */
}
.player-handcard-container {
  position: relative;
}
.player-handcard-img {
  width: 40px;
  margin-left: 12px;
}
.player-handcard-number {
  position: absolute;
  top: 10px;
  width: 100%;
  text-align: center;
  font-weight: bold;
}
.player-1 {
  margin: 0 auto;
  margin-top: 30px;
}

.player-2 {
  position: absolute;
  top: 16%;
  left: 5%;
}

.player-3 {
  position: absolute;
  top: 36%;
  left: 0%;
}

.player-4 {
  position: absolute;
  top: 16%;
  right: 5%;
}

.player-5 {
  position: absolute;
  top: 36%;
  right: 0;
}

.player-6 {
  position: absolute;
  top: 55%;
  left: 5%;
}

.player-7 {
  position: absolute;
  top: 60%;
  width: 100%;
}

@import "../styles/uno-cards.less";
</style>
