import { createContext, useContext, useState, useEffect } from 'react'
import type { User, AuthResponse, UserProfileDto } from '../types'
import api from '../services/api'

interface AuthContextType {
  user: User | null
  login: (username: string, password: string) => Promise<void>
  register: (username: string, password: string) => Promise<void>
  logout: () => void
  isLoading: boolean
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    checkAuth()
  }, [])

  const checkAuth = async () => {
    const accessToken = localStorage.getItem('accessToken')
    
    if (!accessToken) {
      setIsLoading(false)
      return
    }

    try {
      console.log('Checking authentication with existing token...')
      await fetchUser()
    } catch (error) {
      console.log('Auto-login failed, clearing tokens')
      clearTokens()
      setUser(null)
    } finally {
      setIsLoading(false)
    }
  }
  const fetchUser = async () => {
    try {
      console.log('Fetching user profile...')
      const response = await api.get<UserProfileDto>('/auth/profile')
      const userData = response.data;
    
      if (!userData.username) {
        console.error('No username in profile response!')
        throw new Error('Invalid user data received')
      }
    
  
      const userObj: User = {
        id: '',
        username: userData.username,
        role: userData.role
      }
      setUser(userObj)
    } catch (error) {
      console.log('Failed to fetch user profile')
      throw error
    }
  }
  const login = async (username: string, password: string) => {
    try {
      console.log('Attempting login...')
      const response = await api.post<AuthResponse>('/auth/login', { 
        username,
        password 
      })
      
      const { accessToken, refreshToken } = response.data
      
      localStorage.setItem('accessToken', accessToken)
      localStorage.setItem('refreshToken', refreshToken)
      
      await fetchUser()
    } catch (error) {
      clearTokens()
      throw error
    }
  }

  const register = async (username: string, password: string) => {
    try {
      console.log('Attempting registration...')
      const response = await api.post('/auth/register', {
        username,
        password
      })
    
      console.log('Registration successful:', response.data)
    } catch (error) {
      console.log('Registration failed:', error)
      throw error
    }
  }

  const logout = async () => {
    try {
      await api.post('/auth/signout')
    } catch (error) {
      console.error('Logout error:', error)
    } finally {
      clearTokens()
      setUser(null)
    }
  }

  const clearTokens = () => {
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
  }

  useEffect(() => {
    console.log('User state changed:', user)
    console.log('Access token in localStorage:', localStorage.getItem('accessToken'))
  }, [user])

  return (
    <AuthContext.Provider value={{ user, login, register, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  )
}

export const useAuth = () => {
  const context = useContext(AuthContext)
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider')
  }
  return context
}