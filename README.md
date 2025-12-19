# ConsoleRSA

Una aplicación de consola en C# que implementa el algoritmo de encriptación RSA para cifrar y descifrar archivos.

## Descripción

ConsoleRSA es una implementación del algoritmo de criptografía asimétrica RSA que permite:
- Generar pares de claves públicas y privadas
- Cifrar archivos de cualquier tipo
- Descifrar archivos previamente cifrados
- Almacenar las claves generadas en archivos binarios
- Empaquetar las claves en un archivo ZIP para facilitar su distribución

## Requisitos

- .NET Core 3.1 o superior
- Sistema operativo: Windows, Linux o macOS

## Estructura del Proyecto

```
ConsoleRSA/
├── ConsoleRSA/
│   ├── Program.cs           # Punto de entrada de la aplicación
│   ├── RSA_Encryptor.cs     # Clase principal con la lógica de RSA
│   ├── ConsoleRSA.csproj    # Archivo de proyecto
│   ├── Keys/                # Directorio para claves generadas
│   └── BundledKeys/         # Directorio para claves empaquetadas
└── ConsoleRSA.sln           # Archivo de solución
```

## Características

### Generación de Claves
- Genera claves RSA a partir de dos números primos (p y q)
- Calcula automáticamente:
  - `n = p * q` (módulo)
  - `φ(n) = (p-1)(q-1)` (función phi de Euler)
  - `e` (exponente público)
  - `d` (exponente privado)
- Guarda las claves en formato binario optimizado

### Cifrado
- Cifra archivos byte por byte usando la clave pública
- Procesa archivos en bloques de 100,000 bytes para optimizar memoria
- Genera archivos con extensión `.rsa`

### Descifrado
- Descifra archivos usando la clave privada
- Restaura el archivo original con su contenido intacto

### Gestión de Claves
- Almacena claves públicas en: `Keys/public.key`
- Almacena claves privadas en: `Keys/private.key`
- Empaqueta ambas claves en: `BundledKeys/Keys.zip`

## Instalación

1. Clonar el repositorio:
```bash
git clone <url-del-repositorio>
cd ConsoleRSA
```

2. Restaurar dependencias:
```bash
dotnet restore
```

3. Compilar el proyecto:
```bash
dotnet build
```

## Uso

### Configuración Básica

Antes de ejecutar, configura las rutas en `Program.cs`:

```csharp
int p = 61, q = 53; // Números primos (p * q debe ser mayor a 255)
string name = "archivo.txt"; // Nombre del archivo a cifrar

// Configura tus rutas
string filePath = @"ruta/al/archivo/original";
string pathEncryption = @"ruta/para/archivos/cifrados";
string pathDecryption = @"ruta/para/archivos/descifrados";
```

### Ejecución

```bash
dotnet run
```

El programa:
1. Genera las claves RSA
2. Cifra el archivo especificado
3. Descifra el archivo cifrado
4. Muestra el tiempo de ejecución en milisegundos

## Ejemplo de Uso

```csharp
RSA_Encryptor encryptor = new RSA_Encryptor();
int p = 61, q = 53;

// Generar claves
var keys = encryptor.GenerateKeys(p, q);

// Cifrar
encryptor.Encrypt(filePath, fileName, pathEncryption, keys.e, keys.n);

// Descifrar
encryptor.Decrypt(encryptedFile, fileName, pathDecryption, keys.d, keys.n);
```

## Notas Importantes

- **Seguridad**: Esta implementación es educativa. Para uso en producción, se recomienda usar bibliotecas criptográficas estándar.
- **Tamaño de Claves**: Los números primos p y q determinan el tamaño de la clave. `p * q` debe ser mayor a 255 para poder cifrar todos los valores de byte posibles.
- **Rendimiento**: El cifrado RSA es computacionalmente costoso. Para archivos grandes, considera usar cifrado híbrido (RSA + AES).

## Algoritmo RSA

El algoritmo implementa los siguientes pasos:

1. **Selección de primos**: Se eligen dos números primos p y q
2. **Cálculo de n**: n = p × q
3. **Cálculo de φ(n)**: φ(n) = (p-1) × (q-1)
4. **Selección de e**: Se encuentra un número primo e tal que 1 < e < φ(n) y MCD(e, φ(n)) = 1
5. **Cálculo de d**: Se calcula d tal que (d × e) mod φ(n) = 1
6. **Clave pública**: (e, n)
7. **Clave privada**: (d, n)

### Cifrado
```
C = M^e mod n
```

### Descifrado
```
M = C^d mod n
```

Donde:
- M = Mensaje original
- C = Mensaje cifrado
- e = Exponente público
- d = Exponente privado
- n = Módulo

## Licencia

Este proyecto es de código abierto y está disponible para fines educativos.

## Contribuciones

Las contribuciones son bienvenidas. Por favor:
1. Haz un fork del proyecto
2. Crea una rama para tu característica (`git checkout -b feature/nueva-caracteristica`)
3. Haz commit de tus cambios (`git commit -am 'Agrega nueva característica'`)
4. Push a la rama (`git push origin feature/nueva-caracteristica`)
5. Abre un Pull Request
