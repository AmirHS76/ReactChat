import { useState, useCallback } from 'react';
import { HubConnectionBuilder, HubConnection, HubConnectionState } from '@microsoft/signalr';
import Cookies from 'js-cookie';

interface Message {
    sender: string;
    content: string;
    type: 'sender' | 'recipient';
}

export const useChatConnection = (): [HubConnection | null, (username: string, setMessages: React.Dispatch<React.SetStateAction<Message[]>>, currentUser: string | undefined) => Promise<void>] => {
    const [connection, setConnection] = useState<HubConnection | null>(null);

    const connectToHub = useCallback(async (username: string, setMessages: React.Dispatch<React.SetStateAction<Message[]>>, currentUser: string | undefined) => {
        if (connection) return;

        const token = Cookies.get('token');
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:7240/chatHub', {
                accessTokenFactory: () => token || '',
            })
            .withAutomaticReconnect()
            .build();

        newConnection.on('ReceiveMessage', (sender: string, content: string, type: string) => {
            const isSender = sender === currentUser;
            setMessages((prevMessages) => [
                ...prevMessages,
                {
                    sender: isSender ? 'You' : sender,
                    content,
                    type: isSender ? 'sender' : 'recipient',
                },
            ]);
        });

        try {
            await newConnection.start();
            setConnection(newConnection);
            await newConnection.invoke('JoinPrivateChat', username);
        } catch (error) {
            console.error('Connection failed:', error);
        }
    }, [connection]);

    return [connection, connectToHub];
};
