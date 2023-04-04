import { Component } from '@angular/core';
import { BackApiService } from 'src/api/back-api.service';

@Component({
  selector: 'app-carga',
  templateUrl: './carga.component.html',
  styleUrls: ['./carga.component.scss']
})
export class CargaComponent {

  seleccionarTodas = false;
  islasBaleares = false;
  comunidadValenciana = false;
  euskadi = false;

  resultados = "";

  constructor(
    private backApiService: BackApiService
  ) {}

  // Carga los datos en la base de datos
  loadData() {
    this.resultados = "";
    let option: any;
    if (this.seleccionarTodas)
      option = {"islasBaleares": true, "comunidadValenciana": true, "euskadi": true};
    else 
      option = {"islasBaleares": this.islasBaleares, "comunidadValenciana": this.comunidadValenciana, "euskadi": this.euskadi};

    this.backApiService.loadData(option).subscribe((res: any) => {
      this.resultados = res;
    });
  }

  // Elimina la base de datos
  borrarBD() {
    this.backApiService.borrarBD().subscribe();
  }

  // Limpia consola de resultados
  cancel() {
    this.resultados = "";
    this.seleccionarTodas = false;
    this.islasBaleares = false;
    this.comunidadValenciana = false;
    this.euskadi = false;
  }

}
