import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subscription  } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import bcrypt from 'bcryptjs';

@Injectable({
  providedIn: 'root'
})
export class SignUpService {

  constructor(private http: HttpClient) { }

  public createUser(userName: string, password: string): Observable<any> {
    const headers = { 'Content-type': 'application/json' }
    const user: User = {
      username: userName,
      password: password,
      token: ""
    };
    return this.http.post<any>(environment.apiUrlSignUp, user, { headers })
  }

  public userIsUsed(userName: string , password: string  ): Observable<any> {
    const headers = { 'Content-type': 'application/json' }
    const user: User = {
      username: userName,
      password: password,
      token: ""
    };
    return this.http.post<any>(environment.apiUrlSignUp + "username-used", user, { headers })
  }

  public encrypt ( password : string) : Observable<any> {
    const rounds = 10
    return new Observable(
      observer =>  bcrypt.hash(password, rounds, (err, hash) => {
        if (err) {
          console.error(err)
          return
        }
        observer.next(hash)
      })
    )
  }
}
