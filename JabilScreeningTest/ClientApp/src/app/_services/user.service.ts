import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { User } from '../_models';
import { apiUrl } from '../../apiSettings';
import { catchError, map } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {
  currentUserSubject: any;
  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<User[]>('');
  }

  register(user: User) {
    return this.http.post(apiUrl.baseUrl + apiUrl.registerUrl, user).pipe(map(res => {
      return res;
    }), catchError(error => {
      let errorMsg: string;
      if (error.error instanceof ErrorEvent) {
        errorMsg = `Error: ${error.error.message}`;
      } else {
        errorMsg = this.getServerErrorMessage(error);
      }
      return throwError(errorMsg);
    }));
  }

  delete(id: number) {
    return this.http.delete('');
  }

  private getServerErrorMessage(error: HttpErrorResponse): string {
    switch (error.status) {
      case 404: {
        return `Not Found: ${error}`;
      }
      case 403: {
        return `Access Denied: ${error}`;
      }
      case 500: {
        return `Internal Server Error: ${error}`;
      }
      default: {
        return `Unknown Server Error: ${error}`;
      }
    }
  }
}
