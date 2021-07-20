import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  redirectByUnauthorized : boolean = false;
  unauthorizedTitle : String = "Se ha cerrado su sesion";
  unauthorizedMessage : string = "Parece que su sesion ha caducado";
  constructor(private route : ActivatedRoute , private auth : AuthenticationService) { }

  ngOnInit() {
    this.route.queryParams
      .subscribe(params => {
        if(params.hasOwnProperty("cause") && params.cause === "unauthorized"){
          this.redirectByUnauthorized = true;
          this.auth.logout();
        }
      }
    );
  }

  private closeErrorModal(){
    this.redirectByUnauthorized = false;
  }

}
