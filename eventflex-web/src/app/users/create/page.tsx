"use client";

import UserForm, { UserFormData } from "../components/UserForm";

export default function AddUserPage() {
  const handleSubmit = async (data: UserFormData) => {
    try {
      await fetch("http://localhost:8080/api/users", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
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