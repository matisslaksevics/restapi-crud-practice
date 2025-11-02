import { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import Sidebar from '../components/Layout/Sidebar';
import ClientList from '../components/Client/ClientList';
import ClientForm from '../components/Client/ClientForm';
import type { ClientSummaryDto, UpdateClientDto} from '../types/index.ts';
import { clientService } from '../services/ClientService';

const Dashboard = () => {
  const { user, logout } = useAuth();
  const [activeView, setActiveView] = useState('profile');
  const [clients, setClients] = useState<ClientSummaryDto[]>([]);
  const [editingClient, setEditingClient] = useState<ClientSummaryDto | null>(null);
  const [selectedClient, setSelectedClient] = useState<ClientSummaryDto | null>(null);
  const [showClientForm, setShowClientForm] = useState(false);
  const [showPasswordForm, setShowPasswordForm] = useState(false);
  const [showRoleForm, setShowRoleForm] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [passwordForm, setPasswordForm] = useState({ currentPassword: '', newPassword: '' });
  const [adminPasswordForm, setAdminPasswordForm] = useState({ newPassword: '' });
  const [roleForm, setRoleForm] = useState({ newRole: 'User' });

  useEffect(() => {
    if (activeView === 'admin-clients') {
      loadClients();
    }
  }, [activeView]);

  const loadClients = async () => {
    try {
      setIsLoading(true);
      const clientsData = await clientService.getAllClients();
      setClients(clientsData);
    } catch (error) {
      console.error('Failed to load clients:', error);
      alert('Failed to load clients');
    } finally {
      setIsLoading(false);
    }
  };

  const handleUpdateClient = async (clientData: UpdateClientDto) => {
    if (!editingClient) return;
    
    try {
      setIsLoading(true);
      await clientService.updateClient(editingClient.id, clientData);
      await loadClients(); 
      setEditingClient(null);
      setShowClientForm(false);
      alert('Client updated successfully');
    } catch (error) {
      console.error('Failed to update client:', error);
      alert('Failed to update client');
    } finally {
      setIsLoading(false);
    }
  };

  const handleDeleteClient = async (id: string) => {
    if (!confirm('Are you sure you want to delete this client?')) return;
    
    try {
    console.log('Attempting to delete client with ID:', id);
    await clientService.deleteClient(id);
    setClients(prev => prev.filter(c => c.id !== id));
    alert('Client deleted successfully');
  } catch (error: any) {
    console.error('Failed to delete client:', error);
    console.error('Error details:', error.response?.data);
    alert(`Failed to delete client: ${error.response?.data?.message || error.message}`);
  }
  };

  const handleChangePassword = async () => {
    try {
      setIsLoading(true);
      await clientService.changePassword(passwordForm.currentPassword, passwordForm.newPassword);
      setShowPasswordForm(false);
      setPasswordForm({ currentPassword: '', newPassword: '' });
      alert('Password changed successfully');
    } catch (error) {
      console.error('Failed to change password:', error);
      alert('Failed to change password');
    } finally {
      setIsLoading(false);
    }
  };

  const handleAdminChangePassword = async () => {
    if (!selectedClient) return;
    
    try {
      setIsLoading(true);
      await clientService.adminChangePassword(selectedClient.id, adminPasswordForm.newPassword);
      setShowPasswordForm(false);
      setSelectedClient(null);
      setAdminPasswordForm({ newPassword: '' });
      alert('Password changed successfully');
    } catch (error) {
      console.error('Failed to change password:', error);
      alert('Failed to change password');
    } finally {
      setIsLoading(false);
    }
  };

  const handleChangeRole = async () => {
    if (!selectedClient) return;
    
    try {
      setIsLoading(true);
      await clientService.changeUserRole(selectedClient.id, roleForm.newRole);
      await loadClients();
      setShowRoleForm(false);
      setSelectedClient(null);
      setRoleForm({ newRole: 'User' });
      alert('Role changed successfully');
    } catch (error) {
      console.error('Failed to change role:', error);
      alert('Failed to change role');
    } finally {
      setIsLoading(false);
    }
  };

  const handleEditClient = (client: ClientSummaryDto) => {
    setEditingClient(client);
    setShowClientForm(true);
  };

  const handleChangePasswordClick = (client: ClientSummaryDto) => {
    setSelectedClient(client);
    setShowPasswordForm(true);
  };

  const handleChangeRoleClick = (client: ClientSummaryDto) => {
    setSelectedClient(client);
    setShowRoleForm(true);
  };

  const handleCancelForm = () => {
    setShowClientForm(false);
    setShowPasswordForm(false);
    setShowRoleForm(false);
    setEditingClient(null);
    setSelectedClient(null);
    setPasswordForm({ currentPassword: '', newPassword: '' });
    setAdminPasswordForm({ newPassword: '' });
    setRoleForm({ newRole: 'User' });
  };
  const renderActiveView = () => {
    switch (activeView) {
      case 'profile':
        return (
          <div style={{
            backgroundColor: 'white',
            padding: '1.5rem',
            borderRadius: '8px',
            boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)'
          }}>
            <h3 style={{ fontSize: '1.25rem', fontWeight: 'bold', marginBottom: '1rem' }}>
              User Profile
            </h3>
            <div style={{ display: 'grid', gap: '0.5rem' }}>
              <p><strong>Username:</strong> {user?.username}</p>
              <p><strong>Role:</strong> {user?.role}</p>
              <p><strong>User ID:</strong> {user?.id}</p>
            </div>
            <div style={{ marginTop: '2rem', paddingTop: '1.5rem', borderTop: '1px solid #e5e7eb' }}>
              <h4 style={{ fontSize: '1.125rem', fontWeight: 'bold', marginBottom: '1rem' }}>
                Change Password
              </h4>
              <div style={{ display: 'grid', gap: '1rem', maxWidth: '400px' }}>
                <div>
                  <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: '500' }}>
                    Current Password
                  </label>
                  <input
                    type="password"
                    value={passwordForm.currentPassword}
                    onChange={(e) => setPasswordForm(prev => ({ ...prev, currentPassword: e.target.value }))}
                    style={{
                      width: '100%',
                      padding: '0.5rem 0.75rem',
                      border: '1px solid #d1d5db',
                      borderRadius: '4px'
                    }}
                  />
                </div>
                <div>
                  <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: '500' }}>
                    New Password
                  </label>
                  <input
                    type="password"
                    value={passwordForm.newPassword}
                    onChange={(e) => setPasswordForm(prev => ({ ...prev, newPassword: e.target.value }))}
                    style={{
                      width: '100%',
                      padding: '0.5rem 0.75rem',
                      border: '1px solid #d1d5db',
                      borderRadius: '4px'
                    }}
                  />
                </div>
                <button
                  onClick={handleChangePassword}
                  disabled={isLoading || !passwordForm.currentPassword || !passwordForm.newPassword}
                  style={{
                    padding: '0.5rem 1rem',
                    backgroundColor: '#3b82f6',
                    color: 'white',
                    border: 'none',
                    borderRadius: '4px',
                    cursor: 'pointer',
                    opacity: (isLoading || !passwordForm.currentPassword || !passwordForm.newPassword) ? 0.5 : 1
                  }}
                >
                  {isLoading ? 'Changing...' : 'Change Password'}
                </button>
              </div>
            </div>
          </div>
        );

      case 'admin-clients':
        return (
          <div>
            <div style={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              marginBottom: '1.5rem'
            }}>
              <h3 style={{ fontSize: '1.25rem', fontWeight: 'bold' }}>
                Client Management
              </h3>
              <div style={{ fontSize: '0.875rem', color: '#6b7280' }}>
                Total Clients: {clients.length}
              </div>
            </div>

            {showClientForm && (
              <div style={{ marginBottom: '2rem' }}>
                <ClientForm
                  client={editingClient}
                  onSubmit={handleUpdateClient}
                  onCancel={handleCancelForm}
                  isLoading={isLoading}
                />
              </div>
            )}

            {showPasswordForm && selectedClient && (
              <div style={{ marginBottom: '2rem', backgroundColor: 'white', padding: '1.5rem', borderRadius: '8px', boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)' }}>
                <h4 style={{ fontSize: '1.125rem', fontWeight: 'bold', marginBottom: '1rem' }}>
                  Change Password for {selectedClient.username}
                </h4>
                <div style={{ display: 'grid', gap: '1rem', maxWidth: '400px' }}>
                  <div>
                    <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: '500' }}>
                      New Password
                    </label>
                    <input
                      type="password"
                      value={adminPasswordForm.newPassword}
                      onChange={(e) => setAdminPasswordForm({ newPassword: e.target.value })}
                      style={{
                        width: '100%',
                        padding: '0.5rem 0.75rem',
                        border: '1px solid #d1d5db',
                        borderRadius: '4px'
                      }}
                    />
                  </div>
                  <div style={{ display: 'flex', gap: '0.5rem' }}>
                    <button
                      onClick={handleAdminChangePassword}
                      disabled={isLoading || !adminPasswordForm.newPassword}
                      style={{
                        padding: '0.5rem 1rem',
                        backgroundColor: '#10b981',
                        color: 'white',
                        border: 'none',
                        borderRadius: '4px',
                        cursor: 'pointer'
                      }}
                    >
                      {isLoading ? 'Changing...' : 'Change Password'}
                    </button>
                    <button
                      onClick={handleCancelForm}
                      style={{
                        padding: '0.5rem 1rem',
                        backgroundColor: '#6b7280',
                        color: 'white',
                        border: 'none',
                        borderRadius: '4px',
                        cursor: 'pointer'
                      }}
                    >
                      Cancel
                    </button>
                  </div>
                </div>
              </div>
            )}

            {showRoleForm && selectedClient && (
              <div style={{ marginBottom: '2rem', backgroundColor: 'white', padding: '1.5rem', borderRadius: '8px', boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)' }}>
                <h4 style={{ fontSize: '1.125rem', fontWeight: 'bold', marginBottom: '1rem' }}>
                  Change Role for {selectedClient.username}
                </h4>
                <div style={{ display: 'grid', gap: '1rem', maxWidth: '400px' }}>
                  <div>
                    <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: '500' }}>
                      New Role
                    </label>
                    <select
                      value={roleForm.newRole}
                      onChange={(e) => setRoleForm({ newRole: e.target.value })}
                      style={{
                        width: '100%',
                        padding: '0.5rem 0.75rem',
                        border: '1px solid #d1d5db',
                        borderRadius: '4px'
                      }}
                    >
                      <option value="User">User</option>
                      <option value="Admin">Admin</option>
                    </select>
                  </div>
                  <div style={{ display: 'flex', gap: '0.5rem' }}>
                    <button
                      onClick={handleChangeRole}
                      disabled={isLoading}
                      style={{
                        padding: '0.5rem 1rem',
                        backgroundColor: '#8b5cf6',
                        color: 'white',
                        border: 'none',
                        borderRadius: '4px',
                        cursor: 'pointer'
                      }}
                    >
                      {isLoading ? 'Changing...' : 'Change Role'}
                    </button>
                    <button
                      onClick={handleCancelForm}
                      style={{
                        padding: '0.5rem 1rem',
                        backgroundColor: '#6b7280',
                        color: 'white',
                        border: 'none',
                        borderRadius: '4px',
                        cursor: 'pointer'
                      }}
                    >
                      Cancel
                    </button>
                  </div>
                </div>
              </div>
            )}

            {isLoading ? (
              <div style={{ textAlign: 'center', padding: '2rem' }}>Loading clients...</div>
            ) : (
              <ClientList
                clients={clients}
                onEdit={handleEditClient}
                onDelete={handleDeleteClient}
                onChangePassword={handleChangePasswordClick}
                onChangeRole={handleChangeRoleClick}
              />
            )}
          </div>
        );

      default:
        return <div>Select a view from the sidebar</div>;
    }
  };

  return (
    <div style={{ minHeight: '100vh', backgroundColor: '#f8fafc' }}>
      {/* Header */}
      <nav style={{
        backgroundColor: 'white',
        padding: '1rem 0',
        boxShadow: '0 1px 3px rgba(0, 0, 0, 0.1)',
        height: '80px'
      }}>
        <div style={{
          maxWidth: '1200px',
          margin: '0 auto',
          padding: '0 1rem',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center'
        }}>
          <h1 style={{ fontSize: '1.25rem', fontWeight: 'bold' }}>
            Client Management System
          </h1>
          <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
            <span>Hello, {user?.username} ({user?.role})</span>
            <button 
              onClick={logout}
              style={{
                backgroundColor: '#ef4444',
                color: 'white',
                padding: '0.5rem 1rem',
                border: 'none',
                borderRadius: '4px',
                cursor: 'pointer'
              }}
            >
              Logout
            </button>
          </div>
        </div>
      </nav>

      {/* Main Content */}
      <div style={{
        display: 'flex',
        maxWidth: '1200px',
        margin: '0 auto',
        padding: '0 1rem'
      }}>
        {/* Sidebar */}
        <Sidebar activeView={activeView} onViewChange={setActiveView} user={user} />

        {/* Content Area */}
        <main style={{
          flex: 1,
          padding: '2rem',
          minHeight: 'calc(100vh - 80px)'
        }}>
          {renderActiveView()}
        </main>
      </div>
    </div>
  );
};

export default Dashboard;