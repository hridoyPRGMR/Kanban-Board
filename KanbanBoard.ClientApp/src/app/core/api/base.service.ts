// base.service.ts
import { HttpClient } from "@angular/common/http";
import { inject } from "@angular/core";
import { Observable } from "rxjs";

export abstract class BaseService<TResponse, TRequest> {
  
  protected http = inject(HttpClient);
  protected abstract readonly endpoint: string;

  // Returns the full Response DTO
  getAll(): Observable<TResponse[]> {
    return this.http.get<TResponse[]>(this.endpoint);
  }

  getById(id: string | number): Observable<TResponse> {
    return this.http.get<TResponse>(`${this.endpoint}/${id}`);
  }

  // Accepts the Request DTO, Returns the Response DTO
  create(payload: TRequest): Observable<TResponse> {
    return this.http.post<TResponse>(this.endpoint, payload);
  }

  // Usually Update uses a Partial of the Request or Response
  update(id: string | number, payload: Partial<TRequest>): Observable<TResponse> {
    return this.http.put<TResponse>(`${this.endpoint}/${id}`, payload);
  }

  delete(id: string | number): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${id}`);
  }
}