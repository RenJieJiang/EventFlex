"use server";

import { loginSchema } from "@/lib/schemas/auth";
import { httpsAgent } from "@/lib/utils";
import axios from "axios";
import { redirect } from "next/navigation";
import * as https from "https";
import { cookies } from "next/headers";


export async function login(prevState: any, formData: FormData) {
  const parsedResult = loginSchema.safeParse(
    Object.fromEntries(formData.entries())
  );

  if (!parsedResult.success) {
    return { errors: parsedResult.error.flatten().fieldErrors };
  }

  try {
    const loginUrl = `${process.env.NEXT_PUBLIC_API_URL}/auth/login`;
    console.log("login url:", loginUrl);
    const httpsAgent = new https.Agent({
      rejectUnauthorized: false,
    });
    const response = await axios.post(loginUrl, parsedResult.data, {
      httpsAgent,
      headers: { "Content-Type": "application/json" },
      withCredentials: true, // Allow sending cookies in requests
    });
    console.log("response headers:", response.headers);
    console.log("response cookies:", response.headers["set-cookie"]);
    if (response.status < 200 || response.status >= 300) {
      return { errors: { email: ["Invalid email or password"] } };
    }
    const setCookieHeader = response.headers["set-cookie"];
    if (setCookieHeader && setCookieHeader.length > 0) {
      (await cookies()).set("access_token", setCookieHeader[0]);
    }
    // Return the cookies and response data
    // return { cookies: response.headers["set-cookie"], data: response.data };
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error("Axios error:", {
        message: error.message,
        response: error.response
          ? {
              status: error.response.status,
              data: error.response.data,
            }
          : null,
        request: error.request
          ? {
              headers: error.request.headers,
              method: error.request.method,
              url: error.request.url,
            }
          : null,
      });
    } else {
      console.error("Unexpected error:", error);
    }
    return { errors: { email: ["Invalid email or password"] } };
  }

  redirect(`/dashboard`);
}
