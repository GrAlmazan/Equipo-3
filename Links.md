## 🔗 Enlaces de Acceso (Entorno de Producción)

Aquí están los nuevos accesos a los servicios desplegados en el servidor Linux.

| Servicio | Descripción | URL de Acceso |
| :--- | :--- | :--- |
| **API (Swagger)** | Documentación segura vía NGINX (HTTPS) | [https://192.168.3.26/swagger](https://192.168.3.26/swagger) |
| **Grafana** | Tableros visuales de métricas | [http://192.168.3.26:3000](http://192.168.3.26:3000) |
| **Prometheus** | Recolección de métricas crudas | [http://192.168.3.26:9090](http://192.168.3.26:9090) |
| **Seq** | Logs de eventos y errores del sistema | [http://192.168.3.26:5341](http://192.168.3.26:5341) |
| **Uptime Kuma** | Monitoreo de estado (Up/Down) | [http://192.168.3.26:3001](http://192.168.3.26:3001) |

> **Nota Importante:** Al entrar a la API, es posible que el navegador muestre una advertencia de seguridad ("La conexión no es privada") debido a que usamos un certificado autofirmado. Debes dar clic en **"Configuración avanzada" -> "Continuar a 192.168.3.26 (no seguro)"**.