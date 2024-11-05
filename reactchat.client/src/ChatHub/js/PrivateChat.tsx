import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import Cookies from 'js-cookie';
import '../css/PrivateChat.css';

interface Message {
    sender: string;
    content: string;
    type: string;
}

const PrivateChat: React.FC = () => {
    const { username } = useParams<{ username: string }>();
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
                if (type === "recipient" || sender !== "You") {
                    setMessages((prevMessages) => [...prevMessages, { sender, content, type }]);
                }
            });

            try {
                await newConnection.start();
                console.log('Connected to SignalR Hub');
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
    }, [username, connection]);


    const sendMessage = async () => {
        if (connection && input.trim()) {
            try {
                await connection.invoke('SendPrivateMessage', username, input);

                setMessages((prevMessages) => [
                    ...prevMessages,
                    { sender: 'You', content: input, type: 'sender' },
                ]);
                setInput('');
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
