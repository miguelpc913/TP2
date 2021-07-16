import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InterceptorService implements HttpInterceptor {

  constructor() { }

  intercept(req: HttpRequest<any> , next: HttpHandler) : Observable<HttpEvent<any>>{
    let user = JSON.parse(localStorage.getItem('user'));
    if(!user){
      return next.handle(req);
    }
    const header = req.clone({
      headers: req.headers.set('Authorization' , `Bearer ${user['token']}` )
    })
    return next.handle(header)
  }
}
