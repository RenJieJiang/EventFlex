import DeleteButton from "./components/DeleteButton";
import EditButton from "./components/EditButton";
import AddButton from "./components/AddButton";
import { fetchEventTypes } from "./actions";

export default async function EventTypesPage() {
  const eventTypes = await fetchEventTypes();

  return (
    <div className="container mx-auto p-4">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold text-gray-500">Event Type List</h1>
        <AddButton />
      </div>
      <table className="min-w-full bg-white text-gray-900">
        <thead>
          <tr>
            <th className="py-2 px-4 border-b text-left">Event Type</th>
            <th className="py-2 px-4 border-b text-left">Description</th>
          </tr>
        </thead>
        <tbody>
          {eventTypes.map((eventType) => (
            <tr key={eventType.id}>
              <td className="py-2 px-4 border-b">{eventType.name}</td>
              <td className="py-2 px-4 border-b">{eventType.description}</td>
              <td className="py-2 px-4 border-b">
                <EditButton id={eventType.id} />
                <DeleteButton id={eventType.id} />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}