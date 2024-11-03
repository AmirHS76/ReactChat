import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";
import { useState, useEffect } from "react";

export const useChatHub = () => {
    const [connection, setConnection] = useState<HubConnection | null>(null);

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl("https://localhost:7240/chatHub")
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);

        return () => {
            newConnection.stop();
        };
    }, []);

    return connection;
};
