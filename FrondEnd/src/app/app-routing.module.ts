import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './componentes/login/login.component';
import { PrincipalComponent } from './componentes/principal/principal.component';
import { ClientesComponent } from './componentes/clientes/clientes.component';
import { MisProductosComponent } from './componentes/mis-productos/mis-productos.component';
import { CompraComponent } from './componentes/compra/compra.component';


const routes: Routes = [
  {path: '', component: LoginComponent},
  {path: 'login', component: LoginComponent},
  {path: 'principal', component: PrincipalComponent},
  {path: 'Clientes', component: ClientesComponent},
  {path: 'MisProductos', component: MisProductosComponent},
  {path: 'Compra', component: CompraComponent},
  {path: '**', redirectTo: ''} // si no encuentra ninguna ruta te redirige a la inicial
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
