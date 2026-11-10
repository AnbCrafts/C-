// Auth & Token Manager
const AuthManager = {
    getAccessToken() {
        return localStorage.getItem('access_token');
    },
    getRefreshToken() {
        return localStorage.getItem('refresh_token');
    },
    getUser() {
        const u = localStorage.getItem('auth_user');
        return u ? JSON.parse(u) : null;
    },
    setTokens(data) {
        if (data.accessToken) localStorage.setItem('access_token', data.accessToken);
        if (data.refreshToken) localStorage.setItem('refresh_token', data.refreshToken);
        if (data.user) localStorage.setItem('auth_user', JSON.stringify(data.user));
        this.updateNav();
    },
    clear() {
        localStorage.removeItem('access_token');
        localStorage.removeItem('refresh_token');
        localStorage.removeItem('auth_user');
        this.updateNav();
    },
    isAuthenticated() {
        return !!this.getAccessToken();
    },
    updateNav() {
        const nav = document.getElementById('authNav');
        if (!nav) return;
        const user = this.getUser();
        if (user && this.isAuthenticated()) {
            nav.innerHTML = `
                <span class="small me-2">Logged in as: <b>${user.userName}</b></span>
                <button class="btn btn-sm btn-outline-danger" onclick="AuthManager.logout()">Logout</button>
            `;
        } else {
            nav.innerHTML = `<a href="/Account/Login" class="btn btn-sm btn-outline-primary">Login</a>`;
        }
    },
    async logout() {
        const refreshToken = this.getRefreshToken();
        if (refreshToken) {
            try {
                await fetch('/api/auth/revoke-token', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ refreshToken })
                });
            } catch (e) {
                console.error(e);
            }
        }
        this.clear();
        window.location.href = '/Account/Login';
    }
};

// Automatic Token Refresh Wrapper for API Calls
async function fetchWithAuth(url, options = {}) {
    options.headers = options.headers || {};
    let accessToken = AuthManager.getAccessToken();
    if (accessToken) {
        options.headers['Authorization'] = `Bearer ${accessToken}`;
    }

    let response = await fetch(url, options);

    // If 401 Unauthorized, automatically attempt token refresh
    if (response.status === 401) {
        const refreshToken = AuthManager.getRefreshToken();
        if (refreshToken && accessToken) {
            const refreshSuccess = await performTokenRefresh(accessToken, refreshToken);
            if (refreshSuccess) {
                // Retry original request with newly issued access token
                accessToken = AuthManager.getAccessToken();
                options.headers['Authorization'] = `Bearer ${accessToken}`;
                response = await fetch(url, options);
            } else {
                AuthManager.clear();
                window.location.href = '/Account/Login';
            }
        } else {
            AuthManager.clear();
            window.location.href = '/Account/Login';
        }
    }

    return response;
}

// Calls /api/auth/refresh-token
async function performTokenRefresh(oldAccessToken, oldRefreshToken) {
    try {
        const res = await fetch('/api/auth/refresh-token', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                accessToken: oldAccessToken,
                refreshToken: oldRefreshToken
            })
        });

        if (res.ok) {
            const data = await res.json();
            if (data.success) {
                AuthManager.setTokens(data);
                return true;
            }
        }
        return false;
    } catch (err) {
        console.error("Token refresh failed:", err);
        return false;
    }
}

document.addEventListener('DOMContentLoaded', () => {
    AuthManager.updateNav();
});
