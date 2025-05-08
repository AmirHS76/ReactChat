import { useState, useEffect } from "react";
import { useChatHub } from "../../../../components/ChatHub/js/useChatHub";
import "../css/NewGroupSection.css";
import Cookies from "js-cookie";
import UserRepository from "../../../../Repositories/UserRepository";
import User from "../../../../types/users";
import { toast } from "react-toastify";

const NewGroupSection = () => {
  const [groupName, setGroupName] = useState("");
  const [allUsers, setAllUsers] = useState<User[]>([]);
  const [selectedUsernames, setSelectedUsernames] = useState<string[]>([]);
  const connection = useChatHub();

  useEffect(() => {
    const fetchUsers = async () => {
      const userRepo = new UserRepository();
      if (connection) {
        if (connection.state !== "Connected") {
          try {
            await connection.start();
          } catch (error) {
            console.error("Error starting connection:", error);
            return;
          }
        }
        try {
          const currentUsername = Cookies.get("username");

          const response = await userRepo.getUsers();

          const userList: User[] = response as User[];
          setAllUsers(
            userList.filter((user) => user.username !== currentUsername)
          );
        } catch (error) {
          console.error("Error fetching users:", error);
        }
      }
    };

    fetchUsers();
  }, [connection]);

  const toggleUserSelection = (username: string) => {
    setSelectedUsernames((prev) =>
      prev.includes(username)
        ? prev.filter((id) => id !== username)
        : [...prev, username]
    );
  };

  const handleCreateGroup = async () => {
    if (connection && groupName && selectedUsernames.length > 0) {
      if (connection.state !== "Connected") {
        try {
          await connection.start();
          console.log("SignalR connection started");
        } catch (error) {
          console.error("Error starting connection:", error);
          return;
        }
      }
      try {
        await connection.invoke("CreateGroup", groupName, selectedUsernames);
        toast.info("Group created successfully!");
        setGroupName("");
        setSelectedUsernames([]);
      } catch (error) {
        console.error("Error creating group:", error);
      }
    } else {
      toast.error("Please enter a group name and select users.");
    }
  };

  return (
    <div className="new-group-section">
      <label className="group-label">Create a new group:</label>
      <input
        type="text"
        value={groupName}
        onChange={(e) => setGroupName(e.target.value)}
        placeholder="Enter group name"
      />

      <div className="users-list">
        <p>Select users to add:</p>
        {allUsers.length > 0 ? (
          <ul>
            {allUsers.map((user) => (
              <li key={user.id}>
                <label>
                  <input
                    type="checkbox"
                    checked={selectedUsernames.includes(user.username)}
                    onChange={() => toggleUserSelection(user.username)}
                  />
                  {user.username}
                </label>
              </li>
            ))}
          </ul>
        ) : (
          <p>No users available.</p>
        )}
      </div>

      <button onClick={handleCreateGroup}>Create Group</button>
    </div>
  );
};

export default NewGroupSection;
