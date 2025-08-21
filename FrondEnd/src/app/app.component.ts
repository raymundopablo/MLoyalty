import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Monitor';

  sideBarOpen= true;
  usuarioLogueado:any;
 
  ngOnInit(): void {
    this.usuarioLogueado  = sessionStorage.Usuario;
  }

  sideBarToggler(){
    this.sideBarOpen = !this.sideBarOpen;
  }
}
