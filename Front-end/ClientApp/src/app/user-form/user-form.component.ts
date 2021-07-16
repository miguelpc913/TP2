import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

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
  
  constructor(private fb : FormBuilder, 
              private router : Router , 
              private auth : AuthenticationService) {

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
      const {username , password} = this.loginForm.value;
      
      if(this.isLogin) this.auth.login(username , password)
  }
}