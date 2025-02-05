import { cookies } from 'next/headers';
// import { redirect } from 'next/navigation';

export default async function Header() {
  const cookieStore = await cookies();
  const userName = cookieStore.get("userName")?.value;

  // const handleLogout = () => {
  //   cookieStore.delete("access_token");
  //   cookieStore.delete("refresh_token");
  //   cookieStore.delete("id");
  //   cookieStore.delete("userName");
  //   cookieStore.delete("email");
  //   // redirect("/login");
  // };

  return (
    <header className="bg-white shadow">
      <div className="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8">
        <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
        <p className="text-sm text-gray-500">
          Welcome back, {userName || "User"}
        </p>
      </div>
      {userName && (
          <button
            // onClick={handleLogout}
            className="bg-red-500 text-white p-2 rounded"
          >
            Logout
          </button>
        )}
    </header>
  );
}