import { Component, OnInit, ViewChild } from '@angular/core';
import {NgbPaginationConfig} from '@ng-bootstrap/ng-bootstrap';
import { HttpGeneralServiceService } from 'src/app/services/http-general-service.service';
import { ConfirmDialogModel, ConfirmDialogComponent } from 'src/app/componentes/confirm-dialog/confirm-dialog.component';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';  // Asegúrate de importar Subscription desde 'rxjs'
import { singleData } from 'src/app/models/singleData';
import { ChangeDetectorRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { iDatoEntrada } from 'src/app/models/iDatoEntrada';
import { Cliente } from 'src/app/models/Cliente';
import { Tienda } from 'src/app/models/Tienda';
import { Articulo } from 'src/app/models/Articulo';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';
import {ItemCarrito } from 'src/app/models/ItemCarrito';
import {Pedido } from 'src/app/models/Pedido';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-compra',
  templateUrl: './compra.component.html',
  styleUrls: ['./compra.component.css']
})
export class CompraComponent implements OnInit {
    
    form: FormGroup;
    ListaClientes: Cliente[] = [];
    ListaTiendas: Tienda[] = [];
    ListaArticulos: Articulo[] = [];
    IdTienda: number = 0;
    result: string = '';
    rol: string = '';
    IdCliente: number = 0;
    apiUrl: string = environment.apiUrl;

    item: ItemCarrito = {
      idArticulo: 0,
      descripcion: "",
      precio: 0,
      cantidad: 0
    };

    pedido: Pedido = {
      IdArticulo: 0,
      IdTienda: 0,
      Cantidad: 0,
      IdCliente: 0
    };

    CarritoDeCompra: ItemCarrito[] = [];

    Articulo: Articulo = {
    idArticulo: 0,
    descripcion: "",
    precio: 0,
    stock: 0,
    imagen: ""
  };

  constructor(  private formBuilder: FormBuilder, private _servises: HttpGeneralServiceService,  private toastr: ToastrService,  public dialog: MatDialog) { 
     this.form = this.formBuilder.group({
      drpTienda: ['',[Validators.required]]
    })
  }

  ngOnInit(): void {
    this.rol = sessionStorage.getItem('Rol') || '';
    this.IdCliente = Number(sessionStorage.getItem('IdCliente')) || 0;
    this.recuperarTiendas();
  }

    recuperarTiendas() {
    const DatoEntrada: iDatoEntrada = {
      apiURL: 'api/Tiendas/RecuperarTiendas',
      objeto: null
    }
    this._servises.recuperarListaItems(DatoEntrada).subscribe(data => {
      this.ListaTiendas = data;
    }, error => {
      this.toastr.warning(error, 'Error');
    });
  }


    recuperarArticulosPorTienda() {
    const DatoEntrada: iDatoEntrada = {
      apiURL: 'api/Articulos/RecuperarArticulosPorTienda' + this.IdTienda,
      objeto: null
    }
    this._servises.recuperarListaItems(DatoEntrada).subscribe(data => {
      this.ListaArticulos = data.listaArticulos;
    }, error => {
      this.toastr.warning(error, 'Error');
    });
  }

  eliminarItemCarrito(i) {
    this.CarritoDeCompra.splice(i, 1);
  }

agregarArticulo(item) {
  // Buscamos si el artículo ya existe en el carrito
  const existente = this.CarritoDeCompra.find(a => a.idArticulo === item.idArticulo);

  if (existente) {
    // Si ya existe, le sumamos 1 a la cantidad
    existente.cantidad += 1;
  } else {
    // Si no existe, lo agregamos con cantidad = 1
    this.CarritoDeCompra.push({
      idArticulo: item.idArticulo,
      descripcion: item.descripcion,
      precio: item.precio,
      cantidad: 1
    });
  }
  console.log(this.CarritoDeCompra);
}


comprar(){
  if (this.CarritoDeCompra.length === 0) {
    this.toastr.warning('El carrito está vacío', 'Advertencia');
    return;
  }

  for (let item of this.CarritoDeCompra) {
    this.pedido.IdArticulo = item.idArticulo;
    this.pedido.IdTienda = this.IdTienda;
    this.pedido.Cantidad = item.cantidad;
    this.pedido.IdCliente = this.IdCliente;

    const DatoEntrada: iDatoEntrada = {
    apiURL: 'api/Articulos/CompraArticulo',
    objeto: this.pedido
  }

  this._servises.insertarItem(DatoEntrada).subscribe(data => {
    this.toastr.success('Compra realizada con éxito', 'Éxito');
    this.CarritoDeCompra = [];
  }, error => {
    this.toastr.warning(error, 'Error');
  });
  }


}

}

