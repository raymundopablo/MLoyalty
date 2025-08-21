import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FormatMonedaService {

  constructor() { }


  formatPesosTexto(cantidad: number)
  { 
    var numero: number = 0;
    var cantidadSinSignos = cantidad.toString().replace(/[$]/g,'');
    var uy = new Intl.NumberFormat('es-MX',{style: 'currency', currency:'MXN'}).format(Number(cantidadSinSignos));
    numero = Number(cantidadSinSignos); 
    var Texto = uy;

    return Texto;
  }

}
