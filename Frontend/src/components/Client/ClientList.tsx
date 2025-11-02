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
      <div style={{
        textAlign: 'center',
        padding: '2rem',
        color: '#6b7280'
      }}>
        No clients found
      </div>
    );
  }

  return (
    <div style={{
      backgroundColor: 'white',
      borderRadius: '8px',
      boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
      overflow: 'hidden'
    }}>
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr style={{ backgroundColor: '#f8fafc' }}>
            <th style={{ padding: '0.75rem 1rem', textAlign: 'left', fontSize: '0.875rem', fontWeight: '600', color: '#374151' }}>Username</th>
            <th style={{ padding: '0.75rem 1rem', textAlign: 'left', fontSize: '0.875rem', fontWeight: '600', color: '#374151' }}>Actions</th>
          </tr>
        </thead>
        <tbody>
          {clients.map((client, index) => (
            <tr key={client.id} style={{ borderBottom: index < clients.length - 1 ? '1px solid #e5e7eb' : 'none' }}>
              <td style={{ padding: '0.75rem 1rem', fontSize: '0.875rem' }}>{client.username}</td>
              <td style={{ padding: '0.75rem 1rem', fontSize: '0.875rem' }}>
                <div style={{ display: 'flex', gap: '0.5rem', flexWrap: 'wrap' }}>
                  <button
                    onClick={() => onEdit(client)}
                    style={{
                      padding: '0.25rem 0.5rem',
                      backgroundColor: '#3b82f6',
                      color: 'white',
                      border: 'none',
                      borderRadius: '4px',
                      cursor: 'pointer',
                      fontSize: '0.75rem'
                    }}
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => onChangePassword(client)}
                    style={{
                      padding: '0.25rem 0.5rem',
                      backgroundColor: '#10b981',
                      color: 'white',
                      border: 'none',
                      borderRadius: '4px',
                      cursor: 'pointer',
                      fontSize: '0.75rem'
                    }}
                  >
                    Change Password
                  </button>
                  <button
                    onClick={() => onChangeRole(client)}
                    style={{
                      padding: '0.25rem 0.5rem',
                      backgroundColor: '#8b5cf6',
                      color: 'white',
                      border: 'none',
                      borderRadius: '4px',
                      cursor: 'pointer',
                      fontSize: '0.75rem'
                    }}
                  >
                    Change Role
                  </button>
                  <button
                    onClick={() => onDelete(client.id)}
                    style={{
                      padding: '0.25rem 0.5rem',
                      backgroundColor: '#ef4444',
                      color: 'white',
                      border: 'none',
                      borderRadius: '4px',
                      cursor: 'pointer',
                      fontSize: '0.75rem'
                    }}
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