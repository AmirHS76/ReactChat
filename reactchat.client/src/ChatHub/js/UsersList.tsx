import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import axios from 'axios';
interface User {
    username: string;
    email: string;
}

const UserList: React.FC = () => {
    const [users, setUsers] = useState<User[]>([]);
    const navigate = useNavigate();
    const [currentUsername, setCurrentUsername] = useState<string | null>(null);

    useEffect(() => {
        const fetchCurrentUser = async () => {
            const token = Cookies.get('token');
            try {
                const response = await axios.get("https://localhost:7240/user", {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                const user = await response.data;
                setCurrentUsername(user.username);
            }
            catch {
                console.error("Failed to fetch current user");
            }
        };

        const fetchUsers = async () => {
            const token = Cookies.get('token');
            try {
                const response = await axios.get("https://localhost:7240/user/getAll", {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });

                const userList: User[] = await response.data;
                setUsers(userList.filter(user => user.username !== currentUsername));
            } 
            catch {
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
                    <li key={user.username} onClick={() => handleUserSelect(user.username)}>
                        {user.username}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default UserList;
