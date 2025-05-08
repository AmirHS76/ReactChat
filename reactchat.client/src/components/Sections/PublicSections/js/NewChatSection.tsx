import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../css/NewChatSection.css";
import AccessesService from "../../../../services/AccessesService";

const NewChatSection = () => {
  const navigate = useNavigate();
  const [canJoinGroup, setCanJoinGroup] = useState<boolean>(false);

  useEffect(() => {
    const fetchAccess = async () => {
      const access = await AccessesService.checkAccesses(["CanJoinGroup"]);
      setCanJoinGroup(access);
    };
    fetchAccess();
  }, []);

  const handleStartChat = () => {
    navigate("/users");
  };

  const handleJoinGroupChat = () => {
    navigate("/groups");
  };

  return (
    <div className="new-chat-section">
      <label className="chat-label">
        Click this button to start a new chat:
      </label>
      <button className="start-chat-button" onClick={handleStartChat}>
        Start a new chat
      </button>
      {canJoinGroup && (
        <button className="start-chat-button" onClick={handleJoinGroupChat}>
          Join a group chat
        </button>
      )}
    </div>
  );
};

export default NewChatSection;
