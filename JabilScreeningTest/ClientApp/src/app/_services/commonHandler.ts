import { Injectable } from "@angular/core";
import { throwError } from "rxjs";

@Injectable({ providedIn: 'root' })
export class CommonHandler {
  errorHandler(error) {
    let errorMessage = 'An error occurred while processing your request';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else if(error.status !== undefined || error.message !== undefined) {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    } else if (error) {
      errorMessage = error;
    }
    return throwError(errorMessage);
  }
}
