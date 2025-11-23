import type { ClientSummaryDto } from '../../types/index.ts';

interface ClientListProps {
  clients: ClientSummaryDto[];
  onEdit: (client: ClientSummaryDto) => void;
  onDelete: (id: string) => void;
  onChangePassword: (client: ClientSummaryDto) => void;
  onChangeRole: (client: ClientSummaryDto) => void;
}

const ClientList = ({ clients, onEdit, onDelete, onChangePassword, onChangeRole }: ClientListProps) => {
  return (
    <div className="bg-white shadow-lg rounded-lg overflow-hidden">
      <table className="min-w-full divide-y divide-gray-200">
        <thead className="bg-gray-50">
          <tr>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Username
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Role
            </th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Actions
            </th>
          </tr>
        </thead>
        <tbody className="bg-white divide-y divide-gray-200">
          {clients.map((client) => (
            <tr key={client.id} className="hover:bg-gray-50">
              <td className="px-6 py-4 whitespace-nowrap">
                <div className="text-sm font-medium text-gray-900">{client.username}</div>
              </td>
              <td className="px-6 py-4 whitespace-nowrap">
                <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${
                  client.role === 'Admin' 
                    ? 'bg-purple-100 text-purple-800' 
                    : 'bg-green-100 text-green-800'
                }`}>
                  {client.role}
                </span>
              </td>
              <td className="px-6 py-4 whitespace-nowrap text-sm font-medium space-x-2">
                <button
                  onClick={() => onEdit(client)}
                  className="text-blue-600 hover:text-blue-900"
                >
                  Edit
                </button>
                <button
                  onClick={() => onChangePassword(client)}
                  className="text-green-600 hover:text-green-900"
                >
                  Change Password
                </button>
                <button
                  onClick={() => onChangeRole(client)}
                  className="text-orange-600 hover:text-orange-900"
                >
                  Change Role
                </button>
                <button
                  onClick={() => onDelete(client.id)}
                  className="text-red-600 hover:text-red-900"
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {clients.length === 0 && (
        <div className="text-center py-8 text-gray-500">
          No clients available
        </div>
      )}
    </div>
  );
};

export default ClientList;