"use client";

import Cookies from 'js-cookie';
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
// import { redirect } from 'next/navigation';

export default  function Header() {
  const [userName, setUserName] = useState<string | null>(null);
  const router = useRouter();
  // const userName = cookieStore.get("userName")?.value;

  useEffect(() => {
    const userName = Cookies.get("userName");
    if (userName) {
      setUserName(userName);
    }
  }, []);

  const handleLogout = async () => {
    // Remove cookies
    const response = await fetch('/logout', {
      method: 'POST',
    });

    if (response.ok) {
      router.push("/login");
    } else {
      console.error("Failed to logout");
    }
  };

  return (
    <header className="bg-white shadow">
      <div className="mx-auto py-6 px-4 sm:px-6 lg:px-8 flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
        <div className="flex items-center space-x-4">
          <p className="text-sm text-gray-500">
            Welcome back, {userName || "User"}
          </p>
          {userName && (
            <button
              onClick={handleLogout}
              className="text-sm text-blue-500 hover:underline"
            >
              Logout
            </button>
          )}
        </div>
      </div>
    </header>
  );
}