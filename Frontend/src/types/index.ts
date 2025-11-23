export interface User {
  id: string;
  username: string;
  role: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
}

export interface UserProfileDto {
  id: string;
  username: string;
  role: string;
}

export interface Client {
  id: string;
  username: string;
  email?: string;
  firstName?: string;
  lastName?: string;
  createdAt: string;
  updatedAt: string;
}

export interface ClientSummaryDto {
  id: string;
  username: string;
  email?: string;
  firstName?: string;
  lastName?: string;
  role?: string;
}

export interface UpdateClientDto {
  username?: string;
  email?: string;
  firstName?: string;
  lastName?: string;
}

export interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
}

export interface AdminPasswordChangeDto {
  id: string;
  newPassword: string;
}

export interface ChangeUserRoleDto {
  id: string;
  newRole: string;
}

export interface BookSummaryDto {
  id: number;
  bookName: string;
  releaseDate: string;
}

export interface BookDetailsDto {
  id: number;
  bookName: string;
  releaseDate: string;
}

export interface CreateBookDto {
  bookName: string;
  releaseDate: string;
}

export interface UpdateBookDto {
  bookName: string;
  releaseDate: string;
}