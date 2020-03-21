<template>
  <div class="home">
    <!-- <img alt="Vue logo" src="../assets/logo.png">
    <HelloWorld msg="Welcome to Your Vue.js App"/> -->
    <h1>Uno Mirimba</h1>

    <button @click.prevent="onClickCreateRoom()">Criar sala</button>
  </div>
</template>

<script>
// @ is an alias to /src
//import HelloWorld from '@/components/HelloWorld.vue'
import {v4 as uuidv4} from "uuid";

export default {
  name: 'Home',
  components: {
    //HelloWorld
  },
  methods:{
    onClickCreateRoom(){
      var roomId = uuidv4();
      this.$router.push(`/room/${roomId}`);
    }
  }
}
</script>
