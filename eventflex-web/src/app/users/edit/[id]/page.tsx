"use client";

import { customFetch } from "@/lib/customFetch";
import { useParams } from "next/navigation";
import { useEffect, useState } from "react";
import UserForm, { UserFormData } from "../../components/UserForm";

interface User {
  id: string;
  name: string;
  email: string;
  phoneNumber: string;
  tenantId?: string;
}

export default function EditUserPage() {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const { id } = useParams();

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await customFetch<User>(`${process.env.NEXT_PUBLIC_API_URL}/users/${id}`, {
          method: "GET",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        });

        setUser(response); 
      } catch (error: any) {
        setError("Failed to fetch user with error: " + error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, [id]);

  const handleSubmit = async (data: UserFormData) => {
    try {
      await customFetch<User>(`${process.env.NEXT_PUBLIC_API_URL}/users/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(data),
      });
    } catch (error: any) {
      setError("Failed to update user");
    }
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4 text-gray-700">Edit User</h1>
      <UserForm initialData={user || undefined} onSubmit={handleSubmit} />
    </div>
  );
}