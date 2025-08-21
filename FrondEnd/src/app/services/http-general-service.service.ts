import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http'; 
import { Observable } from 'rxjs';
import { AspNetUser } from 'src/app/models/AspNetUser';
import { Rol } from 'src/app/models/rol';
import { iDatoEntrada } from 'src/app/models/iDatoEntrada';
import { iDatoOut } from 'src/app/models/iDatoOut';
import { BehaviorSubject } from 'rxjs';
import { Articulo } from 'src/app/models/Articulo';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpGeneralServiceService {

  private myAppUrl = environment.apiUrl;
  private myApiYrl =  '';


  private title = new BehaviorSubject<string>('Inicio');
 
  public customTitle = this.title.asObservable();

  constructor( private http: HttpClient) { }
 
    public changeTitle(msg: string): void {
      this.title.next(msg);
    }

    recuperarUsuario(user: any): Observable<any> {
     if (user.TipoUsuario === 'admin') {
        this.myApiYrl =  'api/Usuario/login';
      } else {
        this.myApiYrl =  'api/Clientes/login';
      }
    return this.http.post(this.myAppUrl + this.myApiYrl, user);
    }

    recuperarItemId(DatoEntrada: iDatoEntrada): Observable<any> {
      this.myApiYrl =  DatoEntrada.apiURL + DatoEntrada.objeto;
      return this.http.get<iDatoOut>(this.myAppUrl + this.myApiYrl);
      }

      insertarItem(DatoEntrada: iDatoEntrada): Observable<any> {
        this.myApiYrl =  DatoEntrada.apiURL;
        return this.http.post(this.myAppUrl + this.myApiYrl, DatoEntrada.objeto);
       }
  
      eliminarItem(apiUrl: string, id: any): Observable<any> {
        this.myApiYrl =  apiUrl + id;
        return this.http.post(this.myAppUrl + this.myApiYrl, id);
       }
       
       recuperarListaItems(DatoEntrada: iDatoEntrada): Observable<any> {
        this.myApiYrl =  DatoEntrada.apiURL;
        return this.http.get<iDatoOut>(this.myAppUrl + this.myApiYrl);
      }


      RecuperarItemsPost(DatoEntrada: iDatoEntrada): Observable<any> {
        this.myApiYrl =  DatoEntrada.apiURL;
        return this.http.post(this.myAppUrl + this.myApiYrl, DatoEntrada.objeto);
       }


    insertarDatosConImagen(articulo: Articulo, IdTienda: number, imagen: File | null): Observable<any> {
    this.myApiYrl =  'api/Articulos/AltaArticulo';
    const combinedFormData = new FormData();
    combinedFormData.append('articulo', JSON.stringify(articulo));
    combinedFormData.append('IdTienda', IdTienda.toString());
    combinedFormData.append('imagen', imagen !== null ? imagen : new File([], 'default'), imagen?.name);

    return this.http.post(this.myAppUrl + this.myApiYrl, combinedFormData);
   }

}