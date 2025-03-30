import { useState, useEffect } from "react";
import { useChatHub } from "../../../components/ChatHub/js/useChatHub";
import { useNavigate } from "react-router-dom";
import "../css/GroupsPage.css";

const GroupsPage = () => {
  const [groups, setGroups] = useState<string[]>([]);
  const connection = useChatHub();
  const navigate = useNavigate();

  useEffect(() => {
    const startAndFetchGroups = async () => {
      if (connection) {
        try {
          if (connection.state !== "Connected") {
            await connection.start();
            console.log("SignalR connection started");
          }
          const fetchedGroups: string[] = await connection.invoke(
            "GetUserGroups"
          );
          setGroups(fetchedGroups);
        } catch (error) {
          console.error("Error with connection:", error);
        }

        connection.on("ReceiveGroupCreated", (groupName: string) => {
          setGroups((prevGroups) => [...prevGroups, groupName]);
        });
      }
    };

    startAndFetchGroups();

    return () => {
      if (connection) {
        connection.off("ReceiveGroupCreated");
      }
    };
  }, [connection]);

  const handleAddNewGroup = () => {
    navigate("/new-group");
  };

  return (
    <div className="groups-container">
      <h1 className="groups-title">Your Groups</h1>
      <div className="groups-list">
        {groups.map((group, index) => (
          <div className="group-card" key={index}>
            {group}
          </div>
        ))}
      </div>
      <div className="add-group-container">
        <button className="add-group-btn" onClick={handleAddNewGroup}>
          Add New Group
        </button>
      </div>
    </div>
  );
};

export default GroupsPage;
