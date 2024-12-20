# Prueba Técnica: Proyecto Lambda AWS

Este repositorio contiene el código y los archivos necesarios para la implementación de una Lambda en AWS que realiza operaciones CRUD sobre una tabla en una base de datos. A continuación, se detalla la estructura y los pasos para utilizar este proyecto.

## Archivos Incluidos

- `jacv_prueba_tecnica\jac_prueba\personas.sql`: Este archivo contiene el script necesario para crear la tabla y el procedimiento almacenado que implementa las operaciones CRUD.

## Endpoint del Servicio

La Lambda está expuesta a través de API Gateway y puede ser accedida mediante el siguiente URL:

```
https://2ngnoydav8.execute-api.us-east-2.amazonaws.com/sandbox/v1/jacv/persona
```

## Documentación del API

La documentación completa del API está disponible en Swagger. Puedes acceder a ella en el siguiente enlace:

[Documentación Swagger del API](https://bucket-test-paycash.s3.us-east-2.amazonaws.com/swagger/jacv/jacv_index.html)

## Operaciones Disponibles

El API permite realizar las siguientes operaciones CRUD:

1. **GET**: Obtiene los registros.
   - Endpoint: `/jacv/persona`

2. **POST**: Crea nuevos registros.
   - Endpoint: `/jacv/persona`

3. **PUT**: Actualiza registros existentes.
   - Endpoint: `/jacv/persona`

4. **DELETE**: Desactiva un registro.
   - Endpoint: `/jacv/persona`

## Instrucciones para Implementación

1. **Despliegue del Script SQL**:
   - Ejecuta el archivo `personas.sql` en tu base de datos para crear la tabla y el procedimiento almacenado necesarios.

2. **Configuración de AWS Lambda**:
   - Sube el código fuente a AWS Lambda.
   - Configura el entorno de ejecución, asegurándote de incluir las dependencias necesarias para conectarte a la base de datos.

3. **Configuración de API Gateway**:
   - Configura API Gateway para exponer los endpoints mencionados anteriormente.
   - Asegúrate de configurar los métodos HTTP y la integración con la Lambda correspondiente.

4. **Verificación del API**:
   - Utiliza la documentación Swagger para probar las operaciones del API y verificar que funcionan correctamente.

---

Si tienes alguna pregunta o necesitas asistencia adicional, no dudes en contactarme.

