Docker
docker-compose up --build -d webapi

1. Login (Para obtener el Token)
Endpoint: POST /api/Auth/login

JSON

{
  "userName": "admin",
  "password": "123456"
}
2. Crear Producto (Componentes)
Endpoint: POST /api/Products (Recuerda el ID que te devuelve, normalmente será el 1 si es el primero).

JSON

{
  "name": "Espada de Hierro",
  "sku": "SWORD-001",
  "price": 150.00
}

3. Ajustar Stock (Inventario)
Endpoint: POST /api/Inventory/adjust (Asegúrate de que productId sea el mismo que creaste arriba).

JSON

{
  "productId": 1,
  "quantity": 50,
  "userId": 1,
  "reason": "Compra de lote inicial"
}
Nota: Si quieres restar inventario (una venta), solo pon la cantidad en negativo (ej. "quantity": -5).