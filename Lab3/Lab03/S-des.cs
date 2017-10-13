using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab03
{
    class S_des
    {
        //Objetos propios del S-des.
        private bool[] LlavePrivada;
        private bool[] LlaveA = new bool[8];
        private bool[] LlaveB = new bool[8];
        private bool[] Arreglo;
        private bool[] SubArregloA;
        private bool[] subArregloB;
        private bool[] subArregloC;
        private bool[] subArregloD;
        private string[,] S0 = new string[,] { {"01", "00", "11", "10"}, { "11", "10", "01", "00" }, {"00", "10","01", "11"}, {"11", "01", "11", "10" } };
        private string[,] S1 = new string[,] { { "00", "01", "10", "11" }, { "10", "00", "01", "11" }, { "11", "00", "01", "00" }, { "10", "01", "00", "11" } };
        //Obtiene la permutación posición par,impar (2,1,4,3,6,5,8,7,10,9...)
        private bool[] Permutación(int longitud, bool[] arreglo)
        {
            bool[] Salida = new bool[longitud];
            for (int i = 0; i < longitud; i++)
            {
                if(i%2 == 0)
                {
                    Salida[i] = arreglo[i + 1];
                }
                else
                {
                    Salida[i] = arreglo[i - 1];
                }
            }
            return Salida;
        }
        //Corre los datos del arreglo hacia la izquierda
        private void DesplazamientoIzq(int cantidad, bool[] arreglo)
        {
            bool primero = arreglo[0];
            bool segundo = arreglo[1];
            Array.Copy(arreglo, cantidad, arreglo, 0, arreglo.Length - cantidad);
            if(cantidad == 1)
            {
                arreglo[arreglo.Length - 1] = primero;
            }
            else
            {
                arreglo[arreglo.Length - 2] = primero; 
                arreglo[arreglo.Length - 1] = segundo;
            }
        }
        //Expande y permuta el arreglo original
        private bool[] ExpandirPermutar(int cantidad, bool[] original)
        {
            bool[] salida = new bool[2*original.Length];
            Array.Copy(original,0,salida,0,original.Length);
            Array.Copy(original, 0, salida, original.Length, original.Length);
            Permutación(cantidad, salida);
            return salida;
        }
        //Función XOR
        private bool[] XOR(bool[] llave, bool[] original)
        {
            bool[] Salida = new bool[llave.Length];
            for (int i = 0; i < llave.Length; i++)
            {
                if(original[i] == llave[i])
                {
                    Salida[i] = false;
                }
                else
                {
                    Salida[i] = true;
                }
            }
            return Salida;
        }
        //Función Switchbox
        private bool[] Switchbox(bool[] arreglo, string[,] matriz)
        {
            bool[] Salida = new bool[2];
            int fila = Convert.ToInt16(Convert.ToInt16(arreglo[0]).ToString() + Convert.ToInt16(arreglo[3]).ToString(), 2);
            int columna = Convert.ToInt16(Convert.ToInt16(arreglo[1]).ToString() + Convert.ToInt16(arreglo[2]).ToString(), 2);
            char[] bits = matriz[columna, fila].ToCharArray();
            Salida[0] = Convert.ToBoolean(Convert.ToInt32(bits[0].ToString()));
            Salida[1] = Convert.ToBoolean(Convert.ToInt32(bits[1].ToString()));
            return Salida;
        }
        //Función XOR inverso
        private bool[] XORInverso(bool[] xor, bool[] arreglo)
        {
            bool[] Salida = new bool[arreglo.Length];
            for (int i = 0; i < Salida.Length; i++)
            {
                //Si eran diferentes
                if (xor[i])
                {
                    Salida[i] = !arreglo[i];
                }
                //Si eran iguales
                else
                {
                    Salida[i] = arreglo[i];
                }
            }
            return Salida;
        }
        //Función que transforma un byte a un arreglo booleano.
        private bool[] FormatoEntrada(byte parametro)
        {
            BitArray bits = new BitArray(new byte[] { parametro });
            bool[] Salida = new bool[8];
            for (int i = 0; i < Salida.Length; i++)
            {
                Salida[i] = bits[7-i];
            }
            return Salida;
        }
        //Función que transforma la salida a un byte.
        private byte FormatoSalida(bool[] parametro)
        {
            BitArray bits = new BitArray(parametro);
            var reversed = new BitArray(bits.Cast<bool>().Reverse().ToArray());
            byte[] bytes = new byte[1];
            reversed.CopyTo(bytes, 0);
            return bytes[0];
        }
        //Procedimmiento que cambia el valor de la llave privada (Necesaria para el híbrido).
        public void CambiarLlave(bool[] Llave)
        {
            LlavePrivada = Llave;
        }
        //Procedimiento que obtiene la llave privada (Necesaria para el híbrido).
        public bool[] ObtenerLlave()
        {
            return this.LlavePrivada;
        }
        //Procedimiento que obtiene valores para las llaves K1 y K2
        private void GeneradorLlaves()
        {
            Arreglo = Permutación(10, LlavePrivada);
            SubArregloA = new bool[5];
            subArregloB = new bool[5];
            Array.Copy(Arreglo, 0, SubArregloA, 0, 5);
            Array.Copy(Arreglo, 5, subArregloB, 0, 5);
            DesplazamientoIzq(1, SubArregloA);
            DesplazamientoIzq(1, subArregloB);
            Array.Copy(SubArregloA, 0, Arreglo, 0, 5);
            Array.Copy(subArregloB, 0, Arreglo, 5, 5);
            LlaveA = Permutación(8, Arreglo);
            DesplazamientoIzq(2, SubArregloA);
            DesplazamientoIzq(2, subArregloB);
            Array.Copy(SubArregloA, 0, Arreglo, 0, 5);
            Array.Copy(subArregloB, 0, Arreglo, 5, 5);
            LlaveB = Permutación(8, Arreglo);
        }
        //Codificación S-des o RSA/S-des según el tipo especificado.
        public byte Codificar(byte Entrada)
        {
            //Generar las llaves (k1 & k2)
            GeneradorLlaves();
            //Permutación inicial
            bool[] parametro = FormatoEntrada(Entrada);
            Arreglo = Permutación(8, parametro);
            //Separar en dos subgrupos (A & B)
            SubArregloA = new bool[4];
            subArregloB = new bool[4];
            Array.Copy(Arreglo, 0, SubArregloA, 0, 4);
            Array.Copy(Arreglo, 4, subArregloB, 0, 4);
            //Realizar el procedimiento con el subgrupo B
            Arreglo = ExpandirPermutar(8, subArregloB);
            Arreglo = XOR(LlaveA, Arreglo);
            subArregloC = new bool[4];
            subArregloD = new bool[4];
            Array.Copy(Arreglo, 0, subArregloC, 0, 4);
            Array.Copy(Arreglo, 4, subArregloD, 0, 4);
            Arreglo = new bool[4];
            Array.Copy(Switchbox(subArregloC, S0), 0, Arreglo, 0, 2);
            Array.Copy(Switchbox(subArregloD, S1), 0, Arreglo, 2, 2);
            Arreglo = Permutación(4, Arreglo);
            //Realizar XOR y obtener nuevo subgrupo A
            SubArregloA = XOR(SubArregloA, Arreglo);
            //Realizar el mismo procedimiento con el subgrupo A
            Arreglo = ExpandirPermutar(8, SubArregloA);
            Arreglo = XOR(LlaveB, Arreglo);
            Array.Copy(Arreglo, 0, subArregloC, 0, 4);
            Array.Copy(Arreglo, 4, subArregloD, 0, 4);
            Arreglo = new bool[4];
            Array.Copy(Switchbox(subArregloC, S0), 0, Arreglo, 0, 2);
            Array.Copy(Switchbox(subArregloD, S1), 0, Arreglo, 2, 2);
            Arreglo = Permutación(4, Arreglo);
            //Realizar XOR y obtener nuevo subgrupo B
            subArregloB = XOR(subArregloB, Arreglo);
            //Preparar la salida del encriptado.
            Arreglo = new bool[8];
            Array.Copy(subArregloB, 0, Arreglo, 0, 4);
            Array.Copy(SubArregloA, 0, Arreglo, 4, 4);
            return FormatoSalida(Permutación(8, Arreglo));
        }
        public byte Decodificar(byte Entrada)
        {
            //Generar las llaves (k1 & k2)
            GeneradorLlaves();
            //Permutación inversa.
            bool[] parametro = FormatoEntrada(Entrada);
            Arreglo = Permutación(8, parametro);
            //Separar en dos subgrupos (B & A)
            SubArregloA = new bool[4];
            subArregloB = new bool[4];
            Array.Copy(Arreglo, 0, subArregloB, 0, 4);
            Array.Copy(Arreglo, 4, SubArregloA, 0, 4);
            //Realizar el procedimiento con el subgrupo A
            Arreglo = ExpandirPermutar(8, SubArregloA);
            Arreglo = XOR(LlaveB, Arreglo);
            subArregloC = new bool[4];
            subArregloD = new bool[4];
            Array.Copy(Arreglo, 0, subArregloC, 0, 4);
            Array.Copy(Arreglo, 4, subArregloD, 0, 4);
            Arreglo = new bool[4];
            Array.Copy(Switchbox(subArregloC, S0), 0, Arreglo, 0, 2);
            Array.Copy(Switchbox(subArregloD, S1), 0, Arreglo, 2, 2);
            Arreglo = Permutación(4, Arreglo);
            //Realizar XOR inverso y obtener subgrupoB original
            subArregloB = XORInverso(subArregloB, Arreglo);
            //Realizar el mismo procedimiento con el subgrupo B
            Arreglo = ExpandirPermutar(8, subArregloB);
            Arreglo = XOR(LlaveA, Arreglo);
            Array.Copy(Arreglo, 0, subArregloC, 0, 4);
            Array.Copy(Arreglo, 4, subArregloD, 0, 4);
            Arreglo = new bool[4];
            Array.Copy(Switchbox(subArregloC, S0), 0, Arreglo, 0, 2);
            Array.Copy(Switchbox(subArregloD, S1), 0, Arreglo, 2, 2);
            Arreglo = Permutación(4, Arreglo);
            //Realizar XOR inverso y obtener subgrupoB original
            SubArregloA = XORInverso(SubArregloA, Arreglo);
            //Preparar la salida del desencriptado.
            Arreglo = new bool[8];
            Array.Copy(SubArregloA, 0, Arreglo, 0, 4);
            Array.Copy(subArregloB, 0, Arreglo, 4, 4);
            return FormatoSalida(Permutación(8, Arreglo));
        }
       
    }
}
