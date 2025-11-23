interface ClientHeaderProps {
  totalClients: number;
  onAddClient?: () => void;
  isAdmin?: boolean;
}

const ClientHeader = ({ totalClients, onAddClient, isAdmin }: ClientHeaderProps) => {
  return (
    <div className="flex justify-between items-center mb-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Client Management</h1>
        <p className="text-gray-600">{totalClients} clients registered</p>
      </div>
      {isAdmin && onAddClient && (
        <button
          onClick={onAddClient}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors"
        >
          Add New Client
        </button>
      )}
    </div>
  );
};

export default ClientHeader;