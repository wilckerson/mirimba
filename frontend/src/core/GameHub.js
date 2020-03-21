import { HubConnectionBuilder } from "@aspnet/signalr";

const GameHub = function(hubUrl, onReceiveMessageFn, onUpdateFn) {

    var connection = undefined;
    var currentUserName = undefined;
    var currentRoomId = undefined;

    async function openConnection() {

        connection = new HubConnectionBuilder()
            .withUrl(hubUrl)
            .build();

        connection.on("ReceiveMessage", onReceiveMessageFn);
        connection.on("Update", onUpdateFn);

        await connection.start();
    }

    async function joinRoom(roomId, userName) {

        currentRoomId = roomId;
        currentUserName = userName;

        await openConnection();

        connection.invoke("JoinRoom", roomId, userName);
    }

    function sendMessage(text) {
        connection.invoke("Send", currentRoomId, currentUserName, text);
    }

    return {
        joinRoom: joinRoom,
        sendMessage: sendMessage
    };
}

export default GameHub;