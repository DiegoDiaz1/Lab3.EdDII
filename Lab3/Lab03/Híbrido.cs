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
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("Escoger una opcion: \n(-c) Encriptar\n(-d) Desencriptar \n(-s) Salir del programa");
                switch (Console.ReadLine())
                {
                    //Encriptar
                    case "-c":
                        {
                            Console.WriteLine("Ingresar ruta del archivo");
                            string ruta = Console.ReadLine();
                            CrearLlavePrivada();
                            using(var fs = new FileStream(ruta, FileMode.Open))
                            {
                                using(var br = new BinaryReader(fs))
                                {
                                    using(var fs2 = new FileStream(ruta + ".cif", FileMode.Create))
                                    {
                                        using (var bwr = new BinaryWriter(fs2))
                                        {
                                            int blocksize = 4 * 1024;
                                            int iteration_number;
                                            if (fs.Length < blocksize)
                                                iteration_number = 1;
                                            else if (fs.Length % blocksize == 0)
                                                iteration_number = (int)fs.Length / blocksize;
                                            else
                                                iteration_number = ((int)fs.Length / blocksize) + 1;
                                            while (iteration_number-- > 0)
                                            {
                                                if (iteration_number == 0)
                                                    blocksize = (int)fs.Length % blocksize;
                                                byte[] input = br.ReadBytes(blocksize);
                                                byte[] output = new byte[input.Length];
                                                for (int i = 0; i < output.Length; i++)
                                                {
                                                    output[i] = objSdes.Codificar(input[i]);
                                                }
                                                bwr.Write(output);
                                                bwr.Flush();
                                            }
                                        }
                                    }
                                }
                            }
                            AnotarLlave(ruta);
                            Console.WriteLine("El archivo fue encriptado con exito.");
                            Console.ReadLine();
                            break;
                        }
                    //Desencriptar
                    case "-d":
                        {
                            Console.WriteLine("Ingresar la ruta del archivo");
                            string ruta = Console.ReadLine();
                            CopiarLlave(ruta + ".key");
                            using (var fs = new FileStream(ruta, FileMode.Open))
                            {
                                using (var br = new BinaryReader(fs))
                                {
                                    using (var fs2 = new FileStream(ruta + ".decif", FileMode.Create))
                                    {
                                        using (var bwr = new BinaryWriter(fs2))
                                        {
                                            int blocksize = 4 * 1024;
                                            int iteration_number;
                                            if (fs.Length < blocksize)
                                                iteration_number = 1;
                                            else if (fs.Length % blocksize == 0)
                                                iteration_number = (int)fs.Length / blocksize;
                                            else
                                                iteration_number = ((int)fs.Length / blocksize) + 1;
                                            while (iteration_number-- > 0)
                                            {
                                                if (iteration_number == 0)
                                                    blocksize = (int)fs.Length % blocksize;
                                                byte[] input = br.ReadBytes(blocksize);
                                                byte[] output = new byte[input.Length];
                                                for (int i = 0; i < output.Length; i++)
                                                {
                                                    output[i] = objSdes.Decodificar(input[i]);
                                                }
                                                bwr.Write(output);
                                                bwr.Flush();
                                            }
                                        }
                                    }
                                }
                            }
                            Console.WriteLine("El archivo fue desencriptado con exito");
                            Console.ReadLine();
                            break;
                        }
                    //Salir
                    case "-s":
                        {
                            salir = true;
                            break;
                        }
                }
                Console.Clear();
            }
        }
        private void CrearLlavePrivada()
        {
            //Crear el nuevo arreglo y generar la llave
            bool[] NuevaLlave = new bool[10];
            objRSA.GenerateKeys();
            //Llenar el nuevo arreglo
            Array.Copy(objRSA.GetPrivateKey(), 0, NuevaLlave, 0, 8);
            NuevaLlave[8] = false;
            NuevaLlave[9] = false;
            //Cambiar la llave
            objSdes.CambiarLlave(NuevaLlave);
        }
        //Procedimiento que crea un archivo.key
        private void AnotarLlave(string parametro)
        {
            String Salida = "";
            bool[] Llave = objSdes.ObtenerLlave();
            for (int i = 0; i < Llave.Length; i++)
            {
                Salida += Convert.ToInt32(Llave[i]);
            }
            using (var SW = new StreamWriter(parametro + ".cif.key"))
            {
                SW.Write(Salida);
            }
        }
        //Procedimiento que obtiene la llave del archivo.key
        private void CopiarLlave(string parametro)
        {
            bool[] Llave = new bool[10];
            char[] Caracteres = File.ReadAllText(parametro).ToCharArray();
            for (int i = 0; i < 10; i++)
            {
                Llave[i] = Convert.ToBoolean(Convert.ToInt32(Caracteres[i].ToString()));
            }
            objSdes.CambiarLlave(Llave);
        }
    }
}
