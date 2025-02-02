import axios, {
  AxiosRequestConfig,
  AxiosResponse,
  InternalAxiosRequestConfig,
} from "axios";
import Cookies from "js-cookie";
import { toast } from "react-toastify";

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 5000,
  headers: {
    "Content-Type": "application/json",
  },
});

apiClient.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = Cookies.get("token");
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

apiClient.interceptors.response.use(
  (response: AxiosResponse) => response,
  async (error) => {
    console.error("API request error:", error);
    const originalRequest = error.config;

    if (error.response?.status === 403) {
      toast.error("You don't have permission to do this action.", {
        position: "top-center",
        autoClose: 5000,
      });
      return Promise.reject(error);
    }

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      const refreshToken = Cookies.get("refreshToken");
      console.log("Unauthorized access - redirecting to login.");
      Cookies.remove("token");
      if (refreshToken) {
        try {
          const response = await apiClient.get(
            `/login?refreshToken=${refreshToken}`
          );

          if (response.status === 200) {
            const newToken = response.data.token;
            Cookies.set("token", newToken);
            apiClient.defaults.headers.common[
              "Authorization"
            ] = `Bearer ${newToken}`;
            originalRequest.headers["Authorization"] = `Bearer ${newToken}`;
            return apiClient(originalRequest);
          } else {
            Cookies.remove("refreshToken");
          }
        } catch (err) {
          Cookies.remove("refreshToken");
          return Promise.reject(err);
        }
      }
    }
    return Promise.reject(error);
  }
);

export const getRequest = async (url: string, config?: AxiosRequestConfig) => {
  try {
    const response = await apiClient.get(url, config);
    return response;
  } catch (error) {
    console.error("GET request error:", error);
    throw error;
  }
};

export const postRequest = async (
  url: string,
  data: any,
  config?: AxiosRequestConfig
) => {
  try {
    const response = await apiClient.post(url, data, config);
    return response;
  } catch (error) {
    console.error("POST request error:", error);
    throw error;
  }
};

export const putRequest = async (
  url: string,
  data: any,
  config?: AxiosRequestConfig
) => {
  try {
    const response = await apiClient.put(url, data, config);
    return response.data;
  } catch (error) {
    console.error("PUT request error:", error);
    throw error;
  }
};
export const patchRequest = async (
  url: string,
  data: any,
  config?: AxiosRequestConfig
) => {
  try {
    const response = await apiClient.patch(url, data, config);
    return response.data;
  } catch (error) {
    console.error("PATCH request error:", error);
    throw error;
  }
};
export const deleteRequest = async (
  url: string,
  config?: AxiosRequestConfig
) => {
  try {
    const response = await apiClient.delete(url, config);
    return response.data;
  } catch (error) {
    console.error("DELETE request error:", error);
    throw error;
  }
};

export default apiClient;
