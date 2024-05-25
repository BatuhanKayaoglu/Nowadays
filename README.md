# Nowadays

UYGULAMA TEKNIK ALTYAPISI

• .NET Core
• RestAPI (SwaggerUI)
• Repository/UnitOfWork Pattern
• SOLID
• EF Core (CodeFirst)

SENARYO
Nowadays adında bir görev takip sistemi (issue tracking) dizayn edilecek. (Ex: Jira)
Şirket oluşturarak ilgili şirkete proje;
Proje oluşturarak ilgili projeye çalışan ve görev;
Görev oluşturarak ilgili göreve projede bulunan çalışanlar eklenebilmelidir.
İlgili uygulamada;

• Şirket Ekleme/Silme/Düzenleme

• Proje Ekleme/Silme/Düzenleme/Atama

• Çalışan Ekleme/Silme/Düzenleme/Atama

• Görev Ekleme/Silme/Düzenleme/Atama

• Raporlama

hizmetleri olmalıdır.
Restful mimari kullanılarak geliştirilecek olan bu API;

• Company

• Project

• Employee

• Issue

• Report

servislerini barındırmalıdır.

1. Mevcut altyapı interface/abstract class gibi veri yapılarını barındırmalıdır.
2. Sisteme eklenen çalışanların TC kimlik numaraları doğrulanarak tüzel kişi
oldukları tespit edilmelidir. (https://tckimlik.nvi.gov.tr/service/kpspublic.asmx?wsdl)
