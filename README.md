## 1️⃣ Introducción

HealthManager es una plataforma web desarrollada en ASP.NET Core MVC que permite gestionar de forma digital los turnos médicos de un centro de salud.

El sistema fue diseñado con foco en la experiencia de usuario, automatización de tareas frecuentes, seguridad, autenticación y buenas prácticas de diseño y ejecución.

Este proyecto forma parte de mi portfolio profesional, con el objetivo de simular un entorno real de gestión médica.

## 💎 Funciones y características

Algunas de las funciones principales son las siguientes:

- Reservas y cancelaciones de turnos
- Autenticación y autorización basada en roles (Pacientes, Doctores y administradores)
- Gestión de registros médicos creados por los profesionales de la salud.
- Creación de registros de turnos mediante tareas en segundo plano.
- Pruebas unitarias y de integración.

## 👨‍💻 Tecnologías usadas

Backend: 

- .NET 10
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Mailkit
- QuestPDF

Frontend:

- Razor views
- Bootstrap
- jQuery

Testing:

- xUnit
- Testscontainers
- Docker

El proyecto incluye pruebas unitarias y de integración utilizando contenedores de Docker para aislar el entorno de testing.

## 📌 Problemas abordados

Durante el desarrollo se enfrentaron desafíos como:

- Evitar la generación duplicada de turnos
- Automatización de procesos recurrentes
- Aislamiento de pruebas de integración usando contenedores

## 🛠 Arquitectura y diseño

El proyecto implementa la arquitectura Modelo-Vista-Controlador (MVC), permitiendo una clara separación de responsabilidades y facilitando el mantenimiento y escalabilidad del sistema.

- Modelos: Representan las entidades del dominio y encapsulan la lógica de negocio asociada.
- Vistas: Interfaz visual con la cual interactúa el usuario. Representa los datos enviados por el controlador de manera visual.
- Controlador: Gestiona el flujo de la aplicación, procesa las solicitudes del usuario y coordina la interacción entre el modelo y la vista.

## 🗃 Modelos de datos

El sistema se compone de las siguientes entidades principales:

- Turnos: almacenan información referente a una consulta médica.
- Pacientes: Representan al usuario general de la aplicación.
- Administrador: Perfiles que permiten la creación de nuevos profesionales médicos en la aplicación.
- Doctor: Profesional encargado de gestionar las consultas y registros médicos.
- Specialty: información sobre una especialidad médica.
- DoctorShift: rango horario de atención del médico y duración de cada consulta
- WorkingDay: días en los que el médico atiende consultas, representados por un valor booleano.

## 📃 Lógica de turnos

Para la creación y gestión de los turnos durante el proceso de desarrollo, se tomaron las siguientes decisiones técnicas.

- La creación de los turnos médicos se hace de manera automatizada mediante tareas en segundo plano.
- La tarea se ejecuta al iniciar la aplicación y mantiene un rango dinámico de disponibilidad de 30 días hacia adelante. Se comprueba que existan turnos para el mes actual y se creen los turnos si no se cumple la condición.
- La generación de turnos y reservas está limitada a un periodo de 30 días para no sobrecargar el sistema con más peticiones a la base de datos.
- Se utilizan transacciones a nivel de base de datos para garantizar la consistencia de los datos ante posibles errores.
- Cada turno cuenta con propiedades de disponibilidad (reservado o disponible) así como una para expresar la asistencia del usuario a la consulta (expresada por un valor booleano)

## 💡 Decisiones técnicas tomadas

- La primera implementación al crear los turnos médicos era un botón que al presionarlo realizaba dicha tarea. Sin embargo, debido a posibles errores humanos en caso de que se use en un escenario real, se buscó la manera de automatizar este proceso, llegando así al uso de tareas en segundo plano para la ejecución de esta función.
- Se dividió todo el sistema de doctores en varias tablas dentro de la base de datos. Esto permitió modularizar la información, mejorar la organización del dominio y evitar el acoplamiento innecesario entre entidades.
- Limitación de 30 días al visualizar, reservar y crear turnos para favorecer el rendimiento de la aplicación al ejecutar dichas tareas.

## ▶ Ejecución del proyecto

1. Clonar el repositorio.
2. Configurar la cadena de conexión en appsettings.json.
3. Ejecutar las migraciones con Entity Framework.
4. Ejecutar la aplicación con dotnet run o desde Visual Studio.

## 🔜 Estado del proyecto y futuras actualizaciones

El proyecto sigue en estado de desarrollo y espero mejorarlo en futuras actualizaciones. Algunas funciones que tengo en mente implementar son las siguientes:

- Confirmación y verificación de cuenta mediante código e-mail
- Función de cambio de contraseña
- Nuevas funciones en el sistema de administradores (gráficos, análisis de diagnósticos en el último mes, qué especialidad fue la más consultada, qué enfermedades o síntomas se encontraron el último mes, rango de edad que más consultaron, etc.)
- Mejorar la interfaz visual con un diseño más moderno y visualmente agradable.
