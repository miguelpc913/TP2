import { Component, OnInit , Input,  Output , EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { emit } from 'process';
@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
  styleUrls: ['./error-modal.component.css']
})
export class ErrorModalComponent implements OnInit {

  @Input () errorMessage;
  @Input () errorTitle;
  @Input () errorStatus;
  @Output () closeErrorModal = new EventEmitter();
  constructor( private router : Router) { }

  ngOnInit() {
    if(this.errorStatus === 401){
      this.router.navigate(["/"] , { queryParams: { cause : "unauthorized"}})
    }

    if(this.errorStatus === 0){
      this.errorTitle = "No se pudo conectar con el servidor";
      this.errorMessage = "Por favor revise su conexion"
    }
  }

  private closeModal (){
    this.closeErrorModal.emit();
  }
}
