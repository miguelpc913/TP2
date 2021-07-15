import { Component, OnInit , Input,  Output , EventEmitter } from '@angular/core';
import { emit } from 'process';
@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
  styleUrls: ['./error-modal.component.css']
})
export class ErrorModalComponent implements OnInit {

  @Input () error;
  @Output () closeErrorModal = new EventEmitter();
  constructor() { }

  ngOnInit() {
    console.log(this.error)
  }

  private closeModal (){
    this.closeErrorModal.emit();
  }
}
