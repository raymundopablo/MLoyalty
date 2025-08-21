import { Component, OnInit } from '@angular/core';
import { HttpGeneralServiceService } from 'src/app/services/http-general-service.service';
import { Articulo } from 'src/app/models/Articulo';
import { iDatoEntrada } from 'src/app/models/iDatoEntrada';
import { ToastrService } from 'ngx-toastr';
import { ConfirmDialogModel, ConfirmDialogComponent } from 'src/app/componentes/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-mis-productos',
  templateUrl: './mis-productos.component.html',
  styleUrls: ['./mis-productos.component.css']
})
export class MisProductosComponent implements OnInit {

  apiUrl: string = environment.apiUrl;
  IdCliente: number = 0;
  result: string = "";
  ListaArticulos: Articulo[] = [];
  constructor(private _servises: HttpGeneralServiceService, private toastr: ToastrService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.IdCliente = Number(sessionStorage.getItem('IdCliente'));
    this.recuperarMisArticulos();
  }


      recuperarMisArticulos() {
      const DatoEntrada: iDatoEntrada = {
        apiURL: 'api/Articulos/RecuperarArticulosPorCliente' + this.IdCliente,
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
                this._servises.eliminarItem("api/Articulos/eliminarArticuloCliente/" + i.idArticulo + "/" , this.IdCliente).subscribe(data => {
                  if (data.exito === 1){
                    this.toastr.success(data.mensaje, 'Exito');
                    this.recuperarMisArticulos();
                  }else{
                    this.toastr.warning(data.mensaje, 'Warning');
                  }
                }, error => {
                  this.toastr.warning(error, 'Error');
                })
            }
          });
        }

}
