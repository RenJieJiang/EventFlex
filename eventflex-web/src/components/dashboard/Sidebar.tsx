export default function Sidebar() {
  return (
    <div className="w-64 bg-gray-800 flex flex-col">
      <nav className="mt-5 flex-1 overflow-y-auto">
        <div className="px-2">
          <a
            href="/dashboard"
            className="group flex items-center px-2 py-2 text-base font-medium rounded-md text-white bg-gray-900"
          >
            Dashboard
          </a>
          <a
            href="/events"
            className="group flex items-center px-2 py-2 text-base font-medium rounded-md text-gray-300 hover:text-white hover:bg-gray-700"
          >
            Event Types
          </a>
          <a
            href="/users"
            className="group flex items-center px-2 py-2 text-base font-medium rounded-md text-gray-300 hover:text-white hover:bg-gray-700"
          >
            Users
          </a>
        </div>
      </nav>
    </div>
  );
}