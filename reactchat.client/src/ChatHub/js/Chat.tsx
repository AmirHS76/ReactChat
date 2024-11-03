import React, { useState, useEffect, useRef } from "react";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import Cookies from 'js-cookie';
import '../css/Chat.css';

const Chat: React.FC = () => {
    const [username, setUsername] = useState("");
    const [message, setMessage] = useState("");
    const [messages, setMessages] = useState<{ user: string, text: string }[]>([]);
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const messageInputRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        const fetchUsername = async () => {
            try {
                const token = Cookies.get('token');
                const response = await fetch("https://localhost:7240/getUserData", {
                    headers: { Authorization: `Bearer ${token}` }
                });
                if (response.ok) {
                    const data = await response.json();
                    setUsername(data.username);
                } else {
                    console.error("Failed to fetch username");
                }
            } catch (error) {
                console.error("Error fetching username:", error);
            }
        };

        fetchUsername();
    }, []);

    useEffect(() => {
        const setupConnection = async () => {
            const newConnection = new HubConnectionBuilder()
                .withUrl("https://localhost:7240/chatHub")
                .withAutomaticReconnect()
                .build();

            newConnection.on("ReceiveMessage", (user, receivedMessage) => {
                setMessages((prevMessages) => [...prevMessages, { user, text: receivedMessage }]);
            });

            try {
                await newConnection.start();
                console.log("Connected to SignalR hub");
                setConnection(newConnection);
            } catch (error) {
                console.error("Error establishing connection:", error);
            }
        };

        if (!connection) {
            setupConnection();
        }

        return () => {
            if (connection) {
                connection.off("ReceiveMessage");
                connection.stop();
            }
        };
    }, [connection]);

    const sendMessage = async () => {
        if (connection && message) {
            await connection.send("SendMessage", username, message);
            setMessage(""); 
            if (messageInputRef.current) {
                messageInputRef.current.focus();
            }
        }
    };

    return (
        <div className="chat-container">
            <div className="welcome-message">Welcome, {username}</div>
            <div className="messages-container">
                {messages.map((msg, idx) => (
                    <p key={idx} className="message" style={{ color: msg.user === username ? '#007bff' : '#ff6347' }}>
                        <strong>{msg.user}:</strong> {msg.text}
                    </p>
                ))}
            </div>
            <div className="input-container">
                <input
                    ref={messageInputRef}
                    type="text"
                    className="message-input"
                    placeholder="Type a message..."
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                />
                <button className="send-button" onClick={sendMessage}>Send</button>
            </div>
        </div>
    );
};

export default Chat;
