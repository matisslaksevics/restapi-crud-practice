import { useState } from 'react';
import type { ClientSummaryDto } from '../../types/index.ts';

interface ChangePasswordModelProps {
  client: ClientSummaryDto;
  onSubmit: (userId: string, newPassword: string) => Promise<void>;
  onCancel: () => void;
  isLoading?: boolean;
}

const ChangePasswordModel = ({ client, onSubmit, onCancel, isLoading = false }: ChangePasswordModelProps) => {
  const [newPassword, setNewPassword] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await onSubmit(client.id, newPassword);
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow-sm mb-8">
      <h4 className="text-lg font-bold mb-4">
        Change Password for {client.username}
      </h4>
      <form onSubmit={handleSubmit}>
        <div className="space-y-4 max-w-md">
          <div>
            <label className="block mb-2 font-medium">
              New Password
            </label>
            <input
              type="password"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-green-500"
              required
              minLength={6}
            />
          </div>
          <div className="flex gap-2">
            <button
              type="submit"
              disabled={isLoading || !newPassword}
              className={`px-4 py-2 text-white rounded-md transition-colors ${
                isLoading || !newPassword
                  ? 'bg-gray-400 cursor-not-allowed'
                  : 'bg-green-500 hover:bg-green-600'
              }`}
            >
              {isLoading ? 'Changing...' : 'Change Password'}
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

export default ChangePasswordModel;