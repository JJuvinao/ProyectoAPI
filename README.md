Esta API se ejecuta en Visual Studio Community
link:https://visualstudio.microsoft.com/es/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022&source=VSLandingPage&passive=false&cid=2030

Para usar la API con VS Community tienes que intaslar Desarrollo de APS.Net Core, APS.NET, Desarrollo de escritorio .NET y Desarrollo de Node.js

Ya teniendo el Visual Studio Community, solo tendra que descargar el .zip del proyecto, descomprimirlo una vez descargado y ejecutar el archivo .sln

Para usar la API tendra que tener sqlserver ya instalado en su computadora.

Si no tiene instalado sqlserver, use este link para descargarlo e instalarlo:https://www.microsoft.com/es-es/download/details.aspx?id=104781

Una vez teniendo el sqlserver, se tiene que abrir la consola de administrador de paquetes
Ruta: Herramienta >> Administrador de paquetes Nuget >> consola de administrador de paquetes

Y escribir los siguentes comandos en el mismo orden 

comando 1: add-migration crearBase

comando 2: update-database

Y ya tendrias conectada la base de datos con la API para manejar la informacion  
