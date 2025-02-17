"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import ConfirmDialog from "@/components/ConfirmDialog";
import { deleteEventType } from "../actions";

interface DeleteButtonProps {
  id: string;
}

export default function DeleteButton({ id }: DeleteButtonProps) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [showConfirmDialog, setShowConfirmDialog] = useState(false);
  const router = useRouter();

  const handleDelete = async () => {
    setLoading(true);
    try {
      await deleteEventType(id);
      router.refresh(); // Refresh the current page to fetch the latest data
    } catch (error: unknown) {
      console.error("Failed to delete event type:", error);
      setError("Failed to delete event type");
    } finally {
      setLoading(false);
      setShowConfirmDialog(false);
    }
  };

  return (
    <>
      {error && <div className="text-red-500">{error}</div>}
      <button
        className="text-red-500 hover:text-red-700 ml-4"
        onClick={() => setShowConfirmDialog(true)}
        disabled={loading}
      >
        {loading ? "Deleting..." : "Delete"}
      </button>
      {showConfirmDialog && (
        <ConfirmDialog
          message="Are you sure you want to delete this event type?"
          onConfirm={handleDelete}
          onCancel={() => setShowConfirmDialog(false)}
        />
      )}
    </>
  );
}