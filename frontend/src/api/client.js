const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:8088'

let currentToken = null
let onUnauthorized = null

export function setAuthToken(token) {
  currentToken = token
}

export function setUnauthorizedHandler(handler) {
  onUnauthorized = handler
}

async function request(path, { method = 'GET', body, query } = {}) {
  const url = new URL(path, BASE_URL)
  if (query) {
    Object.entries(query).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') url.searchParams.set(key, value)
    })
  }

  const headers = { 'Content-Type': 'application/json' }
  if (currentToken) headers.Authorization = `Bearer ${currentToken}`

  const response = await fetch(url, {
    method,
    headers,
    body: body === undefined ? undefined : JSON.stringify(body),
  })

  if (response.status === 204) return null

  const text = await response.text()
  const data = text ? JSON.parse(text) : null

  if (!response.ok) {
    if (response.status === 401 && onUnauthorized) onUnauthorized()
    const message = data?.title || data?.message || `Error ${response.status}`
    throw new Error(message)
  }

  return data
}

export const api = {
  get: (path, query) => request(path, { method: 'GET', query }),
  post: (path, body) => request(path, { method: 'POST', body }),
  put: (path, body) => request(path, { method: 'PUT', body }),
  patch: (path, body) => request(path, { method: 'PATCH', body }),
}
