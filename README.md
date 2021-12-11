# STR
Setur Backend Assessment Project
# Amaç
Basit bir telefon rehberi uygulaması

Projemiz aşağıdaki bileşenler'den oluşacak.

* STR.Data : Projenin veritabanını oluşturacak migration projesi 
* STR.Api.Contacts : Kişi ve iletişim bilgileri ile ilgili CRUD api servisi olacak
* STR.Api.Reports : Raporlama işlemleri ile ilgili CRUD api servisi olacak
* STR.Reporting : raporların kuyruğa alınması ve hazırlanması için RabbitMQ kullanılacak.
* STR.Tests : unit testler için xUnit test projesi kullanılacak.

## STR.Data 
Entity den veritabanı oluşturabilmek için aşağıdaki NUget paketleri yüklenir.
EntityFrameworkCore 
EntityFrameworkcore.Tools
EntityFrameworkcore.Design
EntityFrameworkcore.Relational
Npgsql.EntityFrameworkCore.PostgreSQL

migration class oluşturmak ve db yi yaratabilmek için 
Developer Powershell üzerinden (yok ise => "dotnet tool install --global dotnet-ef --version 3.x.x") dotnet kurulur.
PS içinde STR.Data dosyasına gelerek 
"dotnet ef migrations add initMigration" migrasyon dosyası oluşturulur.
"dotnet ef database update" database oluşturulur.


