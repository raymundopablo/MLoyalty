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
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-principal',
  templateUrl: './principal.component.html',
  styleUrls: ['./principal.component.css'],
  providers: [NgbPaginationConfig]
})
export class PrincipalComponent implements OnInit {

    form: FormGroup;
    ListaClientes: Cliente[] = [];
    ListaTiendas: Tienda[] = [];
    ListaArticulos: Articulo[] = [];
    IdTienda: number = 0;
    result: string = '';
    ArchivoASubir: File | null = null;
    rol: string = '';
    apiUrl: string = environment.apiUrl;
    Articulo: Articulo = {
    idArticulo: 0,
    descripcion: "",
    precio: 0,
    stock: 0,
    imagen: ""
  };

  constructor(  private formBuilder: FormBuilder, private _servises: HttpGeneralServiceService,  private toastr: ToastrService,  public dialog: MatDialog) {

     this.form = this.formBuilder.group({
      drpTienda: ['',[Validators.required]],
      txtIdArticulo: ['',[Validators.required]],
      txtDescripcion: ['',[Validators.required]],
      txtPrecio: ['',[Validators.required]],
      txtStock: ['',[Validators.required]],
      file: [null]
    })
  }


  ngOnInit(): void {
    this.rol = sessionStorage.getItem('Rol') || '';
    this.recuperarTiendas();
  }


    regresar(){
    window.location.href = '/principal'
  }

  CrearNuevo(){
    window.location.href = '/principal'
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

  eliminarArticulo(i){
      const message = 'Vas a eliminar articulo. Estas seguro?';
      const dialogData = new ConfirmDialogModel("Espera!", message);
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        maxWidth: "600px",
        data: dialogData
      });
  
      dialogRef.afterClosed().subscribe(dialogResult => {
        this.result = dialogResult;
        if (this.result){
            this._servises.eliminarItem("api/Articulos/eliminarArticulo/" + i + "/" , this.IdTienda).subscribe(data => {
              if (data.exito === 1){
                this.toastr.success(data.mensaje, 'Exito');
                this.recuperarArticulosPorTienda();
              }else{
                this.toastr.warning(data.mensaje, 'Warning');
              }
            }, error => {
              this.toastr.warning(error, 'Error');
            })
        }
      });
    }


  editarArticulo(i){
  const articuloEncontrado = this.ListaArticulos.find(articulo => articulo.idArticulo === i.idArticulo);

  if (articuloEncontrado) {
    this.Articulo = { ...articuloEncontrado }; // Copia las propiedades del articulo encontrado a this.Articulo
  } else {
    console.log("Articulo no encontrado");
  }
  }

uploadFile(event): any{
  this.ArchivoASubir = event.target.files![0];  
}

  insertarArticulo() {
      this._servises.insertarDatosConImagen(this.Articulo, this.IdTienda, this.ArchivoASubir).subscribe(data => {
        this.toastr.success('Artículo insertado correctamente', 'Éxito');
        this.form.reset();
      }, error => {
        this.toastr.warning(error, 'Error');
      });
    }
}



