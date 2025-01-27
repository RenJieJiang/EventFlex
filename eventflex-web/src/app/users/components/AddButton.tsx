"use client";

import { useRouter } from "next/navigation";

export default function AddButton() {
  const router = useRouter();

  const handleAddUser = () => {
    router.push("/users/create");
  };

  return (
    <button
      onClick={handleAddUser}
      className="bg-blue-700 hover:bg-blue-900 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
    >
      Add User
    </button>
  );
}