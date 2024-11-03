import { useNavigate } from "react-router-dom";
import '../css/NewChatSection.css';

const NewChatSection = () => {
    const navigate = useNavigate();

    const handleStartChat = () => {
        navigate("/chat");
    };
    return (
        <div className="new-chat-section">
            <label className="chat-label">Click this button to start a new chat:</label>
            <button className="start-chat-button" onClick={handleStartChat}>
                Start a new chat
            </button>
        </div>
    );
};

export default NewChatSection;
