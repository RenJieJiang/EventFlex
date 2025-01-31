"use server";
import api from "@/lib/api";
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

const isDevelopment = process.env.NEXT_PUBLIC_ENV === 'development';
console.log('isDevelopment', isDevelopment);

const httpsAgent = new https.Agent({
  rejectUnauthorized: !isDevelopment,
});

export const fetchUsers = async (): Promise<User[]> => {
  // simulate a slow network request
  await new Promise(resolve => setTimeout(resolve, 1000));
  const response = await axios.get(`${process.env.NEXT_PUBLIC_API_URL}/api/users`, { httpsAgent });
  return response.data;
};

export const deleteUser = async (id: string): Promise<void> => {
  await axios.delete(`${process.env.NEXT_PUBLIC_API_URL}/api/users/${id}`, { httpsAgent });
};