import axios from "axios";
import { cookies } from "next/headers";

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
});

export const setupInterceptors = async () => {
  // get token from local storage and set it in the request header
  const cookieStore = await cookies();

  api.interceptors.request.use((config) => {

    const token = cookieStore.get("access_token")?.value;

    if (token) {
      console.log("Setting token in request header:", token);
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });
};

export default api;