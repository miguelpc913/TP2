import { Component, OnInit } from '@angular/core';
import {GastosService} from '../services/gastos.service'
import {Gasto} from '../models/gasto'
import { Categoria } from '../models/categoria';

@Component({
  selector: 'app-gastos',
  templateUrl: './gastos.component.html',
  styleUrls: ['./gastos.component.css']
})
export class GastosComponent implements OnInit {
  private gastos : Gasto[] = [];
  private deudaTotal : number;
  private gastoTotal : number;
  private busquedaDescripcion : string;
  private busquedaPorCategoriaDefault : string = "No filtrar por categoria";
  private filtrado : boolean = false;
  private busquedaCategoria : string;
  private categorias : Categoria[];
  private loadingGastos : boolean = false;
  private loadingCategorias : boolean = false;
  constructor(public gastosService : GastosService) { }

  ngOnInit() {
    this.initState();
  }

  private async initState(){ 
    this.getGastos();  
    this.getCategorias();
    this.busquedaCategoria = this.busquedaPorCategoriaDefault;
    this.busquedaDescripcion = ""
  }

  private async Busqueda(){
    const categoriaQuery : string = this.busquedaCategoria !== this.busquedaPorCategoriaDefault ? this.busquedaCategoria : "";
    const descriptionQuery : string = typeof this.busquedaDescripcion !== "undefined" && (typeof this.busquedaDescripcion === "string" && this.busquedaDescripcion.trim().length > 0)  ? this.busquedaDescripcion.toLowerCase().trim() : "" 
    this.loadingGastos = true;
    this.gastosService.busqueda(descriptionQuery, categoriaQuery).subscribe(
      (response) => {
        this.gastosService.formatResponse(response)
        this.gastos = this.gastosService.createObjects(response);
        this.determineTotal()
      },
      error => console.log(error),
      () => this.loadingGastos = false
    )
    this.filtrado = categoriaQuery !== "" && descriptionQuery !== "" ? true : false;
    this.determineTotal();
  }

  private BorrarGasto (gastoId:number){
    this.loadingGastos = true
    this.gastosService.borrarGastoPorId(gastoId).subscribe(
      () => this.initState(),
      error => console.log(error)
    )
  }

  private getGastos(){
    this.loadingGastos = true
    this.gastosService.getGastos().subscribe(
      (response) => {
        this.gastosService.formatResponse(response)
        this.gastos = this.gastosService.createObjects(response);
        this.determineTotal()
      },
      error => console.log(error) ,
      () =>  this.loadingGastos = false
    )
  }

  private getCategorias(){
    this.loadingCategorias = true
    this.gastosService.getCurrentCategorias().subscribe(
      (response) => {
        this.categorias = response;
        this.categorias.push( {tipo : this.busquedaPorCategoriaDefault , Id: 0 })
      },
      error => console.log(error) , 
      () => this.loadingCategorias = false,
    )
    
  }

  EditarGasto(){
    this.initState()
    scroll({
      top: 0,
      behavior: "smooth"
    });
  }


  private determineTotal(){
    this.deudaTotal = this.calcularDeudaTotal();
    this.gastoTotal = this.calcularGastoTotal();
  }

  private calcularDeudaTotal () : number{
    return this.gastos.filter( gasto => !gasto.pagado).reduce( (total , current) => total + current.getPriceDivided() , 0 );
  }

  private calcularGastoTotal () : number{
    return this.gastos.filter( gasto => gasto.pagado).reduce( (total , current) => total + current.getPriceDivided() , 0 );
  }

}
