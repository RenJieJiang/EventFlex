"use server";

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
  const response = await axios.get("https://localhost:8081/api/users", { httpsAgent } );
  // throw an error for test purposes
  //throw new Error('Test Error');
  return response.data;
};

export const deleteUser = async (id: string): Promise<void> => {
  await axios.delete(`http://localhost:8080/api/users/${id}`, { httpsAgent });
};