# 📦 Pallets API

API RESTful desarrollada en .NET 8 con Entity Framework Core y PostgreSQL para gestión de pallets y productos en planta.

## 🚀 Instalación y ejecución

1. Clonar el repositorio:
    ```bash
    git clone https://github.com/maurih5/pallets-api-core
    cd pallets-api-core
    ```

2. Configurar la cadena de conexión editando `appsettings.Development.json` y renombrandolo a `appsettings.json`:
    ```json
    {
      "ConnectionStrings": {
        "EscorialPostgreSql": "Host=localhost;Database=escorial_db;Username=usuario;Password=contraseña"
      }
    }
    ```

3. Restaurar paquetes y ejecutar:
    ```bash
    dotnet restore
    dotnet run
    ```

## 📚 Endpoints disponibles

- **POST** `/api/login`
    ```json
    {
      "user": "usuario",
      "password": "contraseña"
    }
    ```
  Respuesta: datos del empleado autenticado o `404 Not Found`.

- **GET** `/api/pallets?numero=<codigoPallet>`  
  Retorna datos del pallet por su código.

- **GET** `/api/pallets/productos?numero=<codigoPallet>`  
  Retorna los productos asociados a un pallet.

- **POST** `/api/pallets/asociar-productos`
    ```json
    {
      "codigo": "PALLET001",
      "usuario": "usuarioLogueado",
      "products": [
        {
          "serial": 12345,
          "productId": 1,
          "deleted": false
        },
        {
          "serial": 54321,
          "productId": 2,
          "deleted": true
        }
      ]
    }
    ```
  Respuesta: `204 No Content` o mensaje de error.

## ✍️ Autor

Mauricio
