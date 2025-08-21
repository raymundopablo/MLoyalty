import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators} from '@angular/forms';

import { ToastrService } from 'ngx-toastr';
import {Router} from "@angular/router";
import {Location} from "@angular/common";
import { HttpGeneralServiceService } from 'src/app/services/http-general-service.service';
import { UsuarioLogin } from 'src/app/models/usuarioLogin';
import { iDatoEntrada } from 'src/app/models/iDatoEntrada';




@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  IdEmpresa: number = 0;
  IdSucursal: number = 0;
  IdSala: number = 0;
  form: FormGroup;
  tipoUsuario: string = 'cliente'; // Valor por defecto
  
  constructor(private formBuilder: FormBuilder, private _loginService: HttpGeneralServiceService, private toastr: ToastrService, 
    private router:Router, private Location: Location ) {
    this.form = this.formBuilder.group({
      usuario: ['',[Validators.required]],
      password: ['',[Validators.required]],
      tipoUsuario: ['']
    })
   }

  ngOnInit(): void {
    sessionStorage.clear();
  }


  verificarUsuario(){


    const datosUsuario: UsuarioLogin = {
      Usuario: this.form.get('usuario')?.value,
      Password: this.form.get('password')?.value,
      TipoUsuario: this.tipoUsuario
    }

    sessionStorage.Usuario = datosUsuario.Usuario;

    // Servicio que se conecta a la base de datos
    this._loginService.recuperarUsuario(datosUsuario).subscribe(data => {
      if (data.exito === 1){
        this.toastr.success(data.mensaje, 'Exito');
        this.form.reset();
        console.log(data);
        //Almacena datos de sesion en sesionstorage
        // verifica que sea compatible el session storageZ
        if (typeof(Storage) !== 'undefined') {
          sessionStorage.Usuario = data.datos.userName;
          sessionStorage.rol = data.datos.rolName;
          sessionStorage.IdCliente = data.datos.idCliente;
        } else {
         // Código cuando Storage NO es compatible
         alert("No compatible local storage");
        }

        //this.Location.replaceState('/'); // clears browser history so they can't navigate with back button
        //this.router.navigate(['/principal']); //Redirige
        if (sessionStorage.rol == 'Administrador'){
          window.location.href='/principal';
        }else{
          window.location.href='/MisProductos';
        }
          

      }else{
        this.toastr.warning("No se pudo conectar: " + data.mensaje, 'Warning');
      }

    }, error => {
      this.toastr.warning(error, 'No se pudo conectar con el servidor');
    });
    
  }

}
