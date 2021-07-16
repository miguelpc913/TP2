import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;


  constructor(private http: HttpClient,@Inject('BASE_URL') private baseUrl:string) { }

  public login(userName: string, password: string) {
    
    const UsuarioApi: User= {
      username:userName,
      password:password,
      token: "" 
    };
    console.log(UsuarioApi)
    // return this.http.post<any>(environment.apiUrlAuth, UsuarioApi)
    //     .pipe(
    //       map(respuesta => {
    //           // store user details and jwt token in local storage to keep user logged in between page refreshes
    //           console.log("Respuesta api: ",respuesta);
    //           localStorage.setItem('UsuarioGuardado', JSON.stringify(respuesta));  
    //           return respuesta;
    //       })
          
    //     ); 
}

  public logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
  }

  public isLoggedIn(){
    let loggedIn : boolean = false;

    var user = JSON.parse(localStorage.getItem('user'));
    
    if (user){
      const token = user["token"];
      console.log("ESTA LOGUEADO: ", token )
      loggedIn=true;
    }
    return loggedIn;
  }
}
