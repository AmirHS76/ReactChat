import {
  getRequest,
  postRequest,
  putRequest,
  deleteRequest,
  patchRequest,
} from "../services/apiService";
import User from "../types/users";

class UserRepository {
  async fetchUserRole(): Promise<string> {
    try {
      const response = await getRequest("/user/GetRole");
      return (response.data as { role: string }).role;
    } catch (error) {
      console.error("Error fetching user role:", error);
      return "";
    }
  }

  async getUsers() {
    try {
      const response = await getRequest("/user/getAll");
      return response.data as User[];
    } catch (error) {
      console.error("Error fetching users:", error);
      throw error;
    }
  }

  async updateUser(user: User) {
    try {
      const response = await putRequest("/user", user);
      return response.data;
    } catch (error) {
      console.error("Error updating user:", error);
      throw error;
    }
  }

  async deleteUser(userId: number) {
    try {
      const response = (await deleteRequest(`/user/${userId}`)) as {
        data: unknown;
      };
      return response.data;
    } catch (error) {
      console.error("Error deleting user:", error);
      throw error;
    }
  }

  async getUser(userID: number) {
    try {
      const response = await getRequest(`Users/id=${userID}`);
      return response;
    } catch (error) {
      console.error("Error fetching user:", error);
      throw error;
    }
  }

  async addUser(userModel: User) {
    try {
      const response = await postRequest(`user`, userModel);
      return response;
    } catch (error) {
      console.error("Error Adding user: ", error);
      throw error;
    }
  }
  async getUserData() {
    return await getRequest("api/v1/authenticate/data");
  }

  async updateEmail(newEmail: string) {
    return await patchRequest(`user/${newEmail}`, { email: newEmail });
  }

  async getCurrentUser() {
    return await getRequest("user");
  }
}

export default UserRepository;
