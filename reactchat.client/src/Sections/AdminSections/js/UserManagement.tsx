import React, { useState, useEffect } from "react";
import "../css/UserManagement.css";
import UserRepository from "../../../Repositories/UserRepository";
import userModel from "../../../types/users";

const UserManagementPage: React.FC = () => {
  const [users, setUsers] = useState<userModel[]>([]);
  const [editingUser, setEditingUser] = useState<userModel | null>(null);
  const [newUserData, setNewUserData] = useState<{
    username: string;
    email: string;
    role: string;
  }>({
    username: "",
    email: "",
    role: "RegularUser",
  });
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const userRepository = new UserRepository();

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const usersData = (await userRepository.getUsers()) as userModel[];
        setUsers(usersData);
      } catch (err) {
        console.error(err);
        setError("Failed to load users.");
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
      await userRepository.updateUser(updatedUser);
      setUsers((prevUsers) =>
        prevUsers.map((user) =>
          user.id === updatedUser.id ? updatedUser : user
        )
      );
      setEditingUser(null);
    } catch (err) {
      console.error(err);
      setError("Failed to update user.");
    }
  };

  const handleEdit = (user: userModel) => {
    setEditingUser(user);
    setNewUserData({
      username: user.username,
      email: user.email,
      role: user.role,
    });
  };

  const handleDelete = async (userId: number) => {
    try {
      await userRepository.deleteUser(userId);
      setUsers((prevUsers) => prevUsers.filter((user) => user.id !== userId));
    } catch (err) {
      console.error(err);
      setError("Failed to delete user.");
    }
  };

  if (loading) return <div>Loading users...</div>;

  return (
    <div className="user-management-page">
      <div className="page-title">User Management</div>
      {error && <div className="error-message">{error}</div>}
      <div className="user-list">
        {users.map((user) => (
          <div key={user.id} className="user-card-container">
            <div key={user.id} className="user-card">
              <p>Username: {user.username}</p>
              <p>Email: {user.email}</p>
              <p>Role: {user.role}</p>
            </div>
            <div>
              <button
                className="manage-button"
                onClick={() => handleEdit(user)}
              >
                Edit
              </button>
              <button
                className="manage-button"
                onClick={() => handleDelete(user.id)}
              >
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>

      {editingUser && (
        <div className="modal">
          <div className="modal-content">
            <h3>Edit User</h3>
            <div className="form-group">
              <label>Username:</label>
              <input
                type="text"
                name="username"
                value={newUserData.username}
                onChange={handleChange}
              />
            </div>
            <div className="form-group">
              <label>Email:</label>
              <input
                type="email"
                name="email"
                value={newUserData.email}
                onChange={handleChange}
              />
            </div>
            <div className="button-group">
              <button className="save-button" onClick={handleSave}>
                Save
              </button>
              <button
                className="cancel-button"
                onClick={() => setEditingUser(null)}
              >
                Cancel
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default UserManagementPage;
