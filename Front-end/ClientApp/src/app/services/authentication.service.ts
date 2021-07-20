import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subscription  } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  
  isLoggedIn : boolean = false;
  error;

  constructor(private http: HttpClient , private router : Router ) {
    this.authenticate();
  }

  
  public authenticate () {
    const token = JSON.parse(localStorage.getItem('authToken'));
    if(token !== null){
      this.isLoggedIn = true;
      this.validateToken(token) 
    }else{
      this.isLoggedIn = false;
    } 
  }

  public login(userName: string, password: string) : Subscription {
    const headers = {'Content-type' : 'application/json' }
    const user: User= {
      username:userName,
      password:password,
      token:""
    };
    return this.http.post<any>(environment.apiUrlLogin + "authenticate", user, { headers })
    .subscribe(
      (response) => {
        localStorage.setItem('authToken', JSON.stringify(response));
        this.isLoggedIn = true;
      },
      e => {
        this.logout()
        this.error = e;
        }
      )
      
}

  public logout() {
    // remove user from local storage to log user out
    this.isLoggedIn = false;
    localStorage.removeItem('authToken');
  }

  public validateToken(token : string){
    if (token){
      this.http.get<any> (environment.apiUrlLogin + "ValidateToken" , ).subscribe(
        (response) =>{
          this.isLoggedIn = response;
          if(!response) this.logout();
        },
        (error) =>{
          this.logout()
        }
      )
    }
  }

  public validateTokenForGuard(){
    const token = JSON.parse(localStorage.getItem('authToken'));
    if (token){
      return this.http.get<any> (environment.apiUrlLogin + "ValidateToken" , );
    }
  }
}
