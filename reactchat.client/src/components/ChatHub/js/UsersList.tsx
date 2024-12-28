import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import UserRepository from "../../../Repositories/UserRepository";
interface User {
  username: string;
  email: string;
}

const UserList: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const navigate = useNavigate();
  const [currentUsername, setCurrentUsername] = useState<string | null>(null);
  const userRepo = new UserRepository();
  useEffect(() => {
    const fetchCurrentUser = async () => {
      try {
        const response = await userRepo.getCurrentUser();
        const user = (await response).data as User;
        setCurrentUsername(user.username);
      } catch {
        console.error("Failed to fetch current user");
      }
    };

    const fetchUsers = async () => {
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
    <div>
      <h2>Select a user to chat with:</h2>
      <ul>
        {users.map((user) => (
          <li
            key={user.username}
            onClick={() => handleUserSelect(user.username)}
          >
            {user.username}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default UserList;
