import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BackApiService {

  constructor(
    private http: HttpClient
  ) { }

  private busquedaURL = 'https://localhost:7175/api/Carga/Busqueda'
  private cargaURL = 'https://localhost:7280/api/Carga'

  public getData(options: any) {
    return this.http.get(this.busquedaURL, {params: options});
  }

  public loadData(loadOptions: any) {
    return this.http.post(`${this.cargaURL}/LoadData`, loadOptions, {});
  }  

  public borrarBD() {
    return this.http.delete(`${this.cargaURL}/Delete`, {});
  }  

}
