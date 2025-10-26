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
  username: string;
  role: string;
}

export interface TokenResponseDto {
  accessToken: string;
  refreshToken: string;
}