import { HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { BehaviorSubject, throwError } from "rxjs";
import { catchError, switchMap, filter, take } from "rxjs/operators";
import { AuthService } from "@core/api/auth.service";

// Single shared refresh state so multiple 401s trigger only one refresh request
let refreshInProgress = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptors: HttpInterceptorFn = (req, next) => {
    const accessToken = localStorage.getItem('accessToken');
    const authService = inject(AuthService);

    const addAuthHeader = (token?: string) =>
        token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req;

    const authReq = addAuthHeader(accessToken ?? undefined);

    return next(authReq).pipe(
        catchError((err: any) => {
            // If unauthorized, attempt refresh
            if (err && err.status === 401) {
                const storedRefresh = localStorage.getItem('refreshToken');
                if (!storedRefresh) {
                    // No refresh token available — forward the error
                    return throwError(() => err);
                }

                if (!refreshInProgress) {
                    refreshInProgress = true;
                    // initiate refresh
                    return authService.refreshToken({ refreshToken: storedRefresh }).pipe(
                        switchMap((res) => {
                            // update local storage handled by AuthService
                            const newAccess = res.accessToken;
                            refreshInProgress = false;
                            refreshTokenSubject.next(newAccess ?? null);
                            // retry original request with new token
                            const retryReq = addAuthHeader(newAccess ?? undefined);
                            return next(retryReq);
                        }),
                        catchError((refreshErr) => {
                            refreshInProgress = false;
                            refreshTokenSubject.next(null);
                            // If refresh fails, logout the user or forward the error
                            authService.logout();
                            return throwError(() => refreshErr);
                        })
                    );
                }

                // If a refresh is already in progress, wait for it to complete then retry
                return refreshTokenSubject.pipe(
                    filter(token => token !== null),
                    take(1),
                    switchMap((token) => {
                        const retryReq = addAuthHeader(token ?? undefined);
                        return next(retryReq);
                    })
                );
            }

            return throwError(() => err);
        })
    );
};