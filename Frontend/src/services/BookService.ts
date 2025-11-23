import api from './api';
import type { BookSummaryDto, CreateBookDto, UpdateBookDto, BookDetailsDto } from '../types/index.ts';

export const bookService = {
  async getAllBooks(): Promise<BookSummaryDto[]> {
    const response = await api.get<BookSummaryDto[]>('/book');
    return response.data;
  },

  async getBookById(id: number): Promise<BookDetailsDto> {
    const response = await api.get<BookDetailsDto>(`/book/${id}`);
    return response.data;
  },

  async createBook(bookData: CreateBookDto): Promise<BookSummaryDto> {
    const response = await api.post<BookSummaryDto>('/book/admin/new-book', bookData);
    return response.data;
  },

  async updateBook(id: number, bookData: UpdateBookDto): Promise<BookSummaryDto> {
    const response = await api.put<BookSummaryDto>(`/book/admin/update-book/${id}`, bookData);
    return response.data;
  },

  async deleteBook(id: number): Promise<void> {
    await api.delete(`/book/admin/delete-book/${id}`);
  },
};