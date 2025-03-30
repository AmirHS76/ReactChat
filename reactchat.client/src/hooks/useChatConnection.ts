import { useState, useCallback, useEffect } from "react";
import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";
import Cookies from "js-cookie";

interface Message {
  sender: string;
  content: string;
  type: "sender" | "recipient";
}

let singletonConnection: HubConnection | null = null;

export const disconnectChatHub = async () => {
  if (singletonConnection) {
    await singletonConnection.stop();
    singletonConnection = null;
  }
};

export const useChatConnection = (): [
  HubConnection | null,
  (
    username: string,
    setMessages: React.Dispatch<React.SetStateAction<Message[]>>,
    currentUser: string | undefined
  ) => Promise<void>,
  () => Promise<void>
] => {
  const [connection, setConnection] = useState<HubConnection | null>(
    singletonConnection
  );

  const connectToHub = useCallback(
    async (
      username: string,
      setMessages: React.Dispatch<React.SetStateAction<Message[]>>,
      currentUser: string | undefined
    ) => {
      if (singletonConnection) {
        // Reconfigure the existing handlers if necessary
        singletonConnection.off("ReceiveMessage");
        singletonConnection.on(
          "ReceiveMessage",
          (sender: string, content: string) => {
            const isSender = sender === currentUser;
            setMessages((prevMessages) => [
              {
                sender: isSender ? "You" : sender,
                content,
                type: isSender ? "sender" : "recipient",
              },
              ...prevMessages,
            ]);
          }
        );
        return;
      }

      const token = Cookies.get("token");
      const newConnection = new HubConnectionBuilder()
        .withUrl(import.meta.env.VITE_API_BASE_URL + "chatHub", {
          accessTokenFactory: () => token || "",
        })
        .withAutomaticReconnect()
        .build();

      newConnection.on("ReceiveMessage", (sender: string, content: string) => {
        const isSender = sender === currentUser;
        setMessages((prevMessages) => [
          {
            sender: isSender ? "You" : sender,
            content,
            type: isSender ? "sender" : "recipient",
          },
          ...prevMessages,
        ]);
      });

      try {
        await newConnection.start();
        singletonConnection = newConnection;
        setConnection(newConnection);
        await newConnection.invoke("JoinPrivateChat", username);
      } catch (error) {
        console.error("Connection failed:", error);
      }
    },
    []
  );

  const disconnectFromHub = useCallback(async () => {
    if (connection) {
      await connection.stop();
      singletonConnection = null;
      setConnection(null);
    }
  }, [connection]);

  // Optional: cleanup on unmount
  useEffect(() => {
    return () => {
      if (connection) {
        connection.stop();
        singletonConnection = null;
      }
    };
  }, [connection]);

  return [connection, connectToHub, disconnectFromHub];
};
