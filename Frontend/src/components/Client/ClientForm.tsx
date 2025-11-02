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
    <form onSubmit={handleSubmit} style={{
      backgroundColor: 'white',
      padding: '1.5rem',
      borderRadius: '8px',
      boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)'
    }}>
      <h3 style={{
        fontSize: '1.25rem',
        fontWeight: 'bold',
        marginBottom: '1rem'
      }}>
        Edit Client
      </h3>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginBottom: '1rem' }}>
        <div>
          <label style={{
            display: 'block',
            marginBottom: '0.5rem',
            fontWeight: '500',
            color: '#374151',
            fontSize: '0.875rem'
          }}>
            Username
          </label>
          <input
            type="text"
            name="username"
            value={formData.username}
            onChange={handleChange}
            style={{
              width: '100%',
              padding: '0.5rem 0.75rem',
              border: '1px solid #d1d5db',
              borderRadius: '4px',
              fontSize: '0.875rem'
            }}
          />
        </div>

        <div>
          <label style={{
            display: 'block',
            marginBottom: '0.5rem',
            fontWeight: '500',
            color: '#374151',
            fontSize: '0.875rem'
          }}>
            Email
          </label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            style={{
              width: '100%',
              padding: '0.5rem 0.75rem',
              border: '1px solid #d1d5db',
              borderRadius: '4px',
              fontSize: '0.875rem'
            }}
          />
        </div>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginBottom: '1.5rem' }}>
        <div>
          <label style={{
            display: 'block',
            marginBottom: '0.5rem',
            fontWeight: '500',
            color: '#374151',
            fontSize: '0.875rem'
          }}>
            First Name
          </label>
          <input
            type="text"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            style={{
              width: '100%',
              padding: '0.5rem 0.75rem',
              border: '1px solid #d1d5db',
              borderRadius: '4px',
              fontSize: '0.875rem'
            }}
          />
        </div>

        <div>
          <label style={{
            display: 'block',
            marginBottom: '0.5rem',
            fontWeight: '500',
            color: '#374151',
            fontSize: '0.875rem'
          }}>
            Last Name
          </label>
          <input
            type="text"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            style={{
              width: '100%',
              padding: '0.5rem 0.75rem',
              border: '1px solid #d1d5db',
              borderRadius: '4px',
              fontSize: '0.875rem'
            }}
          />
        </div>
      </div>

      <div style={{ display: 'flex', gap: '0.5rem', justifyContent: 'flex-end' }}>
        <button
          type="button"
          onClick={onCancel}
          disabled={isLoading}
          style={{
            padding: '0.5rem 1rem',
            backgroundColor: '#6b7280',
            color: 'white',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer',
            opacity: isLoading ? 0.5 : 1
          }}
        >
          Cancel
        </button>
        <button
          type="submit"
          disabled={isLoading}
          style={{
            padding: '0.5rem 1rem',
            backgroundColor: '#3b82f6',
            color: 'white',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer',
            opacity: isLoading ? 0.5 : 1
          }}
        >
          {isLoading ? 'Saving...' : 'Update Client'}
        </button>
      </div>
    </form>
  );
};

export default ClientForm;