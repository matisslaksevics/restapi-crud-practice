import { useState, useEffect } from 'react';
import ClientList from '../components/Client/ClientList';
import ClientForm from '../components/Client/ClientForm';
import ClientHeader from '../components/Client/ClientHeader';
import ChangePasswordModel from '../components/Client/ChangePasswordModel';
import ChangeRoleModel from '../components/Client/ChangeRoleModel';
import LoadingSpinner from '../components/Common/LoadingSpinner';
import ErrorMessage from '../components/Common/ErrorMessage';
import Unauthorized from './Unauthorized';
import { useAuth } from '../context/AuthContext';
import type { ClientSummaryDto, UpdateClientDto } from '../types/index.ts';
import { clientService } from '../services/ClientService';

const ClientManagement = () => {
  const { user } = useAuth();
  const [clients, setClients] = useState<ClientSummaryDto[]>([]);
  const [editingClient, setEditingClient] = useState<ClientSummaryDto | null>(null);
  const [selectedClient, setSelectedClient] = useState<ClientSummaryDto | null>(null);
  const [showClientForm, setShowClientForm] = useState(false);
  const [showPasswordForm, setShowPasswordForm] = useState(false);
  const [showRoleForm, setShowRoleForm] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  if (!user || user.role !== 'Admin') {
    return <Unauthorized />;
  }

  useEffect(() => {
    loadClients();
  }, []);

  const loadClients = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const clientsData = await clientService.getAllClients();
      setClients(clientsData);
    } catch (error) {
      console.error('Failed to load clients:', error);
      setError('Failed to load clients');
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
      await clientService.deleteClient(id);
      setClients(prev => prev.filter(c => c.id !== id));
      alert('Client deleted successfully');
    } catch (error: any) {
      console.error('Failed to delete client:', error);
      alert(`Failed to delete client: ${error.response?.data?.message || error.message}`);
    }
  };

  const handleAdminChangePassword = async (userId: string, newPassword: string) => {
    try {
      setIsLoading(true);
      await clientService.adminChangePassword(userId, newPassword);
      setShowPasswordForm(false);
      setSelectedClient(null);
      alert('Password changed successfully');
    } catch (error) {
      console.error('Failed to change password:', error);
      alert('Failed to change password');
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  const handleChangeRole = async (userId: string, newRole: string) => {
    try {
      setIsLoading(true);
      await clientService.changeUserRole(userId, newRole);
      await loadClients();
      setShowRoleForm(false);
      setSelectedClient(null);
      alert('Role changed successfully');
    } catch (error) {
      console.error('Failed to change role:', error);
      alert('Failed to change role');
      throw error;
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
  };

  return (
    <div>
      <ClientHeader totalClients={clients.length} />

      {error && (
        <ErrorMessage message={error} onRetry={loadClients} />
      )}

      {showClientForm && editingClient && (
        <ClientForm
          client={editingClient}
          onSubmit={handleUpdateClient}
          onCancel={handleCancelForm}
          isLoading={isLoading}
        />
      )}

      {showPasswordForm && selectedClient && (
        <ChangePasswordModel
          client={selectedClient}
          onSubmit={handleAdminChangePassword}
          onCancel={handleCancelForm}
          isLoading={isLoading}
        />
      )}

      {showRoleForm && selectedClient && (
        <ChangeRoleModel
          client={selectedClient}
          onSubmit={handleChangeRole}
          onCancel={handleCancelForm}
          isLoading={isLoading}
        />
      )}

      {isLoading && !showClientForm && !showPasswordForm && !showRoleForm ? (
        <LoadingSpinner message="Loading clients..." />
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
};

export default ClientManagement;