import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import UserRepository from "../../../Repositories/UserRepository";
import "../../../styles/UserList.css";
import Cookies from "js-cookie";
interface User {
  username: string;
  email: string;
}

const UserList: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const navigate = useNavigate();
  const [currentUsername, setCurrentUsername] = useState<string | null>(null);
  useEffect(() => {
    const userRepo = new UserRepository();
    const fetchCurrentUser = async () => {
      if (currentUsername) return;
      try {
        const username = Cookies.get("username");
        if (username) {
          setCurrentUsername(username);
          return;
        }
        const response = await userRepo.getCurrentUser();
        const user = (await response).data as User;
        Cookies.set("username", user.username, {
          secure: true,
          sameSite: "Strict",
        });

        setCurrentUsername(user.username);
      } catch {
        console.error("Failed to fetch current user");
      }
    };

    const fetchUsers = async () => {
      if (!currentUsername) return;
      try {
        const response = await userRepo.getUsers();

        const userList: User[] = response as User[];
        setUsers(userList.filter((user) => user.username !== currentUsername));
      } catch {
        console.error("Failed to fetch users");
      }
    };

    fetchCurrentUser().then(fetchUsers);
  }, [currentUsername]);

  const handleUserSelect = (username: string) => {
    navigate(`/privateChat/${username}`);
  };

  return (
    <div className="user-list-container">
      <h2>Select a user to chat with:</h2>
      <ul className="user-list">
        {users.map((user) => (
          <li
            key={user.username}
            onClick={() => handleUserSelect(user.username)}
            className="user-list-item"
          >
            {user.username}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default UserList;
