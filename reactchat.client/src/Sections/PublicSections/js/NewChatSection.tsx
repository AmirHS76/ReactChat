import { useNavigate } from "react-router-dom";
import "../css/NewChatSection.css";
import AccessesService from "../../../services/AccessesService";

const NewChatSection = () => {
  const navigate = useNavigate();

  const handleStartChat = () => {
    navigate("/users");
  };
  const handleJoinGroupChat = () => {
    navigate("/group-chat");
  };

  return (
    <div className="new-chat-section">
      <label className="chat-label">
        Click this button to start a new chat:
      </label>
      <button className="start-chat-button" onClick={handleStartChat}>
        Start a new chat
      </button>
      {AccessesService.checkAccesses(["CanJoinGroup"]) && (
        <button className="start-chat-button" onClick={handleJoinGroupChat}>
          Join a group chat
        </button>
      )}
    </div>
  );
};

export default NewChatSection;
