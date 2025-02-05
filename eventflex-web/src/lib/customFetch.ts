"use server";

import https from "https";
import { cookies } from "next/headers";
import { redirect } from "next/navigation";
import { URL } from "url";

const isDevelopment = process.env.NODE_ENV === "development";

const httpsAgent = new https.Agent({
  rejectUnauthorized: !isDevelopment,
});

export const customFetch = async (url: string, options: RequestInit = {}): Promise<Response> => {
  const urlObj = new URL(url);
  const cookieStore = await cookies();
  const token = cookieStore.get("access_token")?.value;

  console.log("customFetch token: ", token);

  const fetchOptions: https.RequestOptions = {
    ...options,
    agent: isDevelopment ? httpsAgent : undefined, // Only use custom agent in development to ignore self-signed certificate issue
    headers: {
      ...(options.headers as Record<string, string>),
      Authorization: token ? `Bearer ${token}` : undefined, // Add token to headers
    } as Record<string, string | string[]>,
    signal: options.signal instanceof AbortSignal ? options.signal : undefined,
  };

  return new Promise((resolve, reject) => {
    const req = https.request(urlObj, fetchOptions, (res) => {
      let data = '';
      res.on('data', (chunk) => {
        data += chunk;
      });
      res.on('end', () => {
        const response = new Response(data, {
          status: res.statusCode || 200,
          statusText: res.statusMessage,
          headers: new Headers(res.headers as Record<string, string>),
        });

        // Check if the response is unauthorized
        if (response.status === 401) {
          // Clear the cookie and redirect to login page
          cookieStore.set({
            name: "access_token",
            value: "",
            maxAge: -1, // Set maxAge to -1 to delete the cookie
            path: "/", // Ensure the cookie is deleted from the root path
          });

          redirect('/login');
        }

        resolve(response);
      });
    });

    req.on('error', (e) => {
      reject(e);
    });

    if (options.body) {
      if (typeof options.body === 'object') {
        req.write(JSON.stringify(options.body));
      } else {
          req.write(options.body);
      }
    }

    req.end();
  });
};