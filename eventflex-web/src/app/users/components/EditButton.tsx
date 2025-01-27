"use client";

import { useRouter } from "next/navigation";

interface EditButtonProps {
  userId: string;
}

export default function EditButton({ userId }: EditButtonProps) {
  const router = useRouter();

  const handleEdit = () => {
    router.push(`/users/edit/${userId}`);
  };

  return (
    <button
      className="text-blue-500 hover:text-blue-700"
      onClick={handleEdit}
    >
      Edit
    </button>
  );
}