<template>
  <div class="game-room">
      <h4 class="pt-3">Uno Mirimba - Sala de Jogo</h4>
    <div class="board-container">

    
    </div>
    <div class="deck-container">
      <img src="img/uno_deck_back.png" height="100%" />
    </div>
    <h5>Wilckerson</h5>
    <div class="handcards-container m-2 mt-4">
<!--       
      <div class="handcard">
        <div class="handcard-content red1"></div>
      </div>
       <div class="handcard">
        <div class="handcard-content"></div>
      </div>
      <div class="handcard">
        <div class="handcard-content red2"></div>
      </div> -->
    <div class="handcard">
        <div class="handcard-content red4"></div>
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
      </div> -->



    </div>
  </div>
</template>

<style scoped>
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
  height: 40%;
  /* border: 1px solid red; */
}

.handcards-container {
  /* height: 30%; */
  white-space: nowrap;
    overflow-x: auto;
  /* border: 1px solid yellow; */
}

.handcard {
  max-width: 132px;
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

.red1 {
  background-position-x: 7.7%;
}

.red2 {
  background-position-x: 15.37%;
}

.red3 {
  background-position-x: 23.07%;
}

.red4 {
  background-position-x: 30.75%;
}
</style>
