import { fetchUsers } from "./actions";
import DeleteButton from "./components/DeleteButton";
import EditButton from "./components/EditButton";
import AddButton from "./components/AddButton";

export default async function UsersPage() {
  const users = await fetchUsers();

  return (
    <div className="container mx-auto p-4">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold text-gray-500">User List</h1>
        <AddButton />
      </div>
      <table className="min-w-full bg-white text-gray-900">
        <thead>
          <tr>
            <th className="py-2 px-4 border-b text-left">Username</th>
            <th className="py-2 px-4 border-b text-left">Email</th>
            <th className="py-2 px-4 border-b text-left">Phone Number</th>
            <th className="py-2 px-4 border-b text-left">Lockout Enabled</th>
            <th className="py-2 px-4 border-b text-left">Actions</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id}>
              <td className="py-2 px-4 border-b">{user.userName}</td>
              <td className="py-2 px-4 border-b">{user.email}</td>
              <td className="py-2 px-4 border-b">
                {user.phoneNumber || "N/A"}
              </td>
              <td className="py-2 px-4 border-b">
                {user.lockoutEnabled ? "Yes" : "No"}
              </td>
              <td className="py-2 px-4 border-b">
                <EditButton userId={user.id} />
                <DeleteButton userId={user.id} />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}