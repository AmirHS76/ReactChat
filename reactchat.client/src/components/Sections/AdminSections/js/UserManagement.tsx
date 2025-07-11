import React, { useState, useEffect } from "react";
import "../css/UserManagement.css";
import UserRepository from "../../../../Repositories/UserRepository";
import userModel from "../../../../types/users";
import { UpdateUserRequest } from "../../../../contexts/UpdateUserRequest";

const UserManagementPage: React.FC = () => {
  const [users, setUsers] = useState<userModel[]>([]);
  const [editingUser, setEditingUser] = useState<userModel | null>(null);
  const [newUserData, setNewUserData] = useState<UpdateUserRequest>();
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const userRepository = React.useMemo(() => new UserRepository(), []);

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
  }, [userRepository]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setNewUserData((prevData) => {
      if (!prevData) return prevData;
      return {
        ...prevData,
        [name]: value,
      };
    });
  };

  const handleSave = async () => {
    if (!editingUser) return;

    if (!newUserData?.username || !newUserData?.email) {
      setError("Username and email are required.");
      return;
    }

    const updatedUser: userModel = {
      ...editingUser,
      username: newUserData.username,
      email: newUserData.email,
      isDisabled: newUserData.isDisabled,
    };

    try {
      await userRepository.updateUser(updatedUser);
      setUsers((prevUsers) =>
        prevUsers.map((user) =>
          user.id === updatedUser.id ? updatedUser : user
        )
      );
      setEditingUser(null);
    } catch {
      setError("Failed to update user.");
    }
  };

  const handleEdit = (user: userModel) => {
    setEditingUser(user);
    setNewUserData({
      id: user.id,
      username: user.username,
      email: user.email,
      role: user.role,
      isDisabled: user.isDisabled,
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
          <div key={user.id} className="user-card">
            <div className="user-info">
              <p>Username: {user.username}</p>
              <p>Email: {user.email}</p>
              <p>Role: {user.role}</p>
              <p>Is Disabled : {user.isDisabled ? "True" : "False"}</p>
            </div>
            <div className="user-actions">
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
                value={newUserData?.username}
                onChange={handleChange}
              />
            </div>
            <div className="form-group">
              <label>Email:</label>
              <input
                type="email"
                name="email"
                value={newUserData?.email}
                onChange={handleChange}
              />
            </div>
            <div className="form-group">
              <label>Is disabled:</label>
              <select
                name="isDisabled"
                value={newUserData?.isDisabled ? "true" : "false"}
                onChange={(e) =>
                  setNewUserData((prevData) =>
                    prevData
                      ? { ...prevData, isDisabled: e.target.value === "true" }
                      : prevData
                  )
                }
              >
                <option value="false">false</option>
                <option value="true">true</option>
              </select>
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
