import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { HubConnectionBuilder, HubConnection, HubConnectionState } from '@microsoft/signalr';
import Cookies from 'js-cookie';
import '../css/PrivateChat.css';

interface Message {
    sender: string;
    content: string;
    type: string;
}

const PrivateChat: React.FC = () => {
    const { username } = useParams<{ username: string }>();
    const currentUser = Cookies.get('username');
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const [messages, setMessages] = useState<Message[]>([]);
    const [input, setInput] = useState('');

    useEffect(() => {
        const connectToHub = async () => {
            if (connection) return;
            const token = Cookies.get('token');

            const newConnection = new HubConnectionBuilder()
                .withUrl(`https://localhost:7240/chatHub`, {
                    accessTokenFactory: () => token || '',
                })
                .withAutomaticReconnect()
                .build();

            newConnection.on('ReceiveMessage', (sender: string, content: string, type: string) => {
                const isSender = sender === currentUser;
                const displaySender = isSender ? 'You' : sender;
                type = isSender ? 'sender' : 'recipient';
                setMessages((prevMessages) => [
                    ...prevMessages,
                    { sender: displaySender, content, type }
                ]);
            });

            try {
                try {
                    await newConnection.start();
                    console.log('Connected to SignalR Hub');
                }
                catch (err) {
                    console.error("Erorr :", err);
                }
                setConnection(newConnection);

                await newConnection.invoke('JoinPrivateChat', username);
            } catch (error) {
                console.error('Connection failed:', error);
            }
        };

        connectToHub();

        return () => {
            connection?.stop();
        };
    }, [username, connection, currentUser]);

    const sendMessage = async () => {
        if (connection && input.trim()) {
            try {
                if (connection.state === HubConnectionState.Connected) {
                    await connection.invoke('SendPrivateMessage', username, input);
                } else {
                    console.error("Cannot send message. SignalR connection is not connected.");
                }
            } catch (error) {
                console.error('Sending message failed:', error);
            }
        }
    };

    return (
        <div className="private-chat-container">
            <h2 className="chat-header">Private chat with {username}</h2>
            <div className="chat-messages">
                {messages.map((message, index) => (
                    <div key={index} className={`message-item ${message.type}`}>
                        <strong>{message.sender}:</strong> {message.content}
                    </div>
                ))}
            </div>
            <div className="input-container">
                <input
                    type="text"
                    value={input}
                    onChange={(e) => setInput(e.target.value)}
                    onKeyDown={(e) => e.key === 'Enter' && sendMessage()}
                    placeholder="Type a message..."
                    className="message-input"
                />
                <button onClick={sendMessage} className="send-button">
                    Send
                </button>
            </div>
        </div>
    );
};

export default PrivateChat;
