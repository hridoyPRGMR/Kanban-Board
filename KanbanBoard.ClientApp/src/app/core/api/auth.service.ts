// src/app/core/services/auth.service.ts
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { LoginDto, LoginResponseDto, RegisterUserDto } from '@core/model/identity.dto';
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
    localStorage.removeItem('user');
    this.currentUser.set(null);

    this.router.navigate(['/login']);
  }

  private setSession(res: LoginResponseDto) {
    localStorage.setItem('accessToken', res.accessToken);
    localStorage.setItem('user', JSON.stringify(res.user));
    this.currentUser.set(res.user);
  }

  private getInitialUser() {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }
}