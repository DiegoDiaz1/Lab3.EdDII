# Lab3.EdDII
Encryptacion DES/RSA
  1) Se obtiene una llave privada siguiendo el algoritmo RSA.
  2) Se emplea esta llave para generar las llaves K1 y K2 y realizar el cifrado S-des.
  3) ARCHIVO DE SALIDA:
  Llave Privada: <bits de la llave privada obtenida con RSA.>
  Texto codificado: <Se muestra el texto cifrado.>
  
  
FUNCIONAMIENTO ALGORITMO S-DES
  Permutación 10:
    Entrada: 1,2,3,4,5,6,7,8,9,10
    Salida: 2,1,4,3,6,5,8,7,10,9
  Permutación 8:
    Entrada: 1,2,3,4,5,6,7,8
    Salida: 2,1,4,3,6,5,8,7
  Permutación 4:
    Entrada: 1,2,3,4
    Salida: 2,1,4,3
  Expandir & Permutar:
    Entrada: 1,2,3,4
    Salida: 1,2,3,4,1,2,3,4
  Permutación Inicial = Permutación Inversa = Permutación 8
  Switch Boxes: Como en la presentación
  S0:
  (01),(00),(11),(10)
  (11),(10),(01),(00)
  (00),(10),(01),(11)
  (11),(01),(11),(10)
  S1:
  (00),(01),(10),(11)
  (10),(00),(01),(11)
  (11),(00),(01),(00)
  (10),(01),(00),(11)
