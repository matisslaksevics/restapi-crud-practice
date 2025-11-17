import { useState, useEffect } from 'react';
import type { ClientSummaryDto, UpdateClientDto } from '../../types/index.ts';

interface ClientFormProps {
  client?: ClientSummaryDto | null;
  onSubmit: (data: UpdateClientDto) => void;
  onCancel: () => void;
  isLoading?: boolean;
}

const ClientForm = ({ client, onSubmit, onCancel, isLoading = false }: ClientFormProps) => {
  const [formData, setFormData] = useState<UpdateClientDto>({
    username: '',
    email: '',
    firstName: '',
    lastName: ''
  });

  useEffect(() => {
    if (client) {
      setFormData({
        username: client.username || '',
        email: client.email || '',
        firstName: client.firstName || '',
        lastName: client.lastName || ''
      });
    }
  }, [client]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const submitData: UpdateClientDto = {};
    Object.entries(formData).forEach(([key, value]) => {
      if (value.trim() !== '') {
        submitData[key as keyof UpdateClientDto] = value;
      }
    });
    
    onSubmit(submitData);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white p-6 rounded-lg shadow-sm mb-8">
      <h3 className="text-xl font-bold mb-4">
        Edit Client
      </h3>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
        <div>
          <label className="block mb-2 font-medium text-sm text-gray-700">
            Username
          </label>
          <input
            type="text"
            name="username"
            value={formData.username}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div>
          <label className="block mb-2 font-medium text-sm text-gray-700">
            Email
          </label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
        <div>
          <label className="block mb-2 font-medium text-sm text-gray-700">
            First Name
          </label>
          <input
            type="text"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div>
          <label className="block mb-2 font-medium text-sm text-gray-700">
            Last Name
          </label>
          <input
            type="text"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
      </div>

      <div className="flex gap-2 justify-end">
        <button
          type="button"
          onClick={onCancel}
          disabled={isLoading}
          className={`px-4 py-2 text-white rounded-md transition-colors ${
            isLoading ? 'bg-gray-400 cursor-not-allowed' : 'bg-gray-500 hover:bg-gray-600'
          }`}
        >
          Cancel
        </button>
        <button
          type="submit"
          disabled={isLoading}
          className={`px-4 py-2 text-white rounded-md transition-colors ${
            isLoading ? 'bg-gray-400 cursor-not-allowed' : 'bg-blue-500 hover:bg-blue-600'
          }`}
        >
          {isLoading ? 'Saving...' : 'Update Client'}
        </button>
      </div>
    </form>
  );
};

export default ClientForm;