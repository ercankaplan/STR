# STR
Setur Backend Assessment Project
# Amaç
Basit bir telefon rehberi uygulaması

Projemiz aşağıdaki bileşenler'den oluşacak.

* STR.Data : Projenin veritabanını oluşturacak migration projesi 
* STR.Api.PhoneBook : Kişi ve iletişim bilgileri ile ilgili CRUD api servisi olacak
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

## STR.Api.PhoneBook

Person,Contract CRUD işlemlerini yapmak üzre eklenen Api projesidir.

## STR.Api.Reports

Rapor Talep ve Sonucu ile ilgili CRUD işlmeleri için eklenen Api projesidir.

Report : Tanımlı rapor
ReportRequest : Tanımlı bir raporun talep kaydı
ReportResult : Rapor çıktısını tutar.

Rapor talepleri kaydedilirken aynı zamanda kuyruğa atılması için 
RabbitMQ.Client
Newtonsoft.Json yükledik nuget paketleri yüklenerek RabbitMQProducer sınıfı eklendi.
(Normalde queue durduğu restart olduğu durumda kuyrukta bekleyen rapor bilgisinin kaybedilmemesi gerekir ve kuyruğa almak içinde ayrı bir hosted/background servis olmalı)

## STR.Reporting

RabbitMQ Queue Consumer servisini çalıştırmak için bir console uygulaması eklendi.

RabbitMQ Erlang virtual runtime’da çalışır. Bunun için öncelikle Erlang‘in ilgili link’den yüklenmesi gerekmektedir
https://github.com/erlang/otp/releases/download/OTP-24.1.7/otp_win64_24.1.7.exe

RabbitMQ Server 'ı aşağıdaki linkden indirebilirsiniz.
https://github.com/rabbitmq/rabbitmq-server/releases/download/v3.9.11/rabbitmq-server-3.9.11.exe

cmd => C:\Program Files\RabbitMQ Server\rabbitmq_server-3.6.6\sbin\rabbitmq-plugins.bat enable rabbitmq_management
comutu ile gerekli configurasyon yapılır.

 http://localhost:15672  adresinden Queue arayüzüne ulaşabilirsiniz. default username/password => guest/guest olacaktır.
 
Console uygulamasına 
RabbitMQ.Client
Newtonsoft.Json yükledik nuget paketleri yüklenerek RabbitServer'a erişilmekte.

## STR.Test

Api Providerları için xUnitTest projesi eklendi.
DbContext in mock lanması için Moq nuget paketi eklendi.
ancak
<a href="https://docs.microsoft.com/en-us/ef/core/testing/#unit-testing" rel="nofollow">Microsoft does not recommend mocking a db context</a>
bu nedenle  DatabaseInMemory ile devam edildi.
Bunun için Microsoft.EntityFrameworkCore.InMemory nugetpaketi eklendi.

