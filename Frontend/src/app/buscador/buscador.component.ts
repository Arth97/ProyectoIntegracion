import { Component } from '@angular/core';
import { BackApiService } from 'src/api/back-api.service';

import Map from 'ol/Map';
import View from 'ol/View';

import GeoJSON from 'ol/format/GeoJSON';

import {Circle as CircleStyle, Fill, Stroke, Style, Icon } from 'ol/style';
import {OSM, Vector as VectorSource} from 'ol/source';
import {Tile as TileLayer, Vector as VectorLayer} from 'ol/layer';


@Component({
  selector: 'app-buscador',
  templateUrl: './buscador.component.html',
  styleUrls: ['./buscador.component.scss']
})
export class BuscadorComponent {

  title = 'Integracion';
  type = ["Hospital", "Centro de salud", "Otros", "Todos"]

  localidad: string = "";
  codigoPostal: string = "";
  provincia: string = "";
  tipo: any = "";
  map: Map = new Map;

  resultados = "";

  esExtent = [-12.0, 33.5, 4.5, 45.5];

  constructor(
    private backApiService: BackApiService
  ) {
  }

  // Inicializa Mapa
  ngOnInit() {
    this.map = new Map({
      view: new View({
        projection: 'EPSG:4326',
        extent: this.esExtent,
        center: [-2.3, 40.4],
        zoom: 1,
      }),
      layers: [
        new TileLayer({
          source: new OSM(),
        }),
      ],
      target: 'ol-map'
    });
  }

  changeClient(data: any) {
    this.tipo = this.type.indexOf(data.value)+1;
  }

  // Borra la consola de resultados
  borrar() {
    this.resultados = "";
  }

  // Extrae los datos pintandolos en la consola y en el mapa
  getData() {
    this.resultados = "";
    let options = {
      'localidad': this.localidad,
      'codigoPostal': this.codigoPostal,
      'provincia': this.provincia,
      'tipo': this.tipo
    }
    this.backApiService.getData(options).subscribe((resultados: any) => {
      resultados.forEach((res: any) => {
        res = res.properties;
        this.resultados = this.resultados + "Nombre: "+res.nombre+" "+"Codigo Postal: "+res.codigoPostal+" "+"Localidad: "+res.localidad+" "+"Provincia: "+res.provincia+" "+"Tipo: "+res.tipo+"\n\n";
      });

      let geojsonObject2 = {
        "type": "FeatureCollection",
        'features': resultados
      };

      let styleFunction = function (feature: any) {
        let imgStyle =  new Style({
          image: new Icon({
            // src: '../assets/icons/planeIcon.svg',
            src: '../../assets/icons/planeIcon.png',
          }),
        })
        return imgStyle
      };

      const vectorSource = new VectorSource({
        features: new GeoJSON().readFeatures(geojsonObject2),
      });
      
      const vectorLayer = new VectorLayer({
        source: vectorSource,
        style: styleFunction,
      });

      this.map.addLayer(vectorLayer);

    });
  }
  
}
