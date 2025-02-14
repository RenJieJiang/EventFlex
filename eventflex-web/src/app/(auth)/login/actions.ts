"use server";

import { loginSchema } from "@/lib/schemas/auth";
import { httpsAgent } from "@/lib/utils";
import axios from "axios";
import { cookies } from "next/headers";

export async function login(prevState: any, formData: FormData) {
  console.log("login function called");

  const parsedResult = loginSchema.safeParse(
    Object.fromEntries(formData.entries())
  );

  if (!parsedResult.success) {
    console.log("Validation failed", parsedResult.error.flatten().fieldErrors);
    return { errors: parsedResult.error.flatten().fieldErrors };
  }

  try {
    const baseUrl = process.env.NEXT_PUBLIC_API_URL;
    if (!baseUrl) {
      throw new Error("NEXT_PUBLIC_API_URL is not defined");
    }

    console.log("baseUrl", baseUrl);

    const urlObj = new URL(baseUrl);
    const loginUrl = `${baseUrl}/auth/login`;

    const fetchOptions = {
      headers: { "Content-Type": "application/json" },
      withCredentials: true, // Allow sending cookies in requests
      agent: urlObj.protocol === "https:" ? httpsAgent : undefined, // Use custom agent only for https
    };

    console.log("loginUrl", loginUrl);
    console.log("fetchOptions", fetchOptions);
    console.log("parsedResult.data", parsedResult.data);

    const response = await axios.post(loginUrl, parsedResult.data, fetchOptions);

    if (response.status < 200 || response.status >= 300) {
      console.log("Login failed with status", response.status);
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
              headers: error.response.headers,
            }
          : null,
        request: error.request
          ? {
              headers: error.request.headers,
              method: error.request.method,
              url: error.request.path, // Use `path` instead of `url` for the request object
            }
          : null,
        config: error.config, // Log the Axios request configuration
      });
    } else {
      console.error("Unexpected error:", error);
    }
    return { errors: { email: ["Invalid email or password"] } };
  }
}