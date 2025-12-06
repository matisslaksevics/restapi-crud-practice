import api from './api';
import type { 
  BorrowSummaryDto, 
  CreateBorrowDto, 
  CreateUserBorrowDto, 
  UpdateBorrowDto 
} from '../types/index.ts';

export const borrowService = {
  async getAllBorrows(): Promise<BorrowSummaryDto[]> {
    const response = await api.get<BorrowSummaryDto[]>('/borrow/admin/all-borrows');
    return response.data;
  },

  async getUserBorrows(userId: string): Promise<BorrowSummaryDto[]> {
    const response = await api.get<BorrowSummaryDto[]>(`/borrow/admin/user-borrows/${userId}`);
    return response.data;
  },

  async adminCreateBorrow(borrowData: CreateBorrowDto): Promise<BorrowSummaryDto> {
    const response = await api.post<BorrowSummaryDto>('/borrow/admin/new-borrow', borrowData);
    return response.data;
  },

  async updateBorrow(id: number, borrowData: UpdateBorrowDto): Promise<void> {
    await api.put(`/borrow/admin/edit-borrow/${id}`, borrowData);
  },

  async deleteBorrow(id: number): Promise<void> {
    await api.delete(`/borrow/admin/delete-borrow/${id}`);
  },

  async getMyBorrows(): Promise<BorrowSummaryDto[]> {
    const response = await api.get<BorrowSummaryDto[]>('/borrow/myborrows');
    return response.data;
  },

  async createBorrow(borrowData: CreateUserBorrowDto): Promise<BorrowSummaryDto> {
    const response = await api.post<BorrowSummaryDto>('/borrow/new-borrow', borrowData);
    return response.data;
  },
};