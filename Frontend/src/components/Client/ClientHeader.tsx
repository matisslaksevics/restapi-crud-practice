interface ClientHeaderProps {
  totalClients: number;
}

const ClientHeader = ({ totalClients }: ClientHeaderProps) => {
  return (
    <div className="flex justify-between items-center mb-6">
      <h1 className="text-2xl font-bold">
        Client Management
      </h1>
      <div className="text-sm text-gray-500">
        Total Clients: {totalClients}
      </div>
    </div>
  );
};

export default ClientHeader;