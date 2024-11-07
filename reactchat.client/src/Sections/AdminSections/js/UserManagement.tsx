import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import '../css/UserManagement.css';

interface User {
    id: string;
    username: string;
    email: string;
    role: string;
}

const UserManagementPage: React.FC = () => {
    const [users, setUsers] = useState<User[]>([]);
    const [editingUser, setEditingUser] = useState<User | null>(null);
    const [newUserData, setNewUserData] = useState<{ username: string; email: string; role: string }>({
        username: '',
        email: '',
        role: 'RegularUser',
    });
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const token = Cookies.get('token');
    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await axios.get('https://localhost:7240/user/getAllUsers', {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setUsers(response.data);
            } catch (err) {
                console.error(err);
                setError('Failed to load users.');
            } finally {
                setLoading(false);
            }
        };

        fetchUsers();
    }, []);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setNewUserData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    const handleSave = async () => {
        if (!editingUser) return;

        const updatedUser = {
            ...editingUser,
            username: newUserData.username,
            email: newUserData.email,
        };

        try {
            await axios.put(`https://localhost:7240/user/updateUser`, updatedUser, {
                headers: { Authorization: `Bearer ${token}` }
            });
            setUsers((prevUsers) =>
                prevUsers.map((user) => (user.id === updatedUser.id ? updatedUser : user))
            );
            setEditingUser(null);
        } catch (err) {
            console.error(err);
            setError('Failed to update user.');
        }
    };

    const handleEdit = (user: User) => {
        setEditingUser(user);
        setNewUserData({
            username: user.username,
            email: user.email,
            role: user.role,
        });
    };

    const handleDelete = async (userId: string) => {
        try {
            await axios.delete(`https://your-backend-url/api/users/${userId}`);
            setUsers((prevUsers) => prevUsers.filter((user) => user.id !== userId));
        } catch (err) {
            console.error(err);
            setError('Failed to delete user.');
        }
    };

    if (loading) return <div>Loading users...</div>;

    return (
        <div className="user-management-page">
            <div className="page-title">User Management</div>
            {error && <div className="error-message">{error}</div>}
            <div className="user-list">
                {users.map((user) => (
                    <div key={user.id} className="user-card">
                        <p>Username: {user.username}</p>
                        <p>Email: {user.email}</p>
                        <p>Role: {user.role}</p>
                        <button className="manage-button" onClick={() => handleEdit(user)}>Edit</button>
                        <button className="manage-button" onClick={() => handleDelete(user.id)}>Delete</button>
                    </div>
                ))}
            </div>

            {editingUser && (
                <div className="edit-form">
                    <h3>Edit User</h3>
                    <div>
                        <label>Username:</label>
                        <input
                            type="text"
                            name="username"
                            value={newUserData.username}
                            onChange={handleChange}
                        />
                    </div>
                    <div>
                        <label>Email:</label>
                        <input
                            type="email"
                            name="email"
                            value={newUserData.email}
                            onChange={handleChange}
                        />
                    </div>
                    <button className="manage-button" onClick={handleSave}>Save</button>
                    <button className="manage-button" onClick={() => setEditingUser(null)}>Cancel</button>
                </div>
            )}
        </div>
    );
};

export default UserManagementPage;
