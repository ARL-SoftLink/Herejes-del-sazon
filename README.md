# Herejes del Sazón – Documentación del proyecto

## Descripción general

**Herejes del Sazón** es un restaurante familiar con sede exclusiva en Centroamérica que desafía los platos convencionales utilizando únicamente ingredientes originarios de la región. Su carta no está definida de forma rígida, sino que evoluciona constantemente para ofrecer experiencias culinarias únicas sin perder la esencia de que toda la familia pueda disfrutar junta. El sistema web aquí documentado es la plataforma digital que acompañará este concepto innovador.

## Objetivo

Crear una experiencia digital que refleje la filosofía del restaurante: ofrecer platos fuera de lo común (herejes) pero diseñados para que ningún miembro de la familia quede excluido. El sitio web debe permitir a los comensales explorar un menú dinámico, conocer el origen de los ingredientes centroamericanos y realizar pedidos (o reservas) de forma intuitiva, todo ello manteniendo un tono cálido, rebelde y familiar.

## Problema que aborda

En Centroamérica, la mayoría de restaurantes familiares ofrecen cartas predecibles que repiten los mismos platillos típicos sin innovar, o bien existen propuestas de autor que resultan inaccesibles para niños, adultos mayores o personas con paladares no aventureros. Además, hay una desconexión entre el consumidor y los ingredientes locales auténticos. El problema es doble: **falta de osadía culinaria** en el segmento familiar y **poca visibilidad** de la riqueza gastronómica centroamericana.

## Solución propuesta

Un sistema web que centralice la propuesta de **Herejes del Sazón** con las siguientes características:

- **Carta viva**: el menú no es fijo; se actualiza periódicamente manteniendo tres “especiales de la casa” y una base rotativa de platos, postres y bebidas (todos con ingredientes 100% centroamericanos).
- **Filtro familiar**: cada platillo se etiqueta como “apto para niños”, “textura suave”, “sin picante” o “aventurero”, permitiendo a las familias elegir sin miedo.
- **Historias de ingredientes**: cada producto tiene una pequeña ficha que narra su origen regional y su productor local.
- **Reservas y pedidos**: interfaz sencilla para reservar mesas o solicitar comida a domicilio, integrando métodos de pago populares en la región.

## Funcionalidades principales (desde el punto de vista del usuario)

1. **Ver el menú completo** con imágenes, descripciones y precios.
2. **Filtrar platos por categoría** (especiales, platos variados, postres, bebidas) y por perfil familiar (sin picante, suave, crunchy, etc.).
3. **Consultar información de ingredientes** y su procedencia centroamericana.
4. **Realizar reservaciones** eligiendo fecha, hora y número de comensales.
5. **Hacer pedidos a domicilio** con seguimiento en tiempo real.
6. **Registrarse / iniciar sesión** para guardar pedidos favoritos o historial.
7. **Dejar reseñas y calificaciones** de los platillos probados.

## Elemento diferenciador

Mientras otros restaurantes familiares apuestan por la seguridad de los platos clásicos, **Herejes del Sazón** se atreve a ser hereje sin excluir a nadie. Su sistema web no solo muestra una carta, sino que **educa y conecta** al comensal con los productores centroamericanos a través de microhistorias. Además, el menú nunca es estático: los propios clientes pueden sugerir combinaciones de ingredientes locales, y el restaurante las prueba en “noches de herejía colectiva”. El sitio web actúa como un puente colaborativo entre la cocina y la familia.

## Impacto esperado

- **Cultural**: Revalorizar los ingredientes centroamericanos y combatir la idea de que la cocina familiar debe ser aburrida o repetitiva.
- **Económico**: Generar una comunidad de pequeños productores locales que provean al restaurante, impulsando la economía regional.
- **Social**: Que las familias redescubran el placer de comer juntas platos diferentes, rompiendo la rutina gastronómica sin conflictos generacionales.
- **Digital**: Convertir el sitio web en un referente de diseño inclusivo y contenido gastronómico interactivo en Centroamérica, inspirando a otros emprendedores.
---

# Despliegue de Herejes Sazon

Esta guía te permitirá levantar el entorno de desarrollo y producción de la aplicación web **Herejes Sazon** (C# MVC) utilizando contenedores Docker.

## Prerrequisitos

Asegúrate de tener instalado en tu máquina:

- [Docker](https://docs.docker.com/get-docker/) (Motor de contenedores, necesario para construir la imagen y para el driver de Minikube).
- [Git](https://git-scm.com/) (para clonar el repositorio)
- **kubectl** (CLI para controlar Kubernetes) (pasos de instalacion incluida en esta guia).
- **Minikube** (Clúster local de Kubernetes)(pasos de instalacion incluida en esta guia).
- **Helm** (Gestor de paquetes para Kubernetes)(pasos de instalacion incluida en esta guia).

## Configuración del Entorno

1. **Clona el repositorio dentro de tu terminal WSL (no en PowerShell/CMD ni en una ruta de Windows):**

   > ⚠️ Por defecto, `git clone` ejecutado desde PowerShell, CMD o herramientas gráficas (GitHub Desktop, el explorador de archivos) termina guardando el proyecto en una ruta de Windows, típicamente `C:\Users\TuUsuario\...`. Para este proyecto **eso rompe el flujo**: todos los pasos siguientes (`kubectl`, `minikube`, `docker`, `start.sh`) se ejecutan desde WSL, y acceder a un proyecto que vive en `C:\` desde dentro de WSL (a través de `/mnt/c/...`) es notablemente más lento y puede causar errores extraños de permisos y de finales de línea. Si ya clonaste el repo en una ruta de Windows, vuelve a clonarlo siguiendo estos pasos en vez de reutilizar esa copia.

   Abre tu terminal WSL (busca "Ubuntu" en el menú Inicio, o desde PowerShell ejecuta `wsl`) y, **ya dentro de Linux**, clona el repositorio en tu carpeta personal:

```bash
   cd ~
   mkdir -p proyectos && cd proyectos
   git clone https://github.com/ARL-SoftLink/Herejes-del-sazon.git
   cd herejes_del_sazon
```

   Tu proyecto queda entonces en algo como `/home/tu-usuario/proyectos/herejes_del_sazon` — dentro del sistema de archivos de WSL, no en `C:\`.

   > **Nota:** puedes seguir editando con una interfaz gráfica normal. Con el repo ya clonado en WSL, ejecuta `code .` desde esa misma terminal para abrir VS Code conectado remotamente a WSL (verás "WSL: Ubuntu" en la esquina inferior izquierda). La terminal integrada de VS Code, en ese modo, ya es una terminal de WSL.

2. **Variables de entorno:** Crea un archivo .env en la raíz del proyecto para sobrescribir la configuración por defecto (como cadenas de conexión o puertos).
```bash
DATABASE_HOST=host.docker.internal
DATABASE_PORT=5432
POSTGRES_USER=tu_usuario
POSTGRES_PASSWORD=tu_contra_segura
POSTGRES_DB=HerejesSazonBD
```
### Configuracion de parametros clave para docker desktop(usuarios windows)

> ⚠️ Este paso es un requisito previo a la sección "Instalación de herramientas". Sin esto, `minikube start --driver=docker` fallará o se quedará colgado en Windows.

1.**Instala Docker Desktop** desde [docker.com](https://www.docker.com/products/docker-desktop/) si aún no lo tienes.

2.**Activa el motor basado en WSL2:**
Abre Docker Desktop → `Settings` → `General` → marca la casilla **"Use the WSL 2 based engine"**.

3.**Activa la integración con tu distribución de WSL:**
Ve a `Settings` → `Resources` → `WSL Integration` → activa el interruptor junto a tu distribución (por ejemplo, `Ubuntu`) → clic en **Apply & Restart**.
Esto es lo que permite que el comando `docker` (y por lo tanto Minikube) funcione dentro de tu terminal WSL.

4.**Asigna recursos según tu hardware real (no copies un valor a ciegas):**

Antes de tocar nada, revisa cuánta RAM y núcleos tiene tu máquina físicamente (Windows, no WSL): `Configuración` → `Sistema` → `Acerca de`, o en PowerShell:
```powershell
systeminfo | findstr /C:"Total Physical Memory"
echo $env:NUMBER_OF_PROCESSORS
```

> ⚠️ **No asignes a WSL2 más recursos de los que tu máquina tiene físicamente.** Si tu equipo tiene 4GB de RAM y le pides `memory=6GB`, no "consigues" memoria extra: Windows y WSL2 empiezan a competir por RAM insuficiente, todo el sistema se vuelve extremadamente lento, y en algunos casos WSL2 ni siquiera arranca. La regla es dejar como mínimo un 40-50% de la RAM y al menos 1-2 núcleos libres para Windows y para el propio Docker Desktop (que también consume recursos en segundo plano, incluso en reposo).

Usa esta tabla como referencia:

| RAM total del equipo | Núcleos totales | `.wslconfig` recomendado |
|---|---|---|
| 16GB o más | 8+ | `memory=6GB`, `processors=4` |
| 8GB | 4 | `memory=4GB`, `processors=2` |
| 4GB o menos | ≤4 | *(ver nota abajo)* |

Crea o edita `%UserProfile%\.wslconfig`:
```ini
[wsl2]
memory=4GB
processors=2
```
Guarda el archivo y reinicia WSL desde PowerShell:
```powershell
wsl --shutdown
```
Vuelve a abrir tu terminal WSL para que los cambios apliquen.

> **Si tu equipo tiene 4GB de RAM o menos:**
> Sé honesto contigo mismo antes de invertir tiempo aquí: un clúster de PostgreSQL con 3 réplicas (CNPG) + el operador + Docker Desktop + la webapp, todo corriendo a la vez sobre 4GB, va a ser lento en el mejor de los casos, e inestable en el peor. Dos alternativas mejores que forzar el `.wslconfig`:
> - **Reduce `instances: 3` a `instances: 1`** en el manifiesto del clúster (`k8s/postgresql-cluster.yaml.template`) solo en esta máquina, para poder desarrollar sin failover local (perderás la posibilidad de probar el failover ahí, pero podrás trabajar en el resto de la app).
> - **Usa GitHub Codespaces** para este proyecto en vez de Minikube local — los recursos ya están garantizados por el tipo de máquina que elijas al crear el Codespace, sin pelear con los límites de tu laptop.
> Si aun así quieres intentarlo localmente, no superes `memory=2GB`, `processors=2`, y cierra el resto de aplicaciones (especialmente el navegador) antes de levantar el entorno.

5.**Verifica la conexión desde WSL:**
```bash
docker info > /dev/null && echo "Docker OK"
```
Si este comando falla, revisa los pasos 2 y 3 antes de continuar.

> **Nota:**
> Si al ejecutar `minikube start` ves errores relacionados con Hyper-V o con el driver, confirma que no tengas otro hipervisor (VirtualBox, Hyper-V standalone) interfiriendo; con `--driver=docker` no se necesita ninguno, porque Minikube crea un contenedor, no una máquina virtual aparte.

### Instalacion de herramientas (ejecutar en WSL o windows Nativo)

> ⚠️ INSTRUCCIÓN IMPORTANTE PARA USUARIOS WINDOWS
> **Todos los comandos de instalación de `kubectl`, `Minikube` y `Helm` deben ejecutarse DENTRO de tu terminal WSL (Windows Subsystem for Linux)**, por ejemplo, en tu distribución Ubuntu/Debian. 
> **No uses PowerShell ni CMD** para estos pasos, ya que los comandos `sudo`, `install` y los scripts bash no son nativos de Windows.

Copia y pega los siguientes bloques en tu terminal WSL/Linux:

1.**Instalar kubectl:**
```bash
curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl"
sudo install -o root -g root -m 0755 kubectl /usr/local/bin/kubectl
``` 
2.**Instalar minukube**
```bash
curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube-linux-amd64
sudo install minikube-linux-amd64 /usr/local/bin/minikube
``` 
3.**Instalar Helm**
```bash
curl https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3 | bash
```
4. **Verificar que todo esta instalado:**
```bash
kubectl version --client
minikube version
helm version
```
5.**Iniciar el cluster de MiniKube**
```bash 
minikube start --driver=docker
```
6. **Instalar el operador CloudNativePG** 
```bash
helm repo add cloudnative-pg https://cloudnative-pg.github.io/charts
helm repo update
helm upgrade --install cnpg cloudnative-pg/cloudnative-pg \
  --namespace cnpg-system --create-namespace

# Esperar a que el operador esté disponible
kubectl wait --for=condition=Available deployment --all -n cnpg-system --timeout=180s
```
> **Nota:**
> Este ultimo paso está incluido dentro del script shell encargado de levantar toda la app, hacerlo de todas formas no afecta en nada el despliegue 

## Conceder permisos de ejecucion
En la terminal WSL con la ruta apuntando a la raiz del proyecto, escribe y ejecuta estos comandos
```bash
# otorgar permiso a: entrypoint.sh
chmod +x entrypoint.sh
# permiso para start.sh
chmod +x start.sh
# permiso para stop.sh
chmod +x stop.sh
```
> **Nota:** Si estas en VS code confirma que la terminal integrada pertenece a WSL y no a windows 

## Iniciar la app
En esa misma terminal WSL apuntando hacia la raiz del proyecto, escribe y ejecuta
1. **levantar la app:**
```bash
./start.sh
```
2. **Detener la app:**
```bash
./stop.sh
```
> Importante: nunca omitas el `./` de lo contrario la terminal arroja: `No such file ...`.

---
## 🍽️ Menú base

A continuación se listan los platillos que representan la identidad de **Herejes del Sazón**.  
Todos elaborados exclusivamente con ingredientes centroamericanos.

### 🔥 Especiales de la casa (3 platos fuertes)

1. **El Hereje Volcánico**  
   Lomo de cerdo adobado en salsa de chiltepín y achiote, acompañado de puré de malanga, plátano macho asado y ensalada de pacaya.

2. **Santa Herejía del Mar**  
   Filete de pargo rojo envuelto en hoja de plátano, bañado en leche de coco y jengibre, servido con arroz de palmito y vegetales típicos (güisquil, loroco, chipilín).

3. **La Rebelión de la Tierra**  
   Cazuela de gallina criolla con masa de maíz quebrado, ayote, ayote tierno, chilacayote, y hierbas olorosas (apazote, yerba mora). Acompañado de chirmol asado.

---

### 🍛 Platos variados (15 opciones)

1. **Pepián de res ahumado** – Espeso guiso con semillas de calabaza, tomate, chile cobán y res ahumada.
2. **Sopa de pescado con limón y jengibre** – Sopa clara con pescado blanco, yuca, camote y hierba luisa.
3. **Enchiladas cremosas de loroco** – Tortillas de maíz azul, frijoles licuados, crema, queso duro y loroco salteado.
4. **Tamales de chipilín y pollo** – Masa de maíz nixtamalizado envuelta en hoja de banano, con chipilín, pollo desmenuzado y salsa de tomate.
5. **Rondón costeño** – Leche de coco, pescado, camarones, yuca, ñame, plátano verde y cúrcuma.
6. **Churrasco hereje** – Carne de res marinada en naranja agria, ajo y comino, asada a la parrilla con chimichurri de culantro coyote.
7. **Arroz a la marinera centroamericano** – Arroz con calamares, mejillones, pimiento rojo, chile dulce y cilantro.
8. **Relleno de papa con carne mechada** – Papa rellena con carne mechada en salsa de tomate y especias, frita y servida con curtido de repollo.
9. **Submarino de platano** – Baguette relleno de plátano maduro horneado, frijoles molidos, queso fresco y chorizo criollo.
10. **Ceviche de concha negra** – Concha negra cocida en jugo de limón, cebolla morada, chile panameño, cilantro y jengibre.
11. **Chilaquiles centroamericanos** – Totopos de maíz bañados en salsa roja de tomate y chile guaque, crema, queso rallado y cebolla encurtida.
12. **Sopa de frijoles con guineo** – Frijoles rojos enteros con trozos de guineo verde, cilantro, comino y chile dulce.
13. **Pupusas de ayote y chipilín** – Masa de maíz rellena de ayote rallado y chipilín, acompañadas de curtido y salsa de tomate asado.
14. **Desayuno hereje (todo el día)** – Huevos pericos (con tomate y cebolla), frijoles parados, queso fresco, plátano maduro frito y tortillas de harina de trigo local.
15. **Chancho con yuca** – Cerdo adobado en naranja agria, ajo y orégano, cocido lentamente con yuca y servido con chicharrón crujiente y salsa de tomate.

---

### 🍰 Postres

1. **Tres leches de cacao** – Bizcocho empapado en tres leches con cacao nibs y crema batida de vainilla.
2. **Dulce de papaya con canela** – Papaya verde cocida en panela con ramas de canela y clavo de olor.
3. **Arroz con leche de coco y pasas** – Arroz cocido en leche de coco, endulzado con rapadura, pasas y canela en polvo.
4. **Pastel de banano y jengibre** – Masa húmeda de banano maduro, jengibre fresco y nuez de macadamia, cubierto con glaseado de miel.
5. **Chilacayote en miel** – Tiras de chilacayote cocidas en miel de panela con canela y pimienta gorda.

---

### 🥤 Bebidas

1. **Refresco de jamaica con jengibre** – Flor de jamaica infusionada con jengibre fresco y endulzada con estevia.
2. **Chicha de maíz rojo** – Bebida fermentada suavemente (sin alcohol fuerte) de maíz rojo, canela y clavo de olor.
3. **Limonada con hierba luisa y miel** – Limón criollo, hierba luisa, miel de abeja local y agua mineral.
4. **Atol de elote tostado** – Elote tostado, leche, canela y un toque de vainilla, espeso y servido caliente.
5. **Horchata de morro y cacao** – Semillas de morro tostadas, arroz, cacao en polvo, canela y azúcar morena.
6. **Fresco de cas con guayaba** – Cas (fruta centroamericana) y guayaba licuados con agua y un toque de limón.
7. **Café de altura chorreado** – Café 100% de origen centroamericano (Honduras, Guatemala o Costa Rica), chorreado con filtro de tela.
8. **Tamalinda de tamarindo** – Pulpa de tamarindo, chía, agua de coco y un toque de chile en polvo (opcional).

---

## 🛠️ Tecnologías utilizadas

*Frontend:* add
*Backend:* add
*Base de datos:* PostgreSQL  
*Control de versiones:* Git + GitHub  
