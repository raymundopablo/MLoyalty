import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpGeneralServiceService } from 'src/app/services/http-general-service.service';


@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  @Output() toggleSidebarForMe: EventEmitter<any> =  new EventEmitter();

  constructor(
    private _servises: HttpGeneralServiceService
  ) { }

  usuarioLogueado:any;
  rol:any;
  tituloPantalla: any;


  ngOnInit(): void {
    // Obtenemos los datos del sesion storage y los almacenamos en variables
    this.usuarioLogueado  = sessionStorage.Usuario;
    this.rol  = sessionStorage.rol;
    this._servises.customTitle.subscribe(msg => this.tituloPantalla = msg);
    }
  
  CerrarSesion() {
    window.location.href='/login';
  }


  toggleSideBar(){
    this.toggleSidebarForMe.emit();
   
  }

}
