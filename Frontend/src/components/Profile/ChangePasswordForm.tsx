import { useState } from 'react';
import { clientService } from '../../services/ClientService';

interface ChangePasswordFormProps {
  onSuccess?: () => void;
}

const ChangePasswordForm = ({ onSuccess }: ChangePasswordFormProps) => {
  const [isLoading, setIsLoading] = useState(false);
  const [passwordForm, setPasswordForm] = useState({ 
    currentPassword: '', 
    newPassword: '' 
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      setIsLoading(true);
      await clientService.changePassword(passwordForm.currentPassword, passwordForm.newPassword);
      setPasswordForm({ currentPassword: '', newPassword: '' });
      alert('Password changed successfully');
      onSuccess?.();
    } catch (error) {
      console.error('Failed to change password:', error);
      alert('Failed to change password');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="mt-8 pt-6 border-t border-gray-200">
      <h4 className="text-lg font-bold mb-4">
        Change Password
      </h4>
      <form onSubmit={handleSubmit}>
        <div className="space-y-4 max-w-md">
          <div>
            <label className="block mb-2 font-medium">
              Current Password
            </label>
            <input
              type="password"
              value={passwordForm.currentPassword}
              onChange={(e) => setPasswordForm(prev => ({ ...prev, currentPassword: e.target.value }))}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              required
            />
          </div>
          <div>
            <label className="block mb-2 font-medium">
              New Password
            </label>
            <input
              type="password"
              value={passwordForm.newPassword}
              onChange={(e) => setPasswordForm(prev => ({ ...prev, newPassword: e.target.value }))}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              required
              minLength={6}
            />
          </div>
          <button
            type="submit"
            disabled={isLoading || !passwordForm.currentPassword || !passwordForm.newPassword}
            className={`px-4 py-2 text-white rounded-md transition-colors ${
              isLoading || !passwordForm.currentPassword || !passwordForm.newPassword
                ? 'bg-gray-400 cursor-not-allowed'
                : 'bg-blue-500 hover:bg-blue-600'
            }`}
          >
            {isLoading ? 'Changing...' : 'Change Password'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default ChangePasswordForm;