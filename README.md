# Library Application 

## İçindekiler

- [Proje Hakkında](#proje-hakkında)
- [Kurulum](#kurulum)
  - [Adım 1: Projenin İndirilmesi](#adım-1-projenin-indirilmesi)
  - [Adım 2: Bağımlılıkların Yüklenmesi](#adım-2-bağımlılıkların-yüklenmesi)
  - [Adım 3: Veritabanı Ayarları](#adım-3-veritabanı-ayarları)
  - [Adım 4: Uygulamanın Çalıştırılması](#adım-4-uygulamanın-çalıştırılması)
- [Proje Bileşenleri](#proje-bileşenleri)
  - [Modeller](#modeller)
  - [Servisler](#servisler)
  - [Controller'lar](#controllerlar)
- [Algoritma ve Mantık](#algoritma-ve-mantık)

## Proje Hakkında

Bu proje, kütüphaneler için kitap yönetimi, ödünç alma ve iade işlemlerini dijitalleştirmeyi amaçlar. ASP.NET Core temelinde inşa edilmiştir ve Cloudinary entegrasyonu ile fotoğraf yükleme özellikleri sunar.

## Kurulum

### Adım 1: Projenin İndirilmesi

Projenin kaynak kodları GitHub üzerinde mevcuttur. Şu komutla projeyi klonlayabilirsiniz:
```bash
git clone [https://github.com/muratkarazade/LibraryApplication]
```

### Adım 2: Bağımlılıkların Yüklenmesi

Proje dizinine gidin ve gerekli NuGet paketlerini yükleyin:
```bash
cd [proje_klasörü]
dotnet restore
```

### Adım 3: Veritabanı Ayarları

1. Veritabanı bağlantı dizesini `appsettings.json` dosyasında belirtin:
```json
"ConnectionStrings": {
    "DefaultConnection": "[BAĞLANTI DİZİNİZ]"
}
```

2. Entity Framework Core migrations ile veritabanını oluşturun:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Adım 4: Uygulamanın Çalıştırılması

Uygulamayı başlatmak için:
```bash
dotnet run
```
Tarayıcınızda `http://localhost:5000` adresini ziyaret ederek uygulamanın çalışıp çalışmadığını kontrol edin.

## Proje Bileşenleri
### Modeller

- **Book**: Kütüphanedeki kitapları temsil eder. Özellikleri arasında kitabın adı, yazarın adı , kitap resmi dosya yolu , ödünç verme durumu gibi bilgiler bulunmaktadır.
  
- **User**: Kütüphaneyi kullanan kişileri temsil eder. Ad, soyad, email ve telefon gibi temel kullanıcı bilgilerini içerir.
  
- **Borrow**: Ödünç alınan kitapları ve kimin ödünç aldığını temsil eder.

### Servisler

Bu proje kapsamında, çeşitli işlevleri gerçekleştirmek için kullanılan servisler bulunmaktadır:

- **PhotoService**: Cloudinary ile entegrasyon sağlar. Fotoğrafların yüklenmesi, listelenmesi ve silinmesi işlevlerini içerir. İlgili hizmet, LibraryApplication.Services isim alanında tanımlanmıştır.

- **BookService**: Kitap ekleme, güncelleme, listeleme ve silme işlemlerini yönetir.

- **UserService**: Kullanıcı kaydı, girişi ve profil bilgilerinin güncellenmesi gibi işlemleri yönetir.
  
- **BorrowService**: Ödünç alınan kitapların yönetimini sağlar. Yeni bir ödünç işlemi ekler, ödünç işlemlerini filtreler, tüm ödünç işlemlerini getirir ve belirli bir ödünç işlemini ID ile getirir.

### Controller'lar

- **BookController**: Kitap işlemlerini yönetir. Ödünç alma, iade, kitap ekleme gibi işlevselliği barındırır.

- **UserController**: Kullanıcı işlemlerini yönetir. Kayıt olma, giriş yapma, profil bilgilerini güncelleme gibi işlevselliği barındırır.

- **BorrowController**:Ödünç alınan kitapları ve kimin ödünç aldığını yönetir.

## Algoritma ve Mantık

Bu bölümde, uygulamanın temel algoritmasının ve işleyiş mantığının detaylarına değinilmektedir.

1. **Ödünç Alma**: Kullanıcı, kütüphanedeki mevcut bir kitabı ödünç almak istediğinde, bu talep veritabanına işlenir ve kitap "ödünç alındı" durumuna getirilir.

2. **Fotoğraf Yükleme**: Kullanıcı veya yönetici, bir kitap veya profil fotoğrafı eklemek istediğinde, fotoğraf önce Cloudinary servisine yüklenir. Başarılı bir yükleme işlemi sonrası dönülen URL  bilgisi veritabanında saklanır.

3. **Hata Yönetimi**: Herhangi bir hata oluştuğunda, kullanıcıya anlaşılır bir hata mesajı döndürülür ve hata, loglama servisi aracılığıyla kaydedilir.
