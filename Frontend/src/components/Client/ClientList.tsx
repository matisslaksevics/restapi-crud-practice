import type { ClientSummaryDto } from '../../types/index.ts';

interface ClientListProps {
  clients: ClientSummaryDto[];
  onEdit: (client: ClientSummaryDto) => void;
  onDelete: (id: string) => void;
  onChangePassword: (client: ClientSummaryDto) => void;
  onChangeRole: (client: ClientSummaryDto) => void;
}

const ClientList = ({ clients, onEdit, onDelete, onChangePassword, onChangeRole }: ClientListProps) => {
  if (clients.length === 0) {
    return (
      <div className="text-center py-8 text-gray-500">
        No clients found
      </div>
    );
  }

  return (
    <div className="bg-white rounded-lg shadow-sm overflow-hidden">
      <table className="w-full border-collapse">
        <thead>
          <tr className="bg-gray-50">
            <th className="px-4 py-3 text-left text-sm font-semibold text-gray-700">Username</th>
            <th className="px-4 py-3 text-left text-sm font-semibold text-gray-700">Actions</th>
          </tr>
        </thead>
        <tbody>
          {clients.map((client, index) => (
            <tr 
              key={client.id} 
              className={index < clients.length - 1 ? 'border-b border-gray-200' : ''}
            >
              <td className="px-4 py-3 text-sm">{client.username}</td>
              <td className="px-4 py-3 text-sm">
                <div className="flex gap-2 flex-wrap">
                  <button
                    onClick={() => onEdit(client)}
                    className="px-2 py-1 bg-blue-500 text-white text-xs rounded hover:bg-blue-600 transition-colors"
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => onChangePassword(client)}
                    className="px-2 py-1 bg-green-500 text-white text-xs rounded hover:bg-green-600 transition-colors"
                  >
                    Change Password
                  </button>
                  <button
                    onClick={() => onChangeRole(client)}
                    className="px-2 py-1 bg-purple-500 text-white text-xs rounded hover:bg-purple-600 transition-colors"
                  >
                    Change Role
                  </button>
                  <button
                    onClick={() => onDelete(client.id)}
                    className="px-2 py-1 bg-red-500 text-white text-xs rounded hover:bg-red-600 transition-colors"
                  >
                    Delete
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ClientList;