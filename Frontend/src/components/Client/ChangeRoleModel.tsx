import { useState } from 'react';
import type { ClientSummaryDto } from '../../types/index.ts';

interface ChangeRoleModelProps {
  client: ClientSummaryDto;
  onSubmit: (userId: string, newRole: string) => Promise<void>;
  onCancel: () => void;
  isLoading?: boolean;
}

const ChangeRoleModel = ({ client, onSubmit, onCancel, isLoading = false }: ChangeRoleModelProps) => {
  const [newRole, setNewRole] = useState(client.role || 'User');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await onSubmit(client.id, newRole);
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow-sm mb-8">
      <h4 className="text-lg font-bold mb-4">
        Change Role for {client.username}
      </h4>
      <form onSubmit={handleSubmit}>
        <div className="space-y-4 max-w-md">
          <div>
            <label className="block mb-2 font-medium">
              New Role
            </label>
            <select
              value={newRole}
              onChange={(e) => setNewRole(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-purple-500"
            >
              <option value="User">User</option>
              <option value="Admin">Admin</option>
            </select>
          </div>
          <div className="flex gap-2">
            <button
              type="submit"
              disabled={isLoading}
              className={`px-4 py-2 text-white rounded-md transition-colors ${
                isLoading
                  ? 'bg-gray-400 cursor-not-allowed'
                  : 'bg-purple-500 hover:bg-purple-600'
              }`}
            >
              {isLoading ? 'Changing...' : 'Change Role'}
            </button>
            <button
              type="button"
              onClick={onCancel}
              className="px-4 py-2 bg-gray-500 text-white rounded-md hover:bg-gray-600 transition-colors"
            >
              Cancel
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default ChangeRoleModel;