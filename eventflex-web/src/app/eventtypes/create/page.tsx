"use client";

import { customFetch } from "@/lib/customFetch";
import EventTypeForm, { EventTypeFormData } from "../components/EventTypeForm";
import { useRouter } from "next/navigation";


export default function AddEventTypePage() {
  const router = useRouter();

  const handleSubmit = async (data: EventTypeFormData) => {
    try {
      await customFetch(`${process.env.NEXT_PUBLIC_API_URL_EVENTTYPE}/eventtypes`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify(data),
      });

      router.push("/eventtypes");
    } catch (error) {
      console.error("Failed to create event type:", error);
    }
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4 text-gray-700">Add EventType</h1>
      <EventTypeForm onSubmit={handleSubmit} />
    </div>
  );
}