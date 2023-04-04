import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Integracion';
  optionSelected: Number = 0;


  switchOption() {
    this.optionSelected = this.optionSelected === 1? 0 : 1;
  }

}
