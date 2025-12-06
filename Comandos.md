Aquí tienes el contenido formateado y listo para copiar. Solo dale al botón de **Copiar** en la esquina del bloque y reemplaza todo el contenido de tu archivo `Comandos.md` con esto:

````markdown
# Guía de Ejecución y Pruebas de API

### 1. Levantar el Entorno
Ejecuta este comando en tu terminal (dentro de la carpeta `Docker/`) para compilar los cambios y reiniciar el servicio:

```bash
docker-compose up --build -d webapi
````

-----

### 2\. Iniciar Sesión (Login)

Obtén tu **Token JWT** para autorizarte en el sistema.

  * **Método:** `POST`
  * **Endpoint:** `/api/Auth/login`
  * **Acción:** Copia el token de la respuesta y pégalo en el botón **Authorize** (escribiendo ` Bearer  ` antes del código).

<!-- end list -->

```json
{
  "userName": "admin",
  "password": "123456"
}
```

-----

### 3\. Crear Producto Nuevo

Registra un nuevo ítem en la base de datos.

  * **Método:** `POST`
  * **Endpoint:** `/api/Products`
  * **Nota:** Fíjate en el `ID` que te devuelve la respuesta (usualmente será `1` si es el primero).

<!-- end list -->

```json
{
  "name": "Espada de Hierro",
  "sku": "SWORD-001",
  "price": 150.00
}
```

-----

### 4\. Ajustar Stock (Inventario)

Registra entradas o salidas de mercancía.

  * **Método:** `POST`
  * **Endpoint:** `/api/Inventory/adjust`
  * **Requisito:** El `productId` debe coincidir con el que creaste en el paso anterior.

#### Opción A: Compra (Sumar Stock)

Usa una cantidad **positiva**.

```json
{
  "productId": 1,
  "quantity": 50,
  "userId": 1,
  "reason": "Compra de lote inicial"
}
```

#### Opción B: Venta (Restar Stock)

Usa una cantidad **negativa**.

```json
{
  "productId": 1,
  "quantity": -5,
  "userId": 1,
  "reason": "Venta a cliente final"
}
```

```
```