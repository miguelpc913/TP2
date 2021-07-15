import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InterceptorService implements HttpInterceptor {

  constructor() { }

  intercept(req: HttpRequest<any> , next: HttpHandler) : Observable<HttpEvent<any>>{
    let token = JSON.parse(localStorage.getItem('token'));
    if(token === ""){
      return next.handle(req);
    }
    const header = req.clone({
      headers: req.headers.set('Authorization' , `Bearer ${token}` )
    })
    return next.handle(header)
  }
}
