// src/app/core/services/auth.service.ts
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { LoginDto, LoginResponseDto, RegisterUserDto, RefreshTokenDto, TokenResponseDto } from '@core/model/identity.dto';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private readonly API_URL = 'http://localhost:5025/api/Auth';

  // Signal to hold user data (null if not logged in)
  currentUser = signal<LoginResponseDto['user'] | null>(this.getInitialUser());

  login(credentials: LoginDto) {
    return this.http.post<LoginResponseDto>(`${this.API_URL}/login`, credentials).pipe(
      tap(res => this.setSession(res))
    );
  }

  register(data: RegisterUserDto) {
    return this.http.post(`${this.API_URL}/register`, data).pipe(
      tap((res) =>{
        console.log(res);
      })
    );
  }

  logout() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
    this.currentUser.set(null);

    this.router.navigate(['/login']);
  }

  /**
   * Call the refresh endpoint to obtain a new access token (and optional new refresh token).
   * This updates stored tokens via updateTokensFromRefresh.
   */
  refreshToken(payload: RefreshTokenDto) {
    return this.http.post<TokenResponseDto>(`${this.API_URL}/refresh`, payload).pipe(
      tap(res => this.updateTokensFromRefresh(res))
    );
  }

  private setSession(res: LoginResponseDto) {
    localStorage.setItem('accessToken', res.accessToken);
    // Store refresh token as well so client can call refresh endpoint when needed.
    // Note: storing refresh tokens in localStorage has XSS risks; consider HttpOnly cookie in production.
    if (res.refreshToken) {
      localStorage.setItem('refreshToken', res.refreshToken);
    }
    localStorage.setItem('user', JSON.stringify(res.user));
    this.currentUser.set(res.user);
  }

  private updateTokensFromRefresh(res: TokenResponseDto) {
    if (!res) return;
    if (res.accessToken) {
      localStorage.setItem('accessToken', res.accessToken);
    }
    if (res.refreshToken) {
      localStorage.setItem('refreshToken', res.refreshToken);
    }
  }

  private getInitialUser() {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }
}