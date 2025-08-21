import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './componentes/login/login.component';
import { FooterComponent } from './componentes/footer/footer.component';
import { MaterialModule} from './material/material.module';
import { MenuComponent } from './componentes/menu/menu.component';
import { PrincipalComponent } from './componentes/principal/principal.component';
import { SideNavComponent} from './componentes/side-nav/side-nav.component';
import {MatDialogModule, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {MatExpansionModule} from '@angular/material/expansion';

// Idioma español para fechas
import locales from '@angular/common/locales/es-mx';
import { registerLocaleData } from '@angular/common';


registerLocaleData(locales, 'es');
// Pdf pdfmake-wrapper
import { PdfMakeWrapper } from 'pdfmake-wrapper';
import * as pdfFonts from "pdfmake/build/vfs_fonts";

import { ConfirmDialogComponent } from './componentes/confirm-dialog/confirm-dialog.component';

import { NgxChartsModule } from '@swimlane/ngx-charts';
import { ClientesComponent } from './componentes/clientes/clientes.component';
import { MisProductosComponent } from './componentes/mis-productos/mis-productos.component';
import { CompraComponent } from './componentes/compra/compra.component';


// If any issue using previous fonts import. you can try this:
// import pdfFonts from "pdfmake/build/vfs_fonts";
// Set the fonts to use
PdfMakeWrapper.setFonts(pdfFonts);

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    FooterComponent,
    MenuComponent,
    PrincipalComponent,
    SideNavComponent,
    ConfirmDialogComponent,
    ClientesComponent,
    MisProductosComponent,
    CompraComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    MaterialModule,
    MatDialogModule,
    NgbModule,
    MatExpansionModule,
    NgxChartsModule,
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'es'}], 
  bootstrap: [AppComponent]
})
export class AppModule { }
