import { Component, OnInit } from '@angular/core';
import { HttpGeneralServiceService } from 'src/app/services/http-general-service.service';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.css']
})
export class SideNavComponent implements OnInit {

  constructor(private _servises: HttpGeneralServiceService) { }

  rol: any;
  message: string = '';
  
  ngOnInit(): void {
    this._servises.customTitle.subscribe(msg => this.message = msg);
    this.rol = sessionStorage.rol;
  }

  
  asignarNombre(name){
    this._servises.changeTitle(name);
  }

}
