"use client";

import { eventTypeSchema } from "@/lib/schemas/eventType";
import { zodResolver } from "@hookform/resolvers/zod";
import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";
import { z } from "zod";

export type EventTypeFormData = z.infer<typeof eventTypeSchema>;

interface EventTypeFormProps {
  initialData?: EventTypeFormData;
  onSubmit: (data: EventTypeFormData) => void;
}

export default function EventTypeForm({
  initialData,
  onSubmit,
}: EventTypeFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<EventTypeFormData>({
    resolver: zodResolver(eventTypeSchema),
    defaultValues: initialData,
  });
  const router = useRouter();

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      {initialData?.id && <input type="hidden" {...register("id")} />}
      <div>
        <label className="block text-gray-700">Name</label>
        <input
          type="text"
          {...register("name")}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm"
        />
        {errors.name && (
          <p className="text-red-500 text-sm">{errors.name.message}</p>
        )}
      </div>
      <div>
        <label className="block text-gray-700">Description</label>
        <input
          type="text"
          {...register("description")}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm"
        />
        {errors.description && (
          <p className="text-red-500 text-sm">{errors.description.message}</p>
        )}
      </div>
      <div className="flex justify-between">
        <button
          type="button"
          onClick={() => router.push("/eventtypes")}
          className="bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
        >
          Cancel
        </button>
        <button
          type="submit"
          className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        >
          Save
        </button>
      </div>
    </form>
  );
}
