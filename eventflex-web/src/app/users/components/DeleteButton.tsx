"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { deleteUser } from "../actions";
import ConfirmDialog from "@/components/ConfirmDialog";

interface DeleteButtonProps {
  userId: string;
}

export default function DeleteButton({ userId }: DeleteButtonProps) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [showConfirmDialog, setShowConfirmDialog] = useState(false);
  const router = useRouter();

  const handleDelete = async () => {
    setLoading(true);
    try {
      await deleteUser(userId);
      router.refresh(); // Refresh the current page to fetch the latest data
    } catch (error: unknown) {
      console.error("Failed to delete user:", error);
      setError("Failed to delete user");
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
          message="Are you sure you want to delete this user?"
          onConfirm={handleDelete}
          onCancel={() => setShowConfirmDialog(false)}
        />
      )}
    </>
  );
}