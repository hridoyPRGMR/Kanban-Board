import { HttpInterceptor, HttpInterceptorFn } from "@angular/common/http";

export const authInterceptors: HttpInterceptorFn = (req, next)=>
{

    const token = localStorage.getItem('accessToken');

    if(token)
    {
        const cloneRequest = req.clone({
            setHeaders:{
                Authorization: `Bearer ${token}`
            }
        });
        return next(cloneRequest);
    }

    return next(req);
}