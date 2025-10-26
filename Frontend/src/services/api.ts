import axios from 'axios'

const api = axios.create({
  baseURL: '/api',
})

let isRefreshing = false
let failedRequests: any[] = []

const getRefreshToken = () => localStorage.getItem('refreshToken')
const setTokens = (accessToken: string, refreshToken: string) => {
  localStorage.setItem('accessToken', accessToken)
  localStorage.setItem('refreshToken', refreshToken)
}
const clearTokens = () => {
  localStorage.removeItem('accessToken')
  localStorage.removeItem('refreshToken')
}

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config

    if (error.response?.status === 401 && !originalRequest._retry) {
      
      if (isRefreshing) {
        return new Promise((resolve) => {
          failedRequests.push(() => {
            originalRequest.headers.Authorization = `Bearer ${localStorage.getItem('accessToken')}`
            resolve(api(originalRequest))
          })
        })
      }

      originalRequest._retry = true
      isRefreshing = true

      try {
        console.log('Access token expired, attempting refresh...')
        const refreshToken = getRefreshToken()
        
        if (!refreshToken) {
          throw new Error('No refresh token available')
        }

        const response = await api.put('/auth/refresh-token', {
          refreshToken: refreshToken
        })

        const { accessToken, refreshToken: newRefreshToken } = response.data
        
        setTokens(accessToken, newRefreshToken)
        
        originalRequest.headers.Authorization = `Bearer ${accessToken}`
        
        failedRequests.forEach(callback => callback())
        failedRequests = []
        
        return api(originalRequest)
      } catch (refreshError) {
        console.log('Refresh failed, logging out...')
        clearTokens()
        window.location.href = '/login'
        return Promise.reject(refreshError)
      } finally {
        isRefreshing = false
      }
    }

    return Promise.reject(error)
  }
)

export default api