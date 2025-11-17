import api from './api';
import type { ClientSummaryDto, UpdateClientDto, UserProfileDto } from '../types/index.ts';

export const clientService = {
  async getAllClients(): Promise<ClientSummaryDto[]> {
    const response = await api.get<ClientSummaryDto[]>('/client/admin/all-clients');
    return response.data;
  },

  async updateClient(id: string, clientData: UpdateClientDto): Promise<void> {
    await api.put(`/client/admin/edit-client/${id}`, clientData);
  },

  async deleteClient(id: string): Promise<void> {
    await api.delete(`/client/admin/delete-client/${id}`);
  },

  async changePassword(currentPassword: string, newPassword: string): Promise<void> {
    await api.put('/auth/change-password', { currentPassword, newPassword });
  },

  async adminChangePassword(userId: string, newPassword: string): Promise<void> {
    await api.put('/auth/admin/change-password', { id: userId, newPassword });
  },

  async changeUserRole(userId: string, newRole: string): Promise<void> {
    await api.put('/auth/admin/change-role', { id: userId, newRole });
  },

  async getUserProfile(userId?: string): Promise<UserProfileDto> {
    if (userId) {
      const response = await api.get<UserProfileDto>(`/auth/admin/profile/${userId}`);
      return response.data;
    } else {
      const response = await api.get<UserProfileDto>('/auth/profile');
      return response.data;
    }
  },
  
  async checkPassword(): Promise<any> {
    const response = await api.get('/auth/check-password');
    return response.data;
  },
};