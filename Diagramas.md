# 1. Diagrama de Flujo de Arquitectura (Nivel Alto)
Este diagrama muestra cómo viaja una petición (Request) desde que el usuario la hace hasta que llega a la base de datos y regresa.
```mermaid
  graph TD
    %% Estilos
    classDef client fill:#f9f,stroke:#333,stroke-width:2px;
    classDef api fill:#bbf,stroke:#333,stroke-width:2px;
    classDef app fill:#dfd,stroke:#333,stroke-width:2px;
    classDef infra fill:#fdd,stroke:#333,stroke-width:2px;
    classDef db fill:#ff9,stroke:#333,stroke-width:2px;

    Client((Cliente / Frontend)):::client
    
    subgraph "Capa WebApi (Presentación)"
        Controller[Controller Endpoint]:::api
        Auth[Auth Middleware / JWT]:::api
        Presenter[Presenter / ViewModel]:::api
    end

    subgraph "Capa Application (Lógica de Negocio)"
        Interactor[Interactor / Use Case]:::app
        Validation[Validaciones de Negocio]:::app
    end

    subgraph "Capa Infrastructure (Datos)"
        Repo[Repository Implementation]:::infra
        SQLClient[SqlClient Wrapper]:::infra
    end

    Database[(SQL Server)]:::db

    %% Flujo
    Client -->|1. HTTP Request + Token| Auth
    Auth -->|2. Petición Válida| Controller
    Controller -->|3. Llama al Caso de Uso| Interactor
    Interactor -->|4. Valida Reglas| Validation
    Validation -->|5. Solicita Datos| Repo
    Repo -->|6. Ejecuta Query/SP| SQLClient
    SQLClient -->|7. Lee/Escribe| Database
    Database -- 8. Retorna Datos --> SQLClient
    SQLClient -- 9. Retorna Entidades --> Repo
    Repo -- 10. Retorna Resultado --> Interactor
    Interactor -- 11. Entrega Datos Procesados --> Presenter
    Presenter -- 12. Formatea JSON --> Controller
    Controller -- 13. HTTP 200 OK --> Client
```

# 2. Diagrama de Flujo del Proceso "Ajuste de Inventario"
Este diagrama detalla la lógica específica del endpoint más importante (/api/Inventory/adjust), mostrando las decisiones que toma el código.
```mermaid
flowchart TD
    Start([Inicio: POST /adjust]) --> Token{¿Token Válido?}
    
    Token -- No --> Error401[Retornar 401 Unauthorized]
    Token -- Sí --> ValidateInput{"¿Datos Completos?<br>(Producto, Cantidad, Razón)"}
    
    ValidateInput -- No --> Error400[Retornar 400 Bad Request]
    ValidateInput -- Sí --> GetStock[Consultar Stock Actual en DB]
    
    GetStock --> Exists{¿Existe Producto?}
    Exists -- No --> Error404[Retornar 404 Not Found]
    
    Exists -- Sí --> CheckType{¿Tipo Movimiento?}
    
    CheckType -- "Entrada (+)" --> UpdateStock[Sumar al Stock]
    CheckType -- "Salida (-)" --> CheckNegative{¿Stock Suficiente?}
    
    CheckNegative -- No --> ErrorBusiness[Retornar Error: Stock Insuficiente]
    CheckNegative -- Sí --> UpdateStock
    
    UpdateStock --> Transac[Iniciar Transacción SQL]
    Transac --> LogMove[Registrar en InventoryMovements]
    LogMove --> SaveStock[Actualizar Tabla Products/Inventory]
    SaveStock --> Commit[Commit Transacción]
    
    Commit --> Success([Retornar 200 OK + Nuevo Stock])
    
    ErrorBusiness --> End([Fin])
    Error401 --> End
    Error400 --> End
    Error404 --> End
    Success --> End
```

# 3. Diagrama Entidad-Relación (Base de Datos)
Este diagrama modela la estructura física de tu base de datos SQL Server, mostrando las tablas principales (Users, Products, Inventory) y cómo se relacionan a través de claves foráneas (FK) en el historial de movimientos (InventoryMovements).
```mermaid
erDiagram
    USERS {
        int Id PK
        string UserName
        string PasswordHash
        string Role "Ej: Admin, User"
    }

    PRODUCTS {
        int Id PK
        string Name
        string SKU
        decimal Price
    }

    INVENTORY {
        int Id PK
        int ProductId FK
        int Quantity "Stock Actual"
        datetime LastUpdated
    }

    INVENTORY_MOVEMENTS {
        int Id PK
        int ProductId FK
        int UserId FK
        int Quantity "Positivo o Negativo"
        string Reason "Ej: Venta, Compra"
        datetime MovementDate
    }

    %% Relaciones
    PRODUCTS ||--|| INVENTORY : "tiene un registro de"
    PRODUCTS ||--o{ INVENTORY_MOVEMENTS : "registra movimientos"
    USERS ||--o{ INVENTORY_MOVEMENTS : "autoriza"
```
# Diagramas de Arquitectura C4
El modelo C4 (Context, Containers, Components, Code) es el estándar de la industria para documentar arquitectura de software. Es ideal para tu manual técnico porque va desde lo general (Contexto) hasta lo específico (Código).

Basado en la estructura de tu repositorio (.NET 8, Docker, Clean Architecture), he diseñado los tres niveles más importantes para tu documentación.

Puedes copiar estos códigos en [Mermaid Live Editor](https://mermaid.live/) para generar las imágenes en alta calidad.

## Nivel 1: Diagrama de Contexto (System Context)
Vista de "gran altura". Muestra quién usa el sistema y con qué otros sistemas externos interactúa.

```mermaid
C4Context
    title Diagrama de Contexto - Sistema de Inventarios
    
    Person(user, "Usuario", "Almacenista o Administrador del sistema.")
    
    System(inventorySys, "Sistema de Inventarios API", "Permite gestionar productos, realizar ajustes de stock y administrar usuarios.")
    
    System_Ext(monitoring, "Plataforma de Observabilidad", "Seq (Logs), Prometheus (Métricas) y Grafana (Dashboards).")
    
    Rel(user, inventorySys, "Utiliza", "HTTPS / Swagger")
    Rel(inventorySys, monitoring, "Envía telemetría", "HTTP / TCP")
```

Nivel 2: Diagrama de Contenedores (Containers)
Vista de "infraestructura". Muestra cómo se despliega el software utilizando Docker. Este es vital para tu proyecto dado que usas docker-compose.
```mermaid
C4Container
    title Diagrama de Contenedores - Arquitectura Docker
    
    Person(user, "Usuario", "Cliente HTTP / Postman / Swagger UI")

    Container_Boundary(c1, "Docker Compose Network") {
        Container(web_api, "Web API (.NET 8)", "C#, ASP.NET Core", "Provee endpoints REST, maneja lógica de negocio y seguridad (JWT).")
        
        ContainerDb(db, "SQL Server 2022", "Microsoft SQL Container", "Almacena tablas de Usuarios, Productos y Movimientos.")
        
        Container(seq, "Seq", "Docker Image", "Servidor de log centralizado y estructurado.")
        
        Container(prom, "Prometheus", "Docker Image", "Recolector de métricas (Scraper).")
        
        Container(graf, "Grafana", "Docker Image", "Visualizador de métricas y alertas.")
    }

    Rel(user, web_api, "Realiza peticiones", "JSON/HTTPS")
    
    Rel(web_api, db, "Lee y escribe datos", "ADO.NET / SQL Client (Puerto 1433)")
    Rel(web_api, seq, "Envía logs de aplicación", "HTTP (Puerto 5341)")
    
    Rel(prom, web_api, "Scrape /metrics", "HTTP")
    Rel(graf, prom, "Consulta datos (PromQL)", "HTTP")
```

Nivel 3: Diagrama de Componentes (Components)
Vista de "código". Muestra cómo está organizada tu API internamente, respetando tu estructura de carpetas (CleanArch, Infra, WebApi).
```mermaid
C4Component
    title Diagrama de Componentes - Estructura Interna (Clean Architecture)

    Container(api, "Web API", ".NET 8", "Punto de entrada")

    Container_Boundary(api_internals, "Lógica de Aplicación") {
        
        Component(controllers, "Controllers", "WebApi Layer", "Recibe HTTP Requests, valida DTOs y llama a los Interactores.")
        
        Component(interactors, "Interactors (Use Cases)", "Application Layer", "Orquesta la lógica de negocio (CQRS). Coordina validaciones y repositorios.")
        
        Component(presenters, "Presenters", "Application/Common", "Formatea la respuesta final para el cliente.")
        
        Component(repos, "Repositories", "Infrastructure Layer", "Implementación concreta del acceso a datos.")
        
        Component(sql_helper, "SQL Client Wrapper", "Infrastructure Layer", "Ejecuta comandos SQL puros y mapea resultados.")
    }

    ContainerDb(database, "Base de Datos", "SQL Server", "Persistencia")

    Rel(api, controllers, "Enruta petición")
    Rel(controllers, interactors, "Ejecuta (Mediator/Pipeline)")
    
    Rel(interactors, repos, "Usa interfaz de repositorio")
    Rel(repos, sql_helper, "Delega ejecución SQL")
    
    Rel(sql_helper, database, "Query / Stored Proc", "T-SQL")
    
    Rel(interactors, presenters, "Envía resultado crudo")
    Rel(presenters, controllers, "Devuelve ViewModel")
```
Nivel 4: Diagrama de Código (Code / Class Diagram)
Vista de "microscopio". Muestra las clases del Dominio, sus atributos y relaciones directas. Es útil para que los desarrolladores entiendan las entidades principales sin leer todo el código.
```mermaid
classDiagram
    note "Diagrama de Código - Entidades del Dominio (Nivel 4)"
    
    class Product {
        +int Id
        +string Name
        +string Sku
        +decimal Price
        +Validate() Result
    }

    class User {
        +int Id
        +string UserName
        +string PasswordHash
        +int RoleId
        +IsAdmin() bool
    }

    class InventoryMovement {
        +int Id
        +int ProductId
        +int UserId
        +int Quantity
        +string Reason
        +DateTime MovementDate
        +IsEntry() bool
    }
    
    class Inventory {
        +int Id
        +int ProductId
        +int CurrentStock
        +DateTime LastUpdated
    }

    Product "1" -- "1" Inventory : tiene registro de
    Product "1" *-- "*" InventoryMovement : historial de
    User "1" --> "*" InventoryMovement : registra/autoriza
```