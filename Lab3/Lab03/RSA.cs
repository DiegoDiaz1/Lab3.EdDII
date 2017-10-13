using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Lab03
{
    class RSA
    {
        private int n = 200000;//Intervalo de numeros primos
        private Random newRNG= new Random();//generador de posiciones en la lista/
        private Int64 possibleCoprime;
        private int d;
       
        //generates a list of prime numbers
        private List<int> GetAllPrimes(){

            List<int> primeNumbers = new List<int> { 2 };
            for (int i = 3; i <= n; i += 2)
            {
                bool isPrime = true;
                foreach (int prime in primeNumbers)
                {
                    if (prime * prime > i)
                        break;
                    if (i % prime == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                    primeNumbers.Add(i);
            }
            return primeNumbers;

        }

        //sets the public and private key
        public void GenerateKeys(){

            List<int> primeList = GetAllPrimes();
            int randoPrime= newRNG.Next(0, primeList.Count);
            int randomNum1 = newRNG.Next(0, primeList.Count);
            int randomNum2 = newRNG.Next(0, primeList.Count);
            int i = 1;
            //checks that p and q are different
            if (randomNum2 == randomNum1){
                while (randomNum1 == randomNum2)
                    randomNum2 = newRNG.Next(0, primeList.Count);
            }

            //checks that e would be different than p and q
            if (randoPrime == randomNum1 || randoPrime == randomNum2){

                while (randoPrime == randomNum1 || randoPrime == randomNum2)
                    randoPrime = newRNG.Next(0,primeList.Count);
            }

            Int64 num1 = primeList[randomNum1];
            Int64 num2 = primeList[randomNum2];
            Int64 Number = num1 * num2;
            Int64 phi = ((num1) - 1) * ((num2) - 1);
            possibleCoprime = primeList.ElementAt(randoPrime);

            if (1 < possibleCoprime && possibleCoprime < Number ) {
                while (!Coprime(possibleCoprime,Number)){
                    randoPrime = newRNG.Next(0, primeList.Count);
                    if (randoPrime == randomNum1 || randoPrime == randomNum2){

                        while (randoPrime == randomNum1 || randoPrime == randomNum2)
                            randoPrime = newRNG.Next(0, primeList.Count);
                    }
                    else{
                        possibleCoprime = primeList.ElementAt(randoPrime);
                    }

                }
 

            }

            while ((i*possibleCoprime)%phi != 1)
            {
                i++;
            }

            d = i;


        }
        //Operates 2 values to se if they  are or not coprime numbers
        private Int64 GetModulus(Int64 value1, Int64 value2) {

            while (value1 != 0 && value2 != 0)
            {
                if (value1 > value2)
                    value1 %= value2;
                else
                    value2 %= value1;
            }
            return Math.Max(value1, value2);
        }
        //Returns a boolean that indicates if the 2 values are coprimes 
        private bool Coprime(Int64 value1, Int64 value2){
            return GetModulus(value1, value2) == 1;
        }


        public bool[] GetPrivateKey() {
            byte[] arrayOFBytes = BitConverter.GetBytes(possibleCoprime);
            BitArray newBits = new BitArray(arrayOFBytes);
            bool[] bitsAsBool = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                bitsAsBool[i] = newBits[i];
            }
            return bitsAsBool;


        }
    }
}
