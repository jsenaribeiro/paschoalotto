import { Observable, throwError, OperatorFunction, catchError } from "rxjs";

export const handleError = <T>(operation = 'operation', results = []) => (error): Observable<T> => {
   console.error(`${operation} failed:`, error);
   return throwError(() => new Error(`${operation} failed: ${error.message}`));
};

export const catchPipe = <T>(method: string, result = []): OperatorFunction<T, T> =>
   catchError(handleError<T>(method, result))