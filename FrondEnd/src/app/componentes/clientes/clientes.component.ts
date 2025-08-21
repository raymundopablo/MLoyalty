import { Component, OnInit, ViewChild } from '@angular/core';
import {NgbPaginationConfig} from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';
import { HttpGeneralServiceService } from 'src/app/services/http-general-service.service';
import { ToastrService } from 'ngx-toastr';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatSort} from '@angular/material/sort';
import { Cliente } from 'src/app/models/Cliente';
import {NgbModal, ModalDismissReasons, NgbModalOptions} from '@ng-bootstrap/ng-bootstrap';
import { MatDialog } from '@angular/material/dialog';
import { iDatoEntrada } from 'src/app/models/iDatoEntrada';
import { ConfirmDialogModel, ConfirmDialogComponent } from 'src/app/componentes/confirm-dialog/confirm-dialog.component';
import {FormControl} from '@angular/forms';
import { Articulo } from 'src/app/models/Articulo';


@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})
export class ClientesComponent implements OnInit {

  displayedColumns = ['idCliente','nombres','apellidos','direccion','accion'];
  dataSource: MatTableDataSource<Cliente>;

  stateCtrl = new FormControl('');
  form: FormGroup;
  ListaClientes: Cliente[] = [];
  ListaArticulos: Articulo[] = [];
  result: string = '';
  mensaje: string = "";
  Exito: number = 10;
  usuario: string = "";


  Cliente: Cliente = {
    idCliente: -1,
    nombres: "",
    apellidos: "",
    direccion: "",
    userName: "",
    password: ""
  };

 
  closeResult: string = '';
  modalOptions:NgbModalOptions;
  title = 'ng-bootstrap-modal-demo';

  constructor(
    private formBuilder: FormBuilder, 
    private _servises: HttpGeneralServiceService, 
    private toastr: ToastrService,
    public dialog: MatDialog,
    private modalService: NgbModal,
  ) {
    this.dataSource = new MatTableDataSource(this.ListaClientes);  
    this.modalOptions = {backdrop:'static', backdropClass:'customBackdrop', size: 'xl'}

    this.form = this.formBuilder.group({
      txtNombre: ['',[Validators.required]],
      txtApellidos: ['',[Validators.required]],
      txtDireccion: ['',[Validators.required]],
      txtUsername: ['',[Validators.required]],
      txtPassword: ['' , Validators.required]
    })

   }

   @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
    this.usuario = sessionStorage.Usuario;
    this.recuperarClientes();
  }

  open(content) {
    this.modalService.open(content, this.modalOptions).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }


  changeEventdDatepickerFechaComienza(event){
    
  }


  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'POR PULSAR ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return  `with: ${reason}`;
    }
  }


  regresar(){
    window.location.href = '/Clientes'
  }

  CrearNuevo(){
    window.location.href = '/Clientes'
  }

  recuperarClientes() {
    const DatoEntrada: iDatoEntrada = {
      apiURL: 'api/Clientes/RecuperarClientes',
      objeto: null
    }
    this._servises.recuperarListaItems(DatoEntrada).subscribe(data => {
      this.ListaClientes = data.listaClientes;

      console.log(this.ListaClientes);
      this.dataSource = new MatTableDataSource(this.ListaClientes);
      this.dataSource.paginator = this.paginator;
    
    }, error => {
      this.toastr.warning(error, 'Error');
    });
  }


  insertarCliente(){
    const cliente: Cliente = {
      idCliente: this.Cliente.idCliente,
      nombres : this.Cliente.nombres,
      apellidos: this.Cliente.apellidos,
      direccion: this.Cliente.direccion,
      userName : this.Cliente.userName,
      password : this.Cliente.password
    }
  


    const DatoEntrada: iDatoEntrada = {
      apiURL: 'api/Clientes/insertarCliente',
      objeto: cliente
    }

    var message = 'Guardar o actualizar el cliente, deseas continuar?';
    const dialogData = new ConfirmDialogModel("Socio", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "600px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;

      if (this.result){

        this._servises.insertarItem(DatoEntrada).subscribe(data => {
          if (data.exito === 1){
            this.toastr.success(data.mensaje, 'Exito');
            this.recuperarClientes();
            this.form.reset();
          }else{
            this.toastr.warning(data.mensaje, 'Warning');
          }
        }, error => {
          this.toastr.warning(error, 'Error');
        })
      }
    });
  }

  
  eliminarCliente(i){
      const message = 'Vas a eliminar cliente. Estas seguro?';
      const dialogData = new ConfirmDialogModel("Espera!", message);
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        maxWidth: "600px",
        data: dialogData
      });
  
      dialogRef.afterClosed().subscribe(dialogResult => {
        this.result = dialogResult;
        if (this.result){
            this._servises.eliminarItem("api/Clientes/eliminarCliente/", i.idCliente).subscribe(data => {
              if (data.exito === 1){
                this.toastr.success(data.mensaje, 'Exito');
                this.recuperarClientes();
              }else{
                this.toastr.warning(data.mensaje, 'Warning');
              }
            }, error => {
              this.toastr.warning(error, 'Error');
            })
        }
      });
    }

  seleccionarCliente(i){
  const clienteEncontrado = this.ListaClientes.find(cliente => cliente.idCliente === i.idCliente);

  if (clienteEncontrado) {
    this.Cliente = { ...clienteEncontrado }; // Copia las propiedades del cliente encontrado a this.Cliente
  } else {
    console.log("Cliente no encontrado");
  }
  }


  seleccionarArticulos(a){
    const DatoEntrada: iDatoEntrada = {
      apiURL: 'api/Articulos/RecuperarArticulosPorCliente' + a.idCliente,
      objeto: null
    }
    this._servises.recuperarListaItems(DatoEntrada).subscribe(data => {
      this.ListaArticulos = data.listaArticulos;
      console.log(this.ListaArticulos);
    
    }, error => {
      this.toastr.warning(error, 'Error');
    });
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }


}
