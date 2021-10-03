import {Injectable} from '@angular/core';
import {ExternalAuthProvider} from "../enums/ExternalAuthProvider";
import {Observable, of} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {catchError, map} from "rxjs/operators";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  constructor(private httpClient: HttpClient, private jwtHelperService: JwtHelperService) {
  }

  externalLogIn(token: string, provider: ExternalAuthProvider): Observable<void> {
    const url = environment.apiUrl + 'auth/external-log-in';

    const model = {
      token,
      provider
    };

    return this.httpClient.post<any>(url, model).pipe(
      map(response => localStorage.setItem('access_token', response.data))
    );
  }

  externalSignUp(token: string, provider: ExternalAuthProvider): Observable<void> {
    const url = environment.apiUrl + 'auth/external-sign-up';

    const model = {
      token,
      provider
    };

    return this.httpClient.post<any>(url, model).pipe(
      map(response => localStorage.setItem('access_token', response.data))
    );
  }

  logOut(): Observable<void> {
    const url = environment.apiUrl + 'auth/log-out';

    return this.httpClient.post<any>(url, null);
  }

  refreshTokens(): Observable<void> {
    const url = environment.apiUrl + 'auth/refresh-tokens';

    return this.httpClient.get<any>(url).pipe(
      map(response => localStorage.setItem('access_token', response.data))
    );
  }

  isLoggedIn(): Observable<boolean> {
    const accessToken = localStorage.getItem('access_token');

    if (!accessToken) {
      return of(false);
    }

    const isTokenExpired = this.jwtHelperService.isTokenExpired(accessToken);

    if (isTokenExpired) {
      return this.refreshTokens().pipe(
        map(() => true),
        catchError(() => of(false))
      )
    }

    return of(true);
  }
}
