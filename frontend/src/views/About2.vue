<template>
  <div class="game-room">
    <div class="board-container">
      <h4 class="pt-3">Uno Mirimba - Sala de Jogo</h4>

      <div class="player player-1">
        <div class="player-handcard-container">
          <img src="img/ico_handcards.png" class="player-handcard-img" />
          <div class="player-handcard-number">5</div>
        </div>
        <div class="player-name">Wilckerson</div>
      </div>

      <div class="player player-2">
        <div class="player-handcard-container">
          <img src="img/ico_handcards.png" class="player-handcard-img" />
          <div class="player-handcard-number">13</div>
        </div>
        <div class="player-name">Dani</div>
      </div>

      <div class="player player-3">
        <div class="player-handcard-container">
          <img src="img/ico_handcards.png" class="player-handcard-img" />
          <div class="player-handcard-number">4</div>
        </div>
        <div class="player-name">Amanda</div>
      </div>

      <div class="player player-4">
        <div class="player-handcard-container">
          <img src="img/ico_handcards.png" class="player-handcard-img" />
          <div class="player-handcard-number">1</div>
        </div>
        <div class="player-name">Juju</div>
      </div>

      <div class="player player-5">
        <div class="player-handcard-container">
          <img src="img/ico_handcards.png" class="player-handcard-img" />
          <div class="player-handcard-number">7</div>
        </div>
        <div class="player-name">Charles</div>
      </div>

      <div class="player player-6">
        <div class="player-handcard-container">
          <img src="img/ico_handcards.png" class="player-handcard-img" />
          <div class="player-handcard-number">1</div>
        </div>
        <div class="player-name">Bose</div>
      </div>

      <div class="player player-7">
        <div class="player-handcard-container">
          <img src="img/ico_handcards.png" class="player-handcard-img" />
          <div class="player-handcard-number">1</div>
        </div>
        <div class="player-name">Shirlene</div>
      </div>

      <!-- <div class="boardcard-stack"> -->
      <div class="boardcard-7">
        <div class="handcard boardcard">
          <div class="handcard-content red7"></div>
        </div>
      </div>

       <div class="boardcard-6">
        <div class="handcard boardcard">
          <div class="handcard-content blue1"></div>
        </div>
      </div>

       <div class="boardcard-5">
        <div class="handcard boardcard">
          <div class="handcard-content green3"></div>
        </div>
      </div>

       <div class="boardcard-4">
        <div class="handcard boardcard">
          <div class="handcard-content blue0"></div>
        </div>
      </div>

       <div class="boardcard-3">
        <div class="handcard boardcard">
          <div class="handcard-content red6"></div>
        </div>
      </div>

       <div class="boardcard-2">
        <div class="handcard boardcard">
          <div class="handcard-content blue4"></div>
        </div>
      </div>

       <div class="boardcard-1">
        <div class="handcard boardcard">
          <div class="handcard-content yellow1"></div>
        </div>
      </div>

      <!-- </div> -->
    </div>
    <div class="deck-container">
      <img src="img/uno_deck_back.png" height="100%" />
    </div>
    <!-- <h5>Wilckerson 2</h5> -->
    <div class="handcards-container m-2 mt-4">
      <div class="handcard">
        <div class="handcard-content plus4"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content red2"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content green6"></div>
      </div>

      <div class="handcard">
        <div class="handcard-content green3"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content plus4"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content red2"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content green6"></div>
      </div>

      <div class="handcard">
        <div class="handcard-content green3"></div>
      </div>

      <!-- 
       <div class="handcard">
        <div class="handcard-content"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content red2"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content red1"></div>
      </div>
       <div class="handcard">
        <div class="handcard-content"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content red2"></div>
      </div>-->
    </div>
  </div>
</template>

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

.board-container {
  /* height: 61.8033%; */
  height: 70%;
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
