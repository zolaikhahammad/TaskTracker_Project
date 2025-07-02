import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../environment';
import * as $ from 'jquery';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private apiUrl = `${environment.apiBaseUrl}${environment.taskEndpoint}`;

  constructor(private http: HttpClient) { }

  approveTaskUsingJQuery(taskId: number, onSuccess: () => void, onError: (message: string) => void): void {
    $.ajax({
      url: `${this.apiUrl}/updateStatus/${taskId}`,
      method: 'PUT',
      contentType: 'application/json',
      data: JSON.stringify({ status: true }),
      success: () => onSuccess(),
      error: (err) => {
        const message = err?.responseJSON?.message || 'Failed to update task.';
        onError(message);
      }
    });
  }

  create(task: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/createTask`, task).pipe(
      catchError(this.handleError)
    );
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/deleteTask/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  getTasks(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  getPagedTasks(pageNumber: number, pageSize: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/getTasks`, {
      params: {
        pageNumber: pageNumber.toString(),
        pageSize: pageSize.toString()
      }
    }).pipe(catchError(this.handleError));
  }

  update(id: number, task: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/updateTask/${id}`, task).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let message = 'Something went wrong. Please try again later.';

    if (error.error?.message || error.error?.detail) {
      message = `Error ${error.status}: ${error.error.message ?? ''} ${error.error.detail ?? ''}`.trim();
    }

    return throwError(() => message);
  }
}
