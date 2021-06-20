import { Component, Input,  Output , EventEmitter ,  OnInit } from '@angular/core';
import { FormBuilder , FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Gasto, GastoData } from '../models/gasto';
import { GastosService } from '../services/gastos.service';
import { Categoria } from "../models/categoria";


@Component({
  selector: 'app-gasto-form',
  templateUrl: './gasto-form.component.html',
  styleUrls: ['./gasto-form.component.css']
})
export class GastoFormComponent implements OnInit {

  @Input () gasto : Gasto;
  @Output () editarGasto = new EventEmitter();

  formGastos : FormGroup;
  categorias : Categoria[];
  invalid : boolean = false;
  nuevaCategoria : boolean = false;
  nuevaCategoriaString : string = "Nueva Categoria";
  nuevaCategoriaObject : Categoria = {Id:  0 , tipo: this.nuevaCategoriaString};
  createGasto : boolean = false;
  datePattern : RegExp = /^([0-9]{2})\/([0-9]{2})\/([0-9]{4})$/;
  loading : boolean = false;

  constructor(private GastoServ : GastosService,
              private fb: FormBuilder,
              private router:Router) { }

  ngOnInit() {
    this.createForm();
  }

  private async createForm (){
    if(typeof this.gasto === "undefined"){
      this.gasto = new Gasto();
      this.createGasto = true;
    }
    this.formGastos = this.fb.group({
      ...this.gasto,
      price: this.gasto.getPriceFull(),
      compartidoEntre : this.gasto.getCompartidoEntre(),
      nuevaCategoria: '',
      categoria: this.gasto.categoria.tipo === "" ? [null] : this.gasto.categoria.tipo,
    });

    this.loading = true;
    this.GastoServ.getCurrentCategorias().subscribe(
      (response) => {
        this.categorias = response;
        this.categorias.push(this.nuevaCategoriaObject);
      },
      error => console.log(error) , 
      () => this.loading = false
    )
  }

  private guardarPago(){
    const gastoData : GastoData  = this.CreateGastoData(Object.assign({},this.formGastos.value));
    this.invalid = !this.checkFormValidity(gastoData);
    if(!this.invalid){
      this.parseDate(gastoData);
      this.loading = false
      if(this.createGasto){
        this.GastoServ.addGasto(gastoData).subscribe( 
          () =>{} ,
          error => console.log(error),
          () =>{
            this.router.navigate(["/"])
            this.loading = false
          })
      }else{
        this.GastoServ.updateGasto(gastoData , this.gasto.id).subscribe( 
          () => {},
          error => console.log(error),
          () => {
            this.editarGasto.emit()
            this.loading = false
          });
      }
    }
  }

  private checkFormValidity(gastoData : GastoData) : boolean{
    const dataToCheck = this.parseDataToCheck(gastoData);
    for (const key in dataToCheck) {
      const element = gastoData[key];
      if (this.checkIfIsString(element , key)) {
        if(!this.isValidString(element , key)) return false
      } else {
        if(!this.isValidNumber(element , key)) return false
      }
    }

    return true;
  }

  private parseDataToCheck(gastoData){
    const dataToCheck = gastoData;
    if(!dataToCheck.compartido) delete dataToCheck.compartidoEntre;
    return dataToCheck;
  }

  private CreateGastoData(gastoData) : GastoData{
    if(this.nuevaCategoria){
      gastoData.categoria = { tipo :gastoData.nuevaCategoria , Id : 0};
    } else{
      gastoData.categoria = { tipo :gastoData.categoria , Id : 0};
    }
    
    delete gastoData.nuevaCategoria;
    delete gastoData.id;
    return gastoData;
  }

  private parseDate(gastoData) {
    gastoData.date = gastoData.date.split("/").reverse().join("-") + "T00:00:00"
  }
  private categoriaChange(){
    this.nuevaCategoria = this.formGastos.value.categoria === this.nuevaCategoriaString ? true : false;
  }

  private checkIfIsString(element: string , key: string) : boolean{
    return (isNaN(parseInt(element)) && typeof element === "string") || key === "date";
  }

  private isValidString(element: string , key: string) : boolean{
    return (key !== "date"  && element.trim().length > 0) || (key === "date" && this.datePattern.test(element));
  }

  private isValidNumber(element :string , key : string){
    let numberElement: number;
    if (key === "compartidoEntre") {
      numberElement = parseInt(element);
    } else {
      numberElement = parseFloat(element);
    }
    
    if (numberElement < 1) return false;

    return true;
  }
}
