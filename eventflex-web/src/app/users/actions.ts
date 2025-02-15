"use server";

import { customFetch } from "@/lib/customFetch";
import { isDevelopment } from "@/lib/utils";

export interface User {
  tenantId: string;
  id: string;
  name: string;
  userName: string;
  email: string;
  phoneNumber: string | null;
  token: string;
  refreshToken: string;
}

console.log('isDevelopment', isDevelopment);

export const fetchUsers = async (): Promise<User[]> => {
  const response = await customFetch<User[]>(`${process.env.NEXT_PUBLIC_API_URL}/users`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });

  return response;
};

export const deleteUser = async (id: string): Promise<void> => {
  await customFetch<void>(`${process.env.NEXT_PUBLIC_API_URL}/users/${id}`, {
    method: "DELETE",
    credentials: "include",
  });
};