import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from "@microsoft/signalr";
import { useState, useEffect } from "react";
import Cookies from "js-cookie";

let singletonConnection: HubConnection | null = null;

export const disconnectHub = async () => {
  if (singletonConnection) {
    await singletonConnection.stop();
    singletonConnection = null;
  }
};

export const useChatHub = () => {
  const [connection, setConnection] = useState<HubConnection | null>(
    singletonConnection
  );
  const serverURL = import.meta.env.VITE_API_BASE_URL;

  const connect = async () => {
    if (
      !singletonConnection ||
      singletonConnection.state !== HubConnectionState.Connected
    ) {
      const token = Cookies.get("token");
      const newConnection = new HubConnectionBuilder()
        .withUrl(`${serverURL}chatHub`, {
          accessTokenFactory: () => token || "",
        })
        .withAutomaticReconnect()
        .build();

      try {
        await newConnection.start();
        singletonConnection = newConnection;
        setConnection(newConnection);
      } catch (error) {
        console.error("Connection failed:", error);
      }
    } else {
      setConnection(singletonConnection);
    }
  };

  useEffect(() => {
    connect();
  }, [serverURL]);

  return connection;
};
