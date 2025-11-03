# C# Konuşmayı Metne Çevirme (Speech-to-Text) Projesi

Bu proje, C# `System.Speech` kütüphanesini kullanarak mikrofon aracılığıyla alınan sesi metne çeviren ve tanımlı sesli komutlara (Örn: "merhaba asistan", "kapat") yanıt veren bir konsol uygulamasıdır.

## ⚠️ ÖNEMLİ GEREKSİNİMLER

Bu proje, platform bağımlı bir kütüphane olan `System.Speech`'i kullandığı için, başarılı bir şekilde çalışması için **Windows işletim sisteminde belirli özelliklerin kurulu olmasını** gerektirir.

### 1. Konuşma Tanıma Paketi Kurulumu (Kritik)

Uygulamanın başlangıçta aldığı `Gerekli kimliğe sahip tanıyıcı bulunamadı` hatası, sistemde Konuşma Tanıma Motoru'nun eksik olduğu anlamına gelir.

**Projenin Dili:** Kod, motoru açıkça **Türkçe (`tr-TR`)** dilinde çalıştırmayı hedeflemektedir.

#### Kurulum Adımları (Hata Çözümü):

1.  **Windows Ayarları** > **Uygulamalar** > **İsteğe Bağlı Özellikler** (Optional features) menüsüne gidin.
2.  **"Özellik ekle"** (Add a feature) butonuna tıklayın.
3.  Listede **"Türkçe Konuşma Tanıma"** veya **"English (United States) Speech Recognition"** paketini bulun ve yükleyin. (Türkçe paketi bulunamazsa İngilizce paketi yükleyip 2. adımdaki CultureInfo'yu `en-US` olarak değiştirmeyi deneyin.)

### 2. Yönetici İzni (Erişim Engellendi Hata Çözümü)

Uygulamanın mikrofon donanımına ve yerel Windows servislerine erişebilmesi için yönetici yetkisi gerekir.

* Visual Studio'yu veya derlenmiş `.exe` dosyasını her zaman **Yönetici olarak (Run as administrator)** çalıştırmanız gerekmektedir.

---

## Projenin Yapısı ve Çalıştırma

### Kod Detayları

* **Motor Başlatma:** `recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("tr-TR"));` satırı ile motor Türkçe olarak başlatılır.
* **Gramerler:** Uygulama hem yüksek doğruluklu **Komut Gramerleri** (örn. `merhaba asistan`) hem de genel dikte için **Dikte Grameri** kullanır.

### Çalıştırma

1.  Proje klasörünü açın.
2.  Visual Studio'yu **Yönetici olarak** başlatın.
3.  Projeyi çalıştırın (F5).
4.  Konsol ekranında "Dinleme başladı." mesajını gördüğünüzde, net bir şekilde şu komutları söyleyerek test edin:

| Komut | Beklenen Aksiyon |
| :--- | :--- |
| `merhaba asistan` | Sesli yanıt verir. |
| `kapat` | Uygulamayı sonlandırır. |
| `Bugün hava nasıl?` | Metni algılayıp ekrana yazar (Dikte Grameri). |
