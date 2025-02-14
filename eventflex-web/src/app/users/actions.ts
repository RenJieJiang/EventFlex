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
  const response = await customFetch(`${process.env.NEXT_PUBLIC_API_URL}/users`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  }) as unknown as Response;

  if (!response.ok) {
    throw new Error(`Error fetching users: ${response.statusText}`);
  }

  return response.json();
};

export const deleteUser = async (id: string): Promise<void> => {
  const response = await customFetch(`${process.env.NEXT_PUBLIC_API_URL}/users/${id}`, {
    method: "DELETE",
    credentials: "include",
  }) as unknown as Response;

  if (!response.ok) {
    throw new Error(`Error deleting user: ${response.statusText}`);
  }
};