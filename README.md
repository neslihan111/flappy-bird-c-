# Flappy Bird (C# / Windows Forms)

Nesne Tabanlı Programlama dersi ödevi kapsamında geliştirilmiş, Flappy Bird benzeri bir oyun. Kuşu SPACE tuşu veya sol fare tıklaması ile zıplatarak borular arasından geçirin. Her geçilen boru bir puan kazandırır, boruya veya yere çarpmak oyunu bitirir.

Proje, sınıfların ayrı dosyalarda tutulduğu nesne tabanlı bir yapıya sahiptir:

- `Bird.cs`: kuşun konumu, hızı, yerçekimi ve zıplama davranışı
- `Pipe.cs`: hareket eden boru çifti, çarpışma ve geçilme kontrolü
- `GameEngine.cs`: oyun durumu, skor, boru üretimi ve çarpışmalar
- `MainForm.cs`: zamanlayıcı, klavye/fare girdisi ve GDI+ ile çizim
- `Program.cs`: uygulamanın giriş noktası

## Kullanılan Teknolojiler

- C#
- .NET 8
- Windows Forms
- GDI+ (System.Drawing)

## Çalıştırma Adımları

1. .NET 8 SDK'yı yükleyin: <https://dotnet.microsoft.com/download/dotnet/8.0>
2. Depoyu klonlayın:
   ```bash
   git clone https://github.com/neslihan111/flappy-bird-csharp.git
   cd flappy-bird-csharp
   ```
3. Aşağıdakilerden birini yapın:
   - Komut satırında `dotnet run` komutunu çalıştırın, veya
   - Visual Studio ile `FlappyBird.csproj` dosyasını açıp F5 tuşuna basın.

Not: Proje Windows Forms kullandığı için yalnızca Windows üzerinde çalışır.

### Kontroller

- **SPACE** veya **sol fare tıklaması**: kuşu zıplatır
- **SPACE** (oyun bittikten sonra): oyunu yeniden başlatır

## Ekran Görüntüleri

![Ekran görüntüsü 1](docs/screenshots/ekran1.png)

![Ekran görüntüsü 2](docs/screenshots/ekran2.png)

> Not: Ekran görüntüleri henüz eklenmedi. Uygulama Windows üzerinde çalıştırılarak alınacak ve `docs/screenshots/` klasörüne `ekran1.png` ve `ekran2.png` olarak eklenecektir.
