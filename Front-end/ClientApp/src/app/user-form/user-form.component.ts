import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { pipe } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { SignUpService } from '../services/sign-up.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})
export class UserFormComponent implements OnInit {

  @Input () isLogin : boolean;

  loginForm : FormGroup;
  invalid : boolean = false;
  invalidMessage : String;
  used : boolean;
  error : string;
  
  constructor(private fb : FormBuilder, 
              private router : Router , 
              private auth : AuthenticationService ,
              private signUp : SignUpService) {

     }

  ngOnInit() {

    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    })
  }

    // convenience getter for easy access to form fields
    get fc() { return this.loginForm.controls; }

  onSubmit(){
      this.used = true;
      // stop here if form is invalid
      if (this.loginForm.invalid) {
          return;
      }
      let {username , password} = this.loginForm.value;

      if (this.isLogin) {
        this.auth.login(username, password)
          .add(
            pipe( 
              () =>{
                if(this.auth.isLoggedIn){
                  this.router.navigate(["/gastos"])
                } else{
                  this.error = "Usuario o contraseña incorrecta.";
                }
              }
            )
          )
      }else{
        this.signUp.userIsUsed(username , password).
        subscribe(
          (userIsUsed) => {
            if(!userIsUsed){
              this.signUp.encrypt(password).
              subscribe(
                (response) => {
                  this.signUp.createUser(username, response).
                  subscribe(
                    () => this.router.navigate(["/"]),
                    (e) => console.log(e),
                  )
                },
                (e) => console.log(e)
              )
            }else{
              this.error = "Usuario usado";
            }
          }
        )
      }
  }
}