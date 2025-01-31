"use server";

import { loginSchema } from "@/lib/schemas/auth";
import { createSession } from "@/lib/session";
import { httpsAgent } from "@/lib/utils";
import axios from "axios";
import { redirect } from "next/navigation";

export async function login(prevState: any, formData: FormData) {
  const parsedResult = loginSchema.safeParse(Object.fromEntries(formData.entries()));

  if (!parsedResult.success) {
      return { errors: parsedResult.error.flatten().fieldErrors };
  }

  try {
    const loginUrl = `${process.env.NEXT_PUBLIC_API_URL}/auth/login`;
    console.log("login url:", loginUrl);
    const response = await axios.post(loginUrl, parsedResult.data, { 
      httpsAgent,
      headers: { 'Content-Type': 'application/json' }
    });

    if (response.status >= 200 && response.status < 300) {
      const data = response.data;
      console.log("backend login success with data:", data);
      await createSession(data.userId); // 假设返回的数据包含userId
    } else {
      return { errors: { email: ["Invalid email or password"] } };
    }
  } catch (error) {
    console.error("Failed to login:", JSON.stringify(error,null,2));
    return { errors: { email: ["Invalid email or password"] } };
  }

  redirect(`/dashboard`);
}