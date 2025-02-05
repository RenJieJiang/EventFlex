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

    const response = await axios.post(loginUrl, parsedResult.data, {
      httpsAgent,
      headers: { "Content-Type": "application/json" },
      withCredentials: true, // Allow sending cookies in requests
    });

    if (response.status < 200 || response.status >= 300) {
      return { errors: { email: ["Invalid email or password"] } };
    }

    const setCookieHeader = response.headers["set-cookie"];
    console.log("setCookieHeader", setCookieHeader);
    if (setCookieHeader && setCookieHeader.length > 0) {
      const cookieStore = await cookies();
      setCookieHeader.forEach((cookieString: string) => {
        const [cookieNameValue, ...cookieAttributes] = cookieString.split("; ");
        const [cookieName, cookieValue] = cookieNameValue.split("=");
        cookieStore.set(cookieName, cookieValue, {
          path: "/",
          secure: true,
          httpOnly: cookieAttributes.includes("httponly"),
          sameSite: cookieAttributes.includes("samesite=none") ? "none" : undefined,
          expires: new Date(cookieAttributes.find(attr => attr.startsWith("expires="))?.split("=")[1] || ""),
        });
      });

      console.log("response.data", response.data);

      cookieStore.set("id", response.data.id);
      cookieStore.set("userName", response.data.userName);
      cookieStore.set("email", response.data.email);
    }

    return { user: response.data };
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

  // redirect(`/dashboard`);
}
