export class UsuarioLogin {

    Usuario: string;
    Password: string; 
    TipoUsuario: string;

    constructor(Usuario, Password, TipoUsuario){
       this.Usuario = Usuario;
       this.Password = Password;
       this.TipoUsuario = TipoUsuario; 
    }
 }