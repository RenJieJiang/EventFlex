import { fetchUsers } from "./actions";
import DeleteButton from "./components/DeleteButton";
import EditButton from "./components/EditButton";

export default async function UsersPage() {
  
  const users = await fetchUsers();

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4 text-gray-500">User List</h1>
      {/* {error && <div className="text-red-500">{error}</div>} */}
      <table className="min-w-full bg-white text-gray-900">
        <thead>
          <tr>
            <th className="py-2 px-4 border-b text-left">Username</th>
            <th className="py-2 px-4 border-b text-left">Email</th>
            <th className="py-2 px-4 border-b text-left">Email Confirmed</th>
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
                {user.emailConfirmed ? "Yes" : "No"}
              </td>
              <td className="py-2 px-4 border-b">
                {user.phoneNumber || "N/A"}
              </td>
              <td className="py-2 px-4 border-b">
                {user.lockoutEnabled ? "Yes" : "No"}
              </td>
              <td className="py-2 px-4 border-b">
                {/* <Link className="text-blue-500 hover:text-blue-700" href={`/users/edit/${user.id}` }>
                  Edit
                </Link> */}
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