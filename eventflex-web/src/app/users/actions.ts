import api, { setupInterceptors } from "@/lib/api";
import serverApi, { setupServerInterceptors } from "@/lib/serverApi";
import { isDevelopment } from "@/lib/utils";
import axios from "axios";
import https from "https";

export interface User {
  tenantId: string;
  id: string;
  userName: string;
  email: string;
  emailConfirmed: boolean;
  phoneNumber: string | null;
  lockoutEnabled: boolean;
}

console.log('isDevelopment', isDevelopment);

const httpsAgent = new https.Agent({
  rejectUnauthorized: !isDevelopment,
});

export const fetchUsers = async (): Promise<User[]> => {
  // await setupServerInterceptors();

  const response = await serverApi.get(`/users`, { 
    withCredentials: true,
    headers: { "Content-Type": "application/json" },
    httpsAgent,
  });
  return response.data;
};

export const deleteUser = async (id: string): Promise<void> => {
  await axios.delete(`${process.env.NEXT_PUBLIC_API_URL}/users/${id}`, { httpsAgent });
};