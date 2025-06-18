# Timmer de Control de llamada basado en Eventos de 3CX

Este proyecto está diseñado para gestionar eventos de llamadas de 3CX a través de un webhook. Cuando se produce un evento, los datos correspondientes son enviados a un endpoint de webhook configurado previamente. A continuación se describe la estructura del JSON que contiene los datos del evento, los campos y su tipo, así como el archivo de configuración necesario para que el sistema funcione correctamente.

## Archivo de Configuración

El archivo de configuración se utiliza para establecer las variables de entorno necesarias para la integración con el sistema 3CX. Las siguientes variables se deben configurar en el entorno de Docker para que el sistema funcione correctamente.

### Variables de configuración

- **`ConnectionStrings__DefaultConnection`**:  
  - Tipo: `string`  
  - Descripción: Cadena de conexión a la bbdd MySql donde estan los clientes y el tiempo disponible

- **`Config3CX__Domain`**:  
  - Tipo: `string`  
  - Descripción: dominio y puerto de la 3CX en formato dominio:puerto (no incluir protocolo)

- **`Config3CX__ClientId`**:  
  - Tipo: `string`  
  - Descripción: Client ID generado en 3CX

- **`Config3CX__Secret`**:  
  - Tipo: `string`  
  - Descripción: Secret generado en 3CX

- **`Config3CX__CFDEndCall`**:  
  - Tipo: `string`  
  - Descripción: Call flow al que se hace un routeto cuando se acaba el tiempo

- **`Config3CX__CFDInitialNoTime`**:  
  - Tipo: `string`  
  - Descripción: Call flow al que se hace un routeto cuando se inicia una llamada sin el cliente creado o el tiempo asignado

- **`Config3CX__MinExtension`**:  
  - Tipo: `int`  
  - Descripción: Extension minima que se controla

- **`Config3CX__MaxExtension`**:  
  - Tipo: `int`  
  - Descripción: Extension maxima que se controla

### Variables de Entorno

Asegúrate de definir las siguientes variables de entorno en tu contenedor Docker para que la integración con 3CX se realice correctamente:

```bash
ConnectionStrings__DefaultConnection=XXXXX
Config3CX__Domain=dominio.my3cx.es:5001
Config3CX__ClientId=XXXXXXX
Config3CX__Secret=XXXXXXX
Config3CX__CFDEndCall=*99
Config3CX__CFDInitialNoTime=*99
Config3CX__MinExtension=3000
Config3CX__MaxExtension=3999
```
