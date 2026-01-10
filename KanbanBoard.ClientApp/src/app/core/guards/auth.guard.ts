// src/app/core/guards/auth.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '@core/api/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Since currentUser is a Signal, we call it as a function ()
  if (authService.currentUser()) {
    return true;
  }

  // Redirect to login if not authenticated
  // We can pass the current URL as a 'returnUrl' query parameter
  return router.createUrlTree(['/login'], { 
    queryParams: { returnUrl: state.url } 
  });
};