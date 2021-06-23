import { Injectable } from '@angular/core';
import {Gasto, GastoData, GastoResponse} from '../models/gasto'
import {HttpClient , HttpHeaders } from "@angular/common/http"; 
import {Observable} from 'rxjs';
import { Categoria } from '../models/categoria';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GastosService {

  private Gastos : Gasto[]
  constructor(private http: HttpClient) { }

  public addGasto(gastoData : GastoData) : Observable<void>{
    const headers = {'Content-type' : 'application/json' }
    return this.http.post<void> (environment.apiUrl , gastoData , {headers})
  }

  public borrarGastoPorId(id : number) : Observable<void>{
    return this.http.delete<void> (environment.apiUrl  + id);
  }

  public updateGasto(gastoData : GastoData , gastoId : number){
    const headers = {'Content-type' : 'application/json' }
    const gasto : GastoResponse = { id : gastoId, ...gastoData }
    return this.http.put<boolean> (environment.apiUrl  + gastoId , gasto);
  }

  public getGastos() : Observable<GastoResponse[]>{
    return this.http.get<GastoResponse[]> (environment.apiUrl )
  }
  
  public getCurrentCategorias() : Observable<Categoria[]>{
    return this.http.get<Categoria[]> (environment.apiUrl  + "\categorias");
  }

  public busqueda(terminoDescripcion: string , terminoCategoria : string) : Observable<GastoResponse[]>{
    let queryParameter : String = terminoDescripcion !=="" || terminoCategoria !== "" ? "?" : ""
    queryParameter += terminoDescripcion !== "" ? `description=${terminoDescripcion}` : "";
    queryParameter += terminoDescripcion !== "" && terminoCategoria !== "" ? "&" : "";
    queryParameter += terminoCategoria !== "" ? `categoria=${terminoCategoria}` : "";
    return this.http.get<GastoResponse[]> (environment.apiUrl  + `buscar${queryParameter}`);
  }


  public createObjects(response : Array<GastoResponse>) : Array<Gasto>{
    return response.map(gastoData => new Gasto(gastoData.id , gastoData.description , gastoData.categoria , gastoData.date , gastoData.price , gastoData.pagado , gastoData.compartido , gastoData.compartidoEntre));
  }

  public formatResponse(response : Array<GastoResponse> ){
    response.forEach(gasto =>{
      gasto.date = gasto.date.split("T")[0].split("-").reverse().join("/")
    });
  }
}
