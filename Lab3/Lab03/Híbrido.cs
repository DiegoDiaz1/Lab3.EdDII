using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab03
{
    class Híbrido
    {
        RSA objRSA = new RSA();
        S_des objSdes = new S_des();
        //Pantalla mostrada al usuario.
        public void PantallaInicial()
        {
            bool Salir = false;
            while (!Salir)
            {
                Console.WriteLine("Escoger una opción: \n(-c)Encriptar\n(-d)Desencriptar\n(-s)Salir");
                switch (Console.ReadLine())
                {
                    //Encriptar
                    case "-c":
                        {
                            Console.WriteLine("Escribir la ruta del archivo");
                            break;
                        }
                    //Desencriptar
                    case "-d":
                        {
                            Console.WriteLine("Ingresar la ruta del archivo");
                            break;
                        }
                    //Salir
                    case "-s":
                        {
                            Salir = true;
                            break;
                        }
                }
                Console.Clear();
            }
        }
    }
}
