import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, retry, throwError } from 'rxjs';
import { SignService } from '../Sign/sign.service';

// @Injectable()
@Injectable({
  providedIn: 'root',
})
export class JwtInterceptorService implements HttpInterceptor {
  constructor(private signService: SignService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = this.signService.GetToken();

    if (token) {
      req = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` },
      });
    }
    // .pipe(
    //   retry(1),
    //   catchError((err: HttpErrorResponse) => {
    //     let errorMessage = '';
    //     if (err.error instanceof ErrorEvent) {
    //       errorMessage = `Error: ${err.error.message}`;
    //     } else {
    //       errorMessage = `Error Status: ${err.status}\nMessage: ${err.message}`;
    //     }
    //     alert(errorMessage);
    //     return throwError(errorMessage);
    //   })
    // )

    return next.handle(req);
  }
}
