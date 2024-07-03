
using Firebase.Auth;
using Firebase.Storage;

namespace proyectoFinal.Services
{
    public class ServicioImagenP : IServicioImagenP
    {
        public async Task<string> SubirImagen(Stream archivo, string nombre)
        {

            string email = "camilatefa2542@gmail.com";
            string clave = "devuelvemeelcorazon";
            string ruta = "proyectoclaselibreria-6cd68.appspot.com";
            string api_key = "AIzaSyDrufVRstSLm5Tay_-8w1vAK1N77rBO0o4";

            var auth = new FirebaseAuthProvider
                (new FirebaseConfig(api_key));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
              ruta,
              new FirebaseStorageOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                  ThrowOnCancel = true
              })

               .Child("Productos")
               .Child(nombre)
               .PutAsync(archivo, cancellation.Token);

            var downloadURL = await task;
            return downloadURL;
        }
    }
}
