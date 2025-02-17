"use client";

import { useRouter } from "next/navigation";

export default function AddButton() {
  const router = useRouter();

  const handleAddEventType = () => {
    router.push("/eventtypes/create");
  };

  return (
    <button
      onClick={handleAddEventType}
      className="bg-blue-700 hover:bg-blue-900 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
    >
      Add Event Type
    </button>
  );
}