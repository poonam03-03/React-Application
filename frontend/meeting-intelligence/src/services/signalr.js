import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7148/notificationHub")
  .withAutomaticReconnect()
  .build();

export default connection;