#  **Crear un Self-Hosted Runner en GitHub (Linux)**

Un _self-hosted runner_ es una máquina (física o virtual) donde GitHub ejecuta tus workflows de Actions.  
A diferencia de los runners de GitHub, este es **tuyo**, tú lo administras, y corre dentro de tu red o servidor.

----------

#  **1. Crear un Runner desde GitHub**

1.  Entra a tu repositorio en GitHub.
    
2.  Ve a:  
    **Settings → Actions → Runners**
    
3.  Clic en el botón:  
    **“New self-hosted runner”**
    
4.  Selecciona el sistema operativo:  
    **Linux**
    
5.  Selecciona la arquitectura:  
    **x64**
    
6.  GitHub te mostrará exactamente los comandos que debes ejecutar en tu servidor Linux.
    

Tu pantalla debe verse similar a esta (como la imagen que compartiste):

> Muestra instrucciones para descargar el paquete, descomprimirlo y configurarlo.

----------

#  **2. Preparar tu servidor Linux**

Abre una terminal en la máquina Linux donde instalarás el runner.

Asegúrate de tener:

```bash
sudo apt update
sudo apt install -y curl tar bash

```

(Estas herramientas son necesarias para instalar el runner).

----------

#  **3. Descargar el paquete del Runner**

Ejecuta los comandos exactamente como los muestra GitHub:

```bash
# Crear carpeta para el runner
mkdir actions-runner && cd actions-runner

```

Luego descarga la versión más reciente del runner:

```bash
curl -o actions-runner-linux-x64-2.329.0.tar.gz -L \
https://github.com/actions/runner/releases/download/v2.329.0/actions-runner-linux-x64-2.329.0.tar.gz

```

_(El número de versión podría cambiar, usa el que GitHub te muestre)._

----------

#  **4. (Opcional) Validar el hash**

GitHub te da el hash para validar que la descarga no está corrupta.  
Ejemplo:

```bash
echo "1f491ef4eb0a9728b7c96375c4688448814e5f99382a523d4512e5a3349102e1d  actions-runner-linux-x64-2.329.0.tar.gz" | shasum -a 256 -c

```

----------

#  **5. Extraer el runner**

```bash
tar xzf ./actions-runner-linux-x64-2.329.0.tar.gz

```

Esto deja listo el instalador en esa carpeta.

----------

#  **6. Configurar el Runner**

GitHub te muestra un comando como este:

```bash
./config.sh --url https://github.com/Raptor057/Practica-Sesion-6-1 --token XXXXXXXXXXXXXXXXXXXXXXXXXXXXX

```

 **Este comando registra tu servidor como runner de ese repo.**

Durante la configuración te pedirá:

-   Nombre del runner
    
-   Etiquetas (labels)
    
-   Si deseas que arranque como servicio (puedes dar _yes_)
    

**Recomendación:**  
Usa labels como:

```
linux
devops-class
docker
self-hosted

```

Así podrás dirigir tus workflows a este runner.

----------

#  **7. Iniciar el Runner**

Después de configurarlo, ejecuta:

```bash
./run.sh

```

Esto inicia el agente y queda escuchando trabajos de GitHub Actions.

Verás algo como:

```
√ Connected to GitHub
√ Waiting for Jobs...

```

----------

#  **8. Instalarlo como Servicio (Recomendado)**

Para no tener que iniciar el runner manualmente cada vez, instala el servicio:

```bash
sudo ./svc.sh install
sudo ./svc.sh start

```

Esto hace que el runner arranque automáticamente junto con el sistema.

----------

#  **9. Verificar en GitHub**

Vuelve a:

**Settings → Actions → Runners**

Deberías ver tu nuevo runner:

-   Online
    
-   Con tus etiquetas
    
-   Listo para ejecutar workflows
    

----------

#  **10. Ejemplo de Workflow que usa tu Runner**

```yaml
name: Test Runner

on:
  workflow_dispatch:

jobs:
  test:
    runs-on:
      - self-hosted
      - linux
      - devops-class

    steps:
      - name: Checkout repo
        uses: actions/checkout@v4

      - name: Ver info del runner
        run: |
          echo "Runner funcionando en:"
          uname -a
          docker --version
          docker compose version

```

Con esto podrás probar si tu runner funciona perfecto.

----------

#  **Conclusión**

Con esta guía aprendiste a:

✔ Crear y registrar un self-hosted runner  
✔ Configurarlo y ponerlo en línea  
✔ Instalarlo como servicio  
✔ Usarlo desde un workflow  
✔ Integrarlo a pipelines reales (como Docker, build, compose)