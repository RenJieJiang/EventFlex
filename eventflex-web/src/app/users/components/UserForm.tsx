"use client";

import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useRouter } from "next/navigation";
import { userSchema } from "@/lib/schemas/user";

export type UserFormData = z.infer<typeof userSchema>;

interface UserFormProps {
  initialData?: UserFormData;
  onSubmit: (data: UserFormData) => void;
}

export default function UserForm({ initialData, onSubmit }: UserFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<UserFormData>({
    resolver: zodResolver(userSchema),
    defaultValues: initialData,
  });
  const router = useRouter();

  const handleFormSubmit = async (data: UserFormData) => {
    await onSubmit(data);
    router.push("/users");
  };

  return (
    <form onSubmit={handleSubmit(handleFormSubmit)} className="space-y-4">
      {initialData?.id && (
        <input type="hidden" {...register("id")} />
      )}
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
        <label className="block text-gray-700">Email</label>
        <input
          type="email"
          {...register("email")}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm"
        />
        {errors.email && (
          <p className="text-red-500 text-sm">{errors.email.message}</p>
        )}
      </div>
      <div>
        <label className="block text-gray-700">Phone Number</label>
        <input
          type="text"
          {...register("phoneNumber")}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm"
        />
        {errors.phoneNumber && (
          <p className="text-red-500 text-sm">{errors.phoneNumber.message}</p>
        )}
      </div>
      <div>
        <label className="block text-gray-700">Tenant ID (Optional)</label>
        <input
          type="text"
          {...register("tenantId")}
          className="mt-1 block w-full border-gray-300 rounded-md shadow-sm"
        />
      </div>
      <div className="flex justify-between">
        <button
          type="button"
          onClick={() => router.push("/users")}
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
