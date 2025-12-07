# 🛠 Guía de Operaciones y Pruebas de API

Este documento detalla cómo levantar el entorno de desarrollo y los payloads JSON necesarios para probar los flujos principales del sistema.

---

## 🐳 Gestión del Entorno (Docker)

El sistema ahora está dividido en dos partes para facilitar el desarrollo. Ejecuta los comandos en el orden indicado.

### 1. Encender Infraestructura (Solo una vez)

Levanta los servicios base (Base de datos SQL, Seq, Grafana, Prometheus). Solo necesitas ejecutar esto si reinicias tu PC o si cambiaste configuración de infraestructura.

```bash
docker compose -f docker-compose-infra.yml up -d --build
```

### 2. Levantar la Aplicación (Uso Diario)

Utiliza este comando cada vez que hagas cambios en tu código C# (.NET) y quieras probarlos.

```bash
docker compose -f docker-compose-app.yml up -d --build
```

---

## 🧪 Flujo de Pruebas (Endpoints)

Sigue este orden para validar el funcionamiento del sistema.

### 1. Iniciar Sesión (Login)

Obtén tu Token JWT para autorizarte en el sistema.

- **Método:** `POST`
- **Endpoint:** `/api/Auth/login`
- **Acción:** Copia el token de la respuesta y pégalo en el botón **Authorize** de Swagger (escribiendo `Bearer` antes del código).

```json
{
  "userName": "admin",
  "password": "123456"
}
```

### 2. Crear Producto Nuevo

Registra un nuevo ítem en la base de datos.

- **Método:** `POST`
- **Endpoint:** `/api/Products`
- **Nota:** Fíjate en el `id` que te devuelve la respuesta (usualmente será `1` si es el primero).

```json
{
  "name": "Espada de Hierro",
  "sku": "SWORD-001",
  "price": 150.00
}
```

### 3. Ajustar Stock (Inventario)

Registra entradas o salidas de mercancía.

- **Método:** `POST`
- **Endpoint:** `/api/Inventory/adjust`
- **Requisito:** El `productId` debe coincidir con el que creaste en el paso anterior.

#### Opción A: Compra (Sumar Stock)
Usa una cantidad positiva para agregar inventario.

```json
{
  "productId": 1,
  "quantity": 50,
  "userId": 1,
  "reason": "Compra de lote inicial"
}
```

#### Opción B: Venta (Restar Stock)
Usa una cantidad negativa para reducir inventario.

```json
{
  "productId": 1,
  "quantity": -5,
  "userId": 1,
  "reason": "Venta a cliente final"
}
```