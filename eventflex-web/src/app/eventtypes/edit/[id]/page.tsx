"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import EventTypeForm, { EventTypeFormData } from "../../components/EventTypeForm";
import { customFetch } from "@/lib/customFetch";

interface EventType {
  id: string;
  name: string;
  description: string;
}

export default function EditEventTypePage() {
  const [eventType, setEventType] = useState<EventType | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const { id } = useParams();
  const router = useRouter();

  useEffect(() => {
    const fetchEventType = async () => {
      try {
        const response = await customFetch<EventType>(`${process.env.NEXT_PUBLIC_API_URL_EVENTTYPE}/eventtypes/${id}`, {
          method: "GET",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        });

        setEventType(response); 
      } catch (error: any) {
        setError("Failed to fetch event type with error: " + error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchEventType();
  }, [id]);

  const handleSubmit = async (data: EventTypeFormData) => {
    try {
      await customFetch<EventType>(`${process.env.NEXT_PUBLIC_API_URL_EVENTTYPE}/eventtypes/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(data),
      });

      router.push("/eventtypes");
    } catch (error: any) {
      setError("Failed to update event type with error: " + error.message);
    }
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4 text-gray-700">Edit Event Type</h1>
      <EventTypeForm initialData={eventType || undefined} onSubmit={handleSubmit} />
    </div>
  );
}