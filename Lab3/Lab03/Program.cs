using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab03
{
    class Program
    {
        static void Main(string[] args)
        {
            S_des objSdes = new S_des();
            bool salir = false;
            while(!salir)
            {
                Console.WriteLine("Escoger una opcion: \n(-c) Encriptar\n(-d) Desencriptar \n(-s)");
                switch (Console.ReadLine())
                {
                    //Encriptar
                    case "-c":
                        {
                            Console.WriteLine("Ingresar ruta del archivo");
                            string ruta = Console.ReadLine();
                            FileStream fs = new FileStream(ruta, FileMode.Open);
                            BinaryReader br = new BinaryReader(fs);
                            FileStream fs2 = new FileStream(ruta + ".cif", FileMode.Create);
                            BinaryWriter bwr = new BinaryWriter(fs2);
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
                            bwr.Close();
                            fs2.Close();
                            br.Close();
                            fs.Close();
                            Console.WriteLine("El archivo fue encriptado con exito.");
                            Console.ReadLine();
                            break;
                        }
                    //Desencriptar
                    case "-d":
                        {
                            Console.WriteLine("Ingresar la ruta del archivo");
                            string ruta = Console.ReadLine();
                            FileStream fs = new FileStream(ruta, FileMode.Open);
                            BinaryReader br = new BinaryReader(fs);
                            FileStream fs2 = new FileStream(ruta + ".decif", FileMode.Create);
                            BinaryWriter bwr = new BinaryWriter(fs2);
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
                            bwr.Close();
                            fs2.Close();
                            br.Close();
                            fs.Close();
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
    }
}
