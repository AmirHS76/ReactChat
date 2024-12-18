import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";
import { useState, useEffect } from "react";

export const useChatHub = () => {
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const serverURL = import.meta.env.VITE_API_BASE_URL;
    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(`${serverURL}chatHub`)
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);

        return () => {
            newConnection.stop();
        };
    }, []);

    return connection;
};
