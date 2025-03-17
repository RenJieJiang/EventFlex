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
  const apiUrl = `${process.env.NEXT_PUBLIC_API_URL_EVENTTYPE}/eventtypes`;
  console.log('Fetching event types from:', apiUrl);

  try {
    const response = await customFetch<EventType[]>(apiUrl, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    });

    console.log('Response data:', response);

    return response;
  } catch (error) {
    console.error('Error fetching event types:', error);
    throw error;
  }
};

export const deleteEventType = async (id: string): Promise<void> => {
  await customFetch<void>(`${process.env.NEXT_PUBLIC_API_URL_EVENTTYPE}/eventtypes/${id}`, {
    method: "DELETE",
    credentials: "include",
  });
};