"use server";
import axios from "axios";
import { cookies } from "next/headers";
import { httpsAgent } from "@/lib/utils";
import { redirect } from "next/navigation";
import { clearCookiesAndRedirect } from "./clearCookiesAndRedirect";

const serverApi = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  // httpsAgent,
});

const extractToken = (cookieString: string): string | null => {
  const match = cookieString.match(/access_token=([^;]+)/);
  return match ? match[1] : null;
};

serverApi.interceptors.request.use(async (config) => {
  const cookieStore = await cookies();
  const cookieString = cookieStore.get("access_token")?.value;
  const token = cookieString ? extractToken(cookieString) : null;

  if (token) {
    console.log("Setting token in request header:", token);
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

serverApi.interceptors.response.use(
  (response) => response,
  async (error) => {
    console.log("Server API error:", error);
    if (error.response && error.response.status === 401) {
      redirect("/login");
    }
    return Promise.reject(error);
  }
);

export default serverApi;