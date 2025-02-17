"use client";

import { useRouter } from "next/navigation";

interface EditButtonProps {
  id: string;
}

export default function EditButton({ id }: EditButtonProps) {
  const router = useRouter();

  const handleEdit = () => {
    router.push(`/eventtypes/edit/${id}`);
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