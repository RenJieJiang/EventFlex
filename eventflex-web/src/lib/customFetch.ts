"use server";

import https from "https";
import { cookies } from "next/headers";
import { URL } from "url";

const isDevelopment = process.env.NODE_ENV === "development";

const httpsAgent = new https.Agent({
  rejectUnauthorized: !isDevelopment,
});

export const customFetch = async (url: string, options: RequestInit = {}) => {
  const urlObj = new URL(url);
  const cookieStore = await cookies();
  const token = cookieStore.get("access_token")?.value;

  console.log("customFetch token: ", token);

  const fetchOptions: https.RequestOptions = {
    ...options,
    agent: isDevelopment ? httpsAgent : undefined,
    headers: {
      ...(options.headers as Record<string, string>),
      Authorization: token ? `Bearer ${token}` : undefined,
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
        resolve(new Response(data, {
          status: res.statusCode,
          statusText: res.statusMessage,
          headers: new Headers(res.headers as Record<string, string>),
        }));
      });
    });

    req.on('error', (e) => {
      reject(e);
    });

    if (options.body) {
      req.write(options.body);
    }

    req.end();
  });
};