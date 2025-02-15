"use client";

import { customFetch } from "@/lib/customFetch";
import UserForm, { UserFormData } from "../components/UserForm";

export default function AddUserPage() {
  const handleSubmit = async (data: UserFormData) => {
    try {
      await customFetch(`${process.env.NEXT_PUBLIC_API_URL}/users`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify(data),
      });
    } catch (error) {
      console.error("Failed to create user:", error);
    }
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4 text-gray-700">Add User</h1>
      <UserForm onSubmit={handleSubmit} />
    </div>
  );
}