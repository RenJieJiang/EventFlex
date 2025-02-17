"use server";

import { customFetch } from "@/lib/customFetch";
import { isDevelopment } from "@/lib/utils";

export interface EventType {
  id: string;
  name: string;
  description: string;
}

console.log('isDevelopment', isDevelopment);

export const fetchEventTypes = async (): Promise<EventType[]> => {
  const response = await customFetch<EventType[]>(`${process.env.NEXT_PUBLIC_API_URL_EVENTTYPE}/eventtypes`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
  });

  return response;
};

export const deleteEventType = async (id: string): Promise<void> => {
  await customFetch<void>(`${process.env.NEXT_PUBLIC_API_URL_EVENTTYPE}/eventtypes/${id}`, {
    method: "DELETE",
    credentials: "include",
  });
};