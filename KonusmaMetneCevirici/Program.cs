using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

namespace SesliAsistanim
{
    class Program
    {
        // Konuşma tanıma motoru
        private static SpeechRecognitionEngine recognizer;
        // Asistanın konuşması için (Opsiyonel ama test için iyi)
        private static SpeechSynthesizer synthesizer;

        // Uygulamanın çalışmaya devam etmesi için event nesnesi
        private static ManualResetEvent completionEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("--- C# SESLİ ASİSTAN BAŞLATILIYOR ---");

            try
            {
                // 1. Motorları Başlatma
                // Türkçe dilini hedef alıyoruz. Eğer sisteminizde kurulu değilse, "tr-TR" kaldırılıp boş bırakılabilir.
                recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("tr-TR"));
                synthesizer = new SpeechSynthesizer();
                synthesizer.SetOutputToDefaultAudioDevice();

                // 2. Giriş ve Çıkış Ayarları
                recognizer.SetInputToDefaultAudioDevice();

                // 3. Gramerleri Yükleme
                LoadGrammars();

                // 4. Olayları Tanımlama
                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                recognizer.SpeechRecognitionRejected += Recognizer_SpeechRejected;

                // 5. Dinlemeyi Başlatma (Sürekli mod)
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                synthesizer.SpeakAsync("Sistem başlatıldı. Dinliyorum.");
                Console.WriteLine("Dinleme başladı. Kapatmak için ENTER tuşuna basınız.");

                // Uygulamanın sonsuza kadar çalışmasını sağla
                completionEvent.WaitOne();

                // Kapanış
                recognizer.RecognizeAsyncStop();
                recognizer.Dispose();
                synthesizer.Dispose();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[KRİTİK HATA] Lütfen Windows Konuşma Tanıma ayarlarınızı kontrol edin.");
                Console.WriteLine("Detay: " + ex.Message);
                Console.ResetColor();
                Console.ReadLine();
            }
        }

        private static void LoadGrammars()
        {
            // A) KOMUT GRAMERİ (Yüksek Doğruluklu Komutlar)
            Choices choices = new Choices();
            choices.Add(new string[] { "merhaba asistan", "ne yapıyorsun", "kapat" });

            GrammarBuilder gb = new GrammarBuilder(choices);
            gb.Culture = new System.Globalization.CultureInfo("tr-TR"); // Türkçe kültür
            Grammar komutGrameri = new Grammar(gb);
            komutGrameri.Name = "KomutGrameri";
            recognizer.LoadGrammar(komutGrameri);

            // B) DİKTE GRAMERİ (Serbest Konuşma Tanıma)
            // Bu gramer, komutlar dışında her şeyi yazıya dökmeye çalışır.
            DictationGrammar dictationGrammar = new DictationGrammar();
            dictationGrammar.Name = "DikteGrameri";
            recognizer.LoadGrammar(dictationGrammar);
        }

        private static void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedText = e.Result.Text;
            float confidence = e.Result.Confidence;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[Algılandı] Metin: {recognizedText} | Güven: {confidence:P0}");
            Console.ResetColor();

            // Algılanan metne göre işlem yapma
            if (recognizedText.ToLower().Contains("merhaba asistan"))
            {
                synthesizer.SpeakAsync("Merhaba! Sana nasıl yardımcı olabilirim?");
            }
            else if (recognizedText.ToLower().Contains("kapat"))
            {
                synthesizer.SpeakAsync("Görüşmek üzere!");
                completionEvent.Set(); // Uygulamayı durdurur
            }
            else if (recognizedText.ToLower().Contains("not al"))
            {
                synthesizer.SpeakAsync("Not almanız gereken nedir?");
                // Burada metni bir dosyaya veya ekrana yazdırabilirsiniz
            }
        }

        private static void Recognizer_SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            // Tanıma motoru bir ses duydu ama hiçbir gramer ile eşleştiremedi.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[Reddedildi] Güven: {e.Result.Confidence:P0} - Tanıyamadı.");
            Console.ResetColor();
        }
    }
}