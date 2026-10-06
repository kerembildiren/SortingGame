# GDD — Chubby's Clutter (çalışma adı)

| Alan | Değer |
|---|---|
| Sürüm | 0.2 (ilerleme kuralları yenilendi: yardımcılar, Oto Sort güçlendirmesi, sade koleksiyon) |
| Tarih | 2026-10-06 |
| Durum | Prototip M4.3'e kadar oynandı ve onaylandı; M5 öncesi ilerleme kuralları güncellendi |
| Platform | iOS ve Android (mobil), dikey ekran |
| Motor | Unity |
| Tür | Cozy / düzenleme / koleksiyon / hafif idle |
| Gelir modeli | Ödüllü reklam + uygulama içi satın alma |

## 0. Bu belge nasıl okunmalı

Bu belge oyunun tek doğruluk kaynağıdır. Geliştirme sırasında (insan ya da Claude Code) bir karar gerektiğinde önce buraya bakılır.

Belgedeki işaretler:

- **[KARAR]** Netleşmiş, tartışılmadan değiştirilmemesi gereken karar.
- **[VARSAYILAN]** Mantıklı bir başlangıç değeri. Test sonrası değişebilir, değişirse Karar Günlüğü'ne (bölüm 19) yazılır.
- **[AÇIK]** Henüz karara bağlanmamış konu. Kod bu konular için esnek yazılmalı, kesin varsayımda bulunmamalı.

Kod içinde kullanılacak isimler `kod_biçiminde` verilmiştir. Türkçe terimlerin kod karşılıkları için bölüm 17'deki sözlüğe bak.

Konsept görseller `concept/` klasöründedir. Görselleri göremeyen okuyucular için her birinin açıklaması bölüm 12'de yazılıdır.

---

## 1. Özet

Oyuncu dağınık, terk edilmiş yerleri düzene sokar: önce küçük bir kutu, sonra bir garaj, bir depo, ileride kapanmış bir oyuncakçı veya terk edilmiş bir sirk. Bu yerlerdeki binlerce eşyayı tek tek ayıklayıp doğru raflara yerleştirir, tozu süpürür ve kaosu düzene çevirir. Eşyaların neredeyse tamamı para getirir; bu para işi hızlandıran aletlere ve oyuncunun yanında çalışan yardımcılara harcanır. Asıl hedef ise her mekânda temaya uygun kostümüyle saklanan maskot figürü, Chubby'yi bulmak ve Koleksiyon Kitabı'nı tamamlamaktır. Bir mekânın tüm bölümleri temizlenince bir sonraki, daha büyük mekân açılır. Oyun yalnızca oynanırken ilerler; çevrimdışı kazanç yoktur.

**Tek cümlelik konsept:** Dağınık yerleri eşya eşya düzenle, içinde saklanan Chubby'leri bul.

**Hedef kitle:** Cozy, ASMR, "oddly satisfying" içerikleri seven geniş mobil kitle. Kısa oturumlarla oynayan, ama uzun vadeli koleksiyon hedeflerinden keyif alan oyuncular.

---

## 2. Değişmez prensipler

Bu prensipler oyunun kimliğidir. Bir özellik bunlardan biriyle çelişiyorsa, özellik değişir; prensip değil.

1. **Kaostan düzene, gözle görülür şekilde.** Her eylem mekânı görünür biçimde daha düzenli hale getirmelidir. Oyuncunun düzenlediği yer düzenli kalır; çıkıp geri döndüğünde emeğini görür.
2. **Baskı yok.** Zaman sınırı yok, yanlış yerleştirmeye ceza yok, enerji sistemi yok, oyuncuyu bekleten duvarlar yok. Oyun her zaman rahatlatıcıdır.
3. **Basit eylem, büyük ölçek.** Temel eylemler tek parmakla yapılır (dokun, sürükle, kaydır). Derinlik eylemlerin karmaşıklığından değil, eşyaların miktarından ve çeşitliliğinden gelir.
4. **Tek kuzey yıldızı: Koleksiyon Kitabı.** Oyuncu her an neyin peşinde olduğunu bilir. Para bir araçtır, hedef koleksiyondur.
5. **%99 yakıt, %1 hazine.** Sıradan eşyalar para ve ilerleme getirir; arada çıkan nadir (mavi) eşyalar küçük sürprizlerdir; asıl ödül her mekânda yalnızca bir tane bulunan Chubby'dir. Koleksiyon parçası nadir kaldıkça değerlidir.
6. **Otomasyon sıkıcı kısmı alır, heyecanlı kısmı asla.** Hiçbir otomasyon (yardımcılar, Oto Sort) nadir eşyaları veya Chubby'yi oyuncunun yerine bulmaz ya da toplamaz. Kutu açmak ve süpürmek de her zaman oyuncunun işidir.
7. **Mühendislik yok.** Konveyör, üretim hattı, yerleşim planı, kaynak zinciri gibi kurulum gerektiren sistemler oyuna girmez. Otomasyon tek dokunuşla satın alınır ve çalışır.
8. **Bilgi = hız.** Oyuncu kategorileri öğrendikçe kendi eliyle hızlanır. Kalıcı hızlanma aletlerden ve yardımcılardan gelir; kendiliğinden dizme (Oto Sort) kalıcı bir hak değil, tek seferlik bir güçlendirmedir. Oyun hiçbir noktada kendi kendini oynar hale gelmez.
9. **Para ile koleksiyon satın alınmaz.** Gerçek parayla belirli bir nadir eşya, koleksiyon parçası veya maskot satın alınamaz. Gerçek para ile satın alınan rastgele ödül (loot box) yoktur.
10. **Tamamen özgün içerik.** Gerçek markalar, logolar, karakterler veya telifli eserler oyunda yer almaz (bkz. bölüm 13).
11. **Oyun yalnızca oynanırken ilerler.** Çevrimdışı (AFK) ilerleme ve kazanç yoktur. Yardımcılar yalnızca oyuncunun içinde bulunduğu odada, oyuncu oradayken çalışır. Oyuncu her zaman oynamaya teşvik edilir.

---

## 3. Referanslar ve farklılaşma

| Referans | Aldığımız | Almadığımız |
|---|---|---|
| Sort Them Ducks (PC, 2026) | Eşyaları kategorilere göre raflara dizme tatmini, nadir özel parçalar, yükseltmelerle hızlanma | Oyuncak ördek dükkânı teması, birinci şahıs yürüyerek gezme, tek mekânlı yapı |
| Needle In A Haystack Simulator ve benzerleri | Devasa miktar hissi, elle başlayıp aletlerle hızlanma, yığında saklı nadir eşyalar | Mühendislik gerektiren otomasyon (konveyör hatları vb.) |
| Yaprak temizleme oyunları (Leaf it Alone, Leaf Blower Revolution vb.) | Yüzeyi süpürerek açma tatmini, alanın gözle görülür şekilde temizlenmesi | Saf idle sayı artırma odağı |
| Bid Wars serisi (mobil), Storage Hunter Simulator (PC) | Depo/mekân satın alma ve içinden ne çıkacak merakı, kanıtlanmış mobil ilgi | Açık artırma ve teklif verme mekaniği, rehin dükkânı ticareti |

**Farklılaşma özeti:** Mobildeki "sort" oyunları bölüm bazlı bulmacalardır. Depo oyunları ise teklif verme ve ticaret odaklıdır; eşyalar bir listede görünür. Bu oyun, kalıcı ve büyük mekânları dokunsal olarak eşya eşya düzenleme deneyimini mobile getirir.

---

## 4. Oyun döngüleri

### 4.1 Anlık döngü (saniyeler)
Kutuya dokun → kutu yerinde devrilir, eşyalar zemine saçılır → eşyayı doğru rafa sürükle → para kazan, raf dolar → tozu süpür, altından yeni eşyalar çıkar → mavi parlayan nadir eşyayı rafına koy, biraz daha fazla para kazan → altın parlayan Chubby'ye dokun, Koleksiyon Kitabı'na gider.

### 4.2 Oturum döngüsü (1–5 dakika)
Genel bakıştan bir bölüm seç → birkaç kutu/yığın düzenle → bölüm yüzdesi artar → yeni alet veya yükseltme al → yardımcılar sen oynarken yanında çalışır → oyundan çık (oyun kapalıyken hiçbir şey ilerlemez).

### 4.3 Meta döngü (günler/haftalar)
Mekânın bölümlerini temizle ve düzenle → o mekânın Chubby'sini bul → tüm bölümler %100 olunca sıradaki mekân açılır → Coin ile aletleri ve yardımcıları güçlendir; mekân ilerledikçe yeni alet ve yardımcı kilitleri açılır.

### 4.4 Kaynak akışı
- Sıradan eşya doğru rafa → **Coin** (anında).
- Nadir (mavi) eşya doğru rafa → biraz daha fazla **Coin**.
- Chubby → **Koleksiyon Kitabı** (her Chubby yalnızca bir kez çıkar, kopya yoktur).
- Coin → aletler, alet yükseltmeleri, yardımcılar, yardımcı yükseltmeleri, bölüm kilitleri.
- Ödüllü reklam veya gerçek para → **Oto Sort** güçlendirmesi (Coin ile alınamaz).

---

## 5. Mekânlar ve bölümler

### 5.1 Yapı [KARAR]
- Her mekân (`Venue`) bir veya daha fazla bölümden (`Section`) oluşur.
- Mekânlar satın alınmaz ve satılmaz: bir mekânın tüm bölümleri %100 olunca sıradaki mekân ücretsiz açılır. Açık artırma veya teklif sistemi yoktur.
- Mekânlar küçükten büyüğe bir merdiven halinde ilerler. Büyüdükçe eşya sayısı, kategori sayısı ve nadir eşya sayısı artar.
- Her mekânın bir teması vardır (çizgi romanlar, garaj, sirk...). Tema; eşya havuzunu, kategorileri ve maskotun kostümünü belirler.
- Mekân ilerlemesi oyundaki tek "seviye" ölçüsüdür: alet ve yardımcı kilitleri oyuncu seviyesine değil, hangi mekâna gelindiğine bağlıdır (bkz. 10). Ayrı bir XP/seviye sistemi yoktur.

### 5.2 İlerleme sırası [VARSAYILAN]
Mekânlar doğrusal sırayla açılır: bir mekân tamamlanınca bir sonraki açılır. İleride belli bir noktadan sonra haritada birden fazla seçenek sunmak **[AÇIK]**.

### 5.3 Mekân merdiveni (taslak)

| # | Mekân | Bölümler | Eşya (yaklaşık) | Not |
|---|---|---|---|---|
| 1 | Çizgi Roman Kutusu | 1 | 50 | Öğretici. Tek ekran, 2–3 dakika. |
| 2 | Garaj | 1–2 | 500 | İlk alet yükseltmeleri (El, Süpürge). |
| 3 | Depo | 4 (Ofis, Garaj, Raf Koridoru, Bodrum) | 8.000 | İlk yardımcı, Mıknatıs'ın alınabilir hale gelmesi. |
| 4 | Kapanmış Oyuncakçı | 3–4 | — | MVP sonrası |
| 5 | Tavan Arası / Eski Ev | 3–4 | — | MVP sonrası |
| 6 | Terk Edilmiş Sirk | 4–5 (Kostüm Odası, Bilet Gişesi, Palyaço Karavanı, Ana Çadır...) | — | MVP sonrası |
| 7+ | Eski Tiyatro, Lunapark... | — | — | MVP sonrası |

Sayılar **[VARSAYILAN]**, dengeleme testlerinde değişecek. Prototipteki güncel büyüklükler: Çizgi Roman Kutusu ~60, Garaj ~200, Depo'nun her odası ~300 eşya (bkz. karar günlüğü).

### 5.4 Bölüm kilitleri [VARSAYILAN]
Bir mekân içindeki bazı bölümler kilitli başlar (ör. Depo'daki Bodrum). Kilit, mekân içindeki diğer bölümlerin belli bir yüzdeye ulaşmasıyla veya Coin ile açılır.

### 5.5 Hafif renovasyon [KARAR]
- Oyunda duvar boyama, mobilya seçme gibi iç dekorasyon **yoktur**. Bu, prensip 3 ve 7 ile çelişir ve içerik maliyetini katlar.
- Renovasyon kendiliğinden olur: bir bölüm %100 olduğunda kısa bir "önce / sonra" animasyonu oynar. Toz kaybolur, ışıklar yanar, renkler canlanır, ortam sesi değişir.
- Temizlenen bölümler genel bakışta temiz ve renkli kalır.

### 5.6 Mekânın tamamlanması [KARAR]
- Mekân satışı yoktur. Mekânın tüm bölümleri %100 olduğunda sıradaki mekân ücretsiz açılır.
- Chubby'nin saklı olduğu bölüm, Chubby alınmadan %100 olmaz. Dolayısıyla tamamlanan bir mekânda bulunmamış koleksiyon parçası kalmaz; eski mekânlara "eksik parça için dönüş" mekaniğine gerek yoktur.
- Temizlenen odalar temiz kalır ve tekrar oynanmaz. Biten odada oyuncu isterse kalıp etrafa bakabilir, dolu raflara yakından bakabilir ve yardımcılarını sevebilir (bkz. 10.3).

---

## 6. Görünümler ve kamera

Oyunda iki ana görünüm vardır. Ayrı bir "çalışma masası" ekranı **yoktur** **[KARAR]**; bu fikir, oyuncuyu mekândan kopardığı için reddedildi.

### 6.1 Genel bakış (`OverviewView`) [KARAR]
- İzometrik, üstten kesit (cutaway) görünüm. Mekânın tüm bölümleri aynı anda görünür.
- Amacı ölçeği hissettirmek ve ilerlemeyi göstermektir. Burada eşya düzenlenmez.
- Dağınık bölümler loş, tozlu ve soluk; temizlenen bölümler aydınlık, sıcak ve renklidir.
- Her bölümün üstünde bir etiket: bölüm adı + temizlik yüzdesi. Kilitli bölümler karanlık ve kilit simgelidir.
- Üstte toplam ilerleme sayacı (ör. `1,240 / 8,000 items`).
- Yardımcılar genel bakışta görünmez; oyuncu hangi odaya girerse orada belirirler (bkz. 10.3).
- Bir bölüme dokununca kamera o bölüme yaklaşır (zoom geçişi) ve Bölüm görünümüne geçilir.
- Referans: `concept/01_overview_warehouse.png`

### 6.2 Bölüm görünümü (`SectionView`) [KARAR]
- Oyunun asıl oynandığı ekran. Kamera bölüme 3/4 açıyla yukarıdan bakar.
- Bölümün **gerçek rafları** ekrandadır, her rafın bir kategori etiketi vardır. Raflardaki boş yuvalar kesikli çizgili silüetlerle gösterilir.
- Zemin; kutular, çantalar, mobilyalar ve dağınık eşyalarla doludur.
- Üstte bölüm adı ve yüzdesi; altta alet çubuğu (El, Süpürge, Mıknatıs).
- Büyük bölümlerde kamera parmakla yana kaydırılabilir **[VARSAYILAN]**; küçük bölümler tek ekrana sığar. Pinch-zoom **[AÇIK]**.
- Referans: `concept/02_section_garage.png`

### 6.3 Ekran yönü [KARAR]
Oyun tamamen dikey (portrait) ve tek elle oynanabilir olmalıdır. Sık kullanılan kontroller ekranın alt yarısında, başparmak erişiminde durur.

---

## 7. Etkileşim ve kontroller

### 7.1 Temel hareketler [KARAR]
| Hareket | Ne yapar |
|---|---|
| Kaba nesneye dokunma (kutu, çanta, sandık) | Nesne **olduğu yerde** devrilir/açılır, içindekiler zemine saçılır. |
| Eşyayı sürükle-bırak | Eşya rafa yerleştirilir. Doğru rafsa yerine oturur ve Coin verir. |
| Zeminde parmağı kaydırma (Süpürge seçiliyken) | Toz, kâğıt, çöp süpürülür. Altından yeni eşyalar çıkabilir. Yaprak temizleme hissi. |
| Altın parlayan Chubby'ye dokunma | Chubby bulma anı tetiklenir (bkz. 9.3). |
| Mavi parlayan nadir eşyayı sürükle-bırak | Normal eşya gibi kendi kategorisinin rafına konur, biraz daha fazla Coin verir (bkz. 9.4). |
| Oda bittikten sonra yardımcıya dokunma | Yardımcı sevinir: kalpler çıkar, tatlı bir ses çıkarır (bkz. 10.3). |

### 7.2 Yanlış yerleştirme [VARSAYILAN]
Ceza yoktur (prensip 2). Eşya yanlış rafa bırakılırsa yumuşak bir animasyonla zemine geri döner; doğru raf kısa bir an hafifçe vurgulanabilir. Hiçbir zaman Coin kaybı olmaz.

### 7.3 Çöp ve kategorisiz eşyalar [VARSAYILAN]
Süpürülen toz ve kâğıt parçaları ayrı eşya sayılmaz; zemindeki bir "kirlilik katmanı" olarak modellenir ve bölüm yüzdesine katkı sağlar. Gerçek "çöp" eşyalar (kırık şeyler vb.) için bölümde bir çöp kutusu bulunur **[AÇIK: dahil edilip edilmeyeceği]**.

### 7.4 Geri bildirim [KARAR]
Her başarılı eylem görsel + ses + haptik geri bildirim verir. Bu oyunun his kalitesi için kritik, "sonra eklenir" diye ertelenmez.

---

## 8. Eşyalar ve kategoriler

### 8.1 Eşya türleri
| Tür | Kod | Davranış |
|---|---|---|
| Sıradan eşya | `ItemRarity.Common` | Bir kategoriye aittir, rafa yerleştirilir, Coin verir. Eşyaların ~%99'u. |
| Nadir eşya | `ItemRarity.Rare` | Bir kategorinin özel üyesidir (ör. jelatinli bir ilk sayı çizgi roman). **Mavi** ışıkla parlar, normal eşya gibi kendi kategorisinin rafına konur ve sıradan eşyadan biraz daha fazla Coin verir. Kitaba girmez. Her odada bulunmaz; olan odalarda birkaç tanedir. |
| Maskot figürü (Chubby) | `ItemRarity.Mascot` | Oyundaki **tek koleksiyon parçası türü**. **Altın** ışıkla parlar, dokunularak alınır, Koleksiyon Kitabı'na gider. Mekân başına bir tane. |
| Kaba nesne (konteyner) | `ContainerDefinition` | Kutu, çanta, sandık. Açılınca içindeki eşyaları saçar. |
| Kirlilik | `DirtLayer` | Süpürülerek temizlenir. Eşya değildir. |

### 8.2 Kategoriler [KARAR]
- Her sıradan eşya tek bir kategoriye aittir (ör. Çizgi Roman, Oyuncak, Alet).
- Kategoriler **mekânlar arası ortaktır**: "Oyuncak" kategorisi birden fazla mekânda çıkabilir. Oyuncunun öğrendiği kategoriler böylece sonraki mekânlarda da işine yarar (prensip 8).
- Her yeni mekân en az birkaç **yeni** kategori getirir; böylece her mekânda elle düzenleme tekrar başlar.
- Kategori, oyuncunun eşyanın görünüşünden tanıyabileceği kadar net olmalıdır. Belirsiz eşya tasarlanmaz.

### 8.3 Eşya çeşitliliği
Aynı kategorideki eşyalar görsel olarak farklı varyasyonlara sahiptir (farklı kapak, renk, şekil). Sort Them Ducks'taki "her ördek farklı" hissi hedeflenir. Varyasyonlar maliyet için modüler üretilmelidir (taban model + doku/renk/aksesuar kombinasyonları).

---

## 9. Koleksiyon sistemi

### 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR]
- Oyunun kuzey yıldızı.
- Kitap tek bir Chubby albümüdür: her mekân için bir yuva vardır ve o mekânın kostümlü Chubby'si oraya girer. Bulunmamış olanlar gri silüet olarak görünür.
- İlerleme sayacı (ör. `2 / 3`).
- Scrapbook / albüm estetiği: kâğıt dokusu, çıkartmalar, damgalar.

### 9.1.1 Vitrin (`CollectionViewer`) [KARAR]
- Kitapta bulunmuş bir parçaya dokununca parça, kararmış ekranın ortasında 3D olarak açılır.
- Oyuncu parçayı tek parmakla döndürür, iki parmakla yakınlaştırır ve kaydırır (editörde: fare tekerleği ve sağ tık sürükleme). Çift dokunuş görünümü sıfırlar; dokunulmadığında parça yavaşça kendi etrafında döner.
- Altta parçanın adı ve kısa açıklaması, tek bir "Kapat" butonu. Kapatınca kitaba dönülür.
- Amaç: toplanan parçaları istendiği an sergileyebilmek; koleksiyonun kendisi ödül hissi verir (prensip 4).

### 9.2 Maskot [KARAR]
- Oyunun özgün bir maskotu vardır: yuvarlak gövdeli, küçük kollu-bacaklı, iri ve sevimli gözlü bir koleksiyon figürü. Taban adı **Chubby** **[VARSAYILAN]**.
- Her mekânda maskot temaya uygun bir kostümle saklıdır (ör. Çizgi Roman Kutusu'nda süper kahraman kostümlü "Captain Chubby", Sirk'te sunucu kostümlü Chubby).
- Mekân başına **tek** Chubby vardır ve o mekânın bölümlerinden birinde (bir kutuda ya da tozun altında) saklıdır. Oyunda başka koleksiyon parçası yoktur; koleksiyon böylece nadir ve değerli kalır **[KARAR]**.
- Chubby'nin saklı olduğu bölüm, Chubby alınmadan %100 olmaz. Bulunan Chubby bir daha çıkmaz.
- Maskot tasarımı hiçbir mevcut karakteri veya logoyu çağrıştırmamalıdır (bkz. bölüm 13).
- **Yardımcılar maskottan ayrı karakterlerdir** **[KARAR]**. Konsept görsellerde ikisi birbirine çok benziyor; nihai tasarımda net şekilde ayrışmalılar.

### 9.3 Chubby bulma anı [KARAR]
- Chubby zeminde altın ışıkla parlar. Oyuncu dokununca figür yükselir, ışık huzmeleri ve parıltılar çıkar, arka plan kararır.
- Chubby otomatik olarak Koleksiyon Kitabı'na eklenir. Oyuncuya "sat mı, sakla mı" seçimi sunulmaz.
- Kartta: başlık, Chubby'nin adı, kısa ve sevimli bir açıklama, tek bir "Devam" butonu.
- Kopya yoktur: bulunan Chubby bir daha çıkmaz, kopya satışı da yoktur.
- Not: `concept/03_rare_find_popup.png` görselindeki "Add to collection / Sell" seçimi geçersizdir.

### 9.4 Nadir (mavi) eşyalar [KARAR]
- Bazı odalarda, bir kategorinin özel üyesi olan birkaç nadir eşya bulunur (ör. çizgi romanlar arasında jelatinli bir ilk sayı). Her odada olmak zorunda değildir.
- **Mavi** ışıkla parlarlar; böylece altın parlayan Chubby ile karışmazlar.
- Normal eşya gibi sürüklenip kendi kategorisinin rafına konurlar ve o kategorinin sıradan eşyasından biraz daha fazla Coin verirler. Yerine oturduklarında küçük bir ek kutlama efekti olur.
- Kitaba girmezler, koleksiyon sayılmazlar.
- Yardımcılar bunlara dokunmaz **[KARAR]**. Oto Sort da dokunmaz **[VARSAYILAN]**; rafa koymak oyuncunun küçük ödülüdür.

### 9.5 Set bonusları [AÇIK]
Kitap tek albüme indiği için "sayfa tamamlama" bonusu kalmadı. Belirli sayıda Chubby bulununca kalıcı bir bonus verilip verilmeyeceği sonra kararlaştırılacak.

---

## 10. Otomasyon ve ilerleme

Kalıcı iki katman vardır: aletler ve yardımcılar. Bunlara ek olarak tek seferlik bir güçlendirme (Oto Sort) bulunur. Hiçbiri kurulum, yerleşim veya mühendislik gerektirmez (prensip 7). Kilitler oyuncu seviyesine değil mekân ilerlemesine bağlıdır (5.1).

### 10.1 Aletler (`Tool`) [KARAR]
Oyuncunun kendi elini güçlendirir. Alet çubuğunda seçilir, Coin ile seviye atlatılır.

| Alet | Temel işlev | Yükseltme örnekleri |
|---|---|---|
| El (`Hand`) | Eşya taşıma. Taşırken üzerinden geçilen eşyalar da ele alınır (her türden, kapasite kadar). Bir rafın üzerinde kısa süre beklenince eldekilerden o rafa ait olanlar yerleşir, diğerleri elde kalır. | Aynı anda taşınabilen eşya sayısı (1 → 2 → 3) |
| Süpürge (`Broom`) | Kirlilik katmanını temizleme | Daha geniş süpürme alanı |
| Mıknatıs (`Magnet`) | Bir eşya taşınırken, parmak hareket ettikçe küçük çekim yarıçapına giren aynı kategoriden eşyalar ele çekilir | Daha çok eşya (1. seviyede 2), biraz daha geniş yarıçap |

**Mıknatıs kilidi [KARAR]:** Mıknatıs güçlü bir alettir ve erken alınamaz. Almak için El'in son seviyede olması gerekir; fiyatı da El yükseltmelerinin toplamından belirgin şekilde yüksektir. Shop'ta kilitliyken de görünür ve neden kilitli olduğunu söyler ("önce El'i geliştir"). Amaç: oyuncu önce El'i geliştirip kazancını hızlandırsın, Mıknatıs'a sonra ulaşsın; "El'i atla, doğrudan Mıknatıs al" en iyi hamle olmasın.

### 10.2 Oto Sort güçlendirmesi (`AutoSortBoost`) [KARAR]
Eski "Kategori Ustalığı"nın yerini alır. Kendiliğinden dizme artık kalıcı bir ilerleme değil, oyuncunun isteyerek kullandığı tek seferlik bir güçlendirmedir; aksi halde oyun bir noktadan sonra fazla kolaylaşıyordu.
- **Kullanım:** Oyuncu odadaki bir rafı seçer. O rafın kategorisi **o oda bitene kadar** kendiliğinden dizilir: yerde açıkta duranlar hemen rafa uçar; sonradan kutudan dökülen ya da süpürülerek ortaya çıkanlar da uçar.
- **Sınır:** Oda başına en fazla 1 raf (hak nereden gelmiş olursa olsun).
- **Edinme:** Ödüllü reklam izleyerek ya da gerçek parayla (IAP: tek tek veya paket halinde satılan kullanım hakları). **Coin ile alınamaz.**
- **Kalıcı değildir:** Oda bitince etkisi biter, başka odaya taşınmaz. Kalıcı kategori ustalığı yoktur; "N eşya diz, kategori açılsın" sayacı kaldırılmıştır.
- Chubby ve nadir (mavi) eşyalar etkilenmez (prensip 6).
- Kutlama: kısa banner + eşyaların rafa uçtuğu tatmin edici animasyon.
- Gerçek reklam ve mağaza sağlayıcıları bağlanana kadar sahte sağlayıcılarla çalışır (bkz. 15.5).

### 10.3 Yardımcılar (`Helper`) [KARAR]
Kaldırılan ustalığın bıraktığı boşluğu dolduran kalıcı ilerleme katmanı.
- **Karakter:** Maskottan ayrı, küçük, tatlı, yuvarlak ve yumuşak (bubbly / squishy) karakterler.
- **Edinme:** Shop'tan Coin ile alınır. Yardımcı yuvaları mekân ilerlemesiyle açılır. Her mekânda yeni yardımcı gelmez; doz bilinçli olarak düşük tutulur **[VARSAYILAN: MVP'de en fazla 2 yardımcı; ilk yuva Garaj tamamlanınca, ikincisi Depo'nun ortasında açılır]**.
- **Nerede çalışırlar:** Atama ekranı yoktur. Oyuncu hangi odaya girerse yardımcılar orada belirir; oyuncu çıkınca çalışmazlar.
- **Ne yaparlar:** Yerde açıkta duran sıradan eşyaları yavaşça, tek tek alıp doğru rafa dizerler. Sonradan ortaya çıkan eşyaları da (oyuncu kutu açtıkça dökülenler, süpürdükçe çıkanlar) görür ve toplarlar; "sonradan gelen eşya atlanır" hatası olmamalıdır.
- **Neye dokunmazlar:** Kutular, toz, nadir (mavi) eşyalar ve Chubby. Kutu açmak ve süpürmek oyuncunun işidir; nadir eşyalar ve Chubby zeminde parlayarak oyuncuyu bekler.
- **Kazanç:** Yardımcının dizdiği eşya da Coin verir **[VARSAYILAN]**.
- **Yükseltmeler:** Coin ile; tek seferde birden fazla eşya taşıma, hız. Yerleşim, rota veya planlama yoktur. Yardımcılar ve yükseltmeleri ucuz değildir; oyun uzun süre oynanabilir kalmalıdır.
- **Çevrimdışı çalışmazlar** (prensip 11). "Siz yokken..." özeti ve çevrimdışı kazanç yoktur.
- **Oda bitince:** Yardımcılar odada boş boş dolaşır. Oyuncu onlara dokununca evcil hayvan gibi sevinirler: zıplar/ezilir, kalpler çıkar, tatlı bir ses çıkarırlar.

### 10.4 Ölçek büyüdükçe oyuncunun rolü
Küçük mekânlarda oyuncu her eşyayı eliyle düzenler. Büyük mekânlarda aletler işi hızlandırır, yardımcılar sıradan işin bir kısmını üstlenir; oyuncu kutuları açar, zemini süpürür, nadir eşyaları ve Chubby'yi kendisi bulur. Oyuncu her zaman odadaki en hızlı ve en önemli çalışandır; yardımcılar ona eşlik eder, onun yerine oynamaz.

---

## 11. Ekonomi ve gelir modeli

### 11.1 Para birimleri
- **Coin** (yumuşak para) [KARAR]: Oyun içinde kazanılır. Aletler, alet yükseltmeleri, yardımcılar, yardımcı yükseltmeleri, bölüm kilitleri. Mekânlar ve Oto Sort Coin ile alınmaz.
- **Premium para** (ör. Gem) **[AÇIK]**: Gerekip gerekmediği ve ne için kullanılacağı netleşmedi. Eklenirse prensip 9 geçerlidir.

### 11.2 Ödüllü reklamlar [KARAR]
Yalnızca oyuncu isteyerek izler.
- **Oto Sort** (bkz. 10.2): reklamın ana ödülü **[KARAR]**. Oda başına 1 raf.
- Başka reklam ödülleri (ör. belirli süre 2x Coin, yardımcılara geçici hız artışı) **[AÇIK]**.

### 11.3 Uygulama içi satın almalar [KARAR]
- **Oto Sort kullanım hakları** **[KARAR]**: güçlendirme (power-up) mantığında, tek tek ve paket halinde satılır. Oyun içi Coin ile alınamaz; ya reklam ya gerçek para.
- Diğer ürünler **[AÇIK]**: başlangıç paketi, kozmetik raf/albüm temaları, reklamsız ödül paketi (reklamı izlemeden ödülleri almak). Coin paketi ve ek yardımcı yuvası gibi tempoyu parayla atlatan ürünler, oyunun uzun süre oynanabilir kalması hedefiyle birlikte değerlendirilir.

### 11.4 Gelir modeli kuralları [KARAR]
- Oyuncunun oyununu kesen zorunlu reklamlar (interstitial) **yoktur** **[VARSAYILAN]**. Eklenmesi düşünülürse prensip 2'ye aykırı olmayacak şekilde tartışılır.
- Gerçek para ile nadir eşya, maskot veya koleksiyon parçası satın alınamaz.
- Gerçek para ile satın alınan rastgele ödül (loot box) yoktur.
- Enerji, bekleme süresi veya ödeme duvarı yoktur.

### 11.5 Dengeleme
Tüm fiyat, ödül ve eşik değerleri koda gömülmez; veri dosyalarından okunur (bkz. 15.2) ve **[VARSAYILAN]** kabul edilir.

---

## 12. Görsel yön

### 12.1 Stil [KARAR]
- Stilize 3D mobil oyun sanatı. Yumuşak, yuvarlak, oyuncak gibi formlar.
- Sıcak ve yumuşak ışık.
- **Dağınık alanlar:** loş, tozlu, hafif soluk renkler.
- **Temizlenmiş alanlar:** aydınlık, sıcak, canlı renkler. Bu kontrast oyunun temel görsel ödülüdür.
- **Nadir eşyalar:** yumuşak altın parıltı.
- Genel his: cozy, rahatlatıcı, tatmin edici (tidy-up ASMR).

### 12.2 Arayüz [KARAR]
- Krem/kırık beyaz zeminli, büyük köşe yuvarlamalı kartlar ve butonlar.
- Birincil eylem butonu sarı/altın; ikincil butonlar açık gri.
- Basit, okunaklı ikonlar. Metin az.
- Üst çubuk: geri butonu (sol), ilerleme göstergesi (orta), ayarlar (sağ).

### 12.3 Konsept görseller

**`concept/01_overview_warehouse.png` — Genel bakış.** İzometrik kesit görünümünde dört bölümlü bir depo. Sol üstte tamamen temizlenmiş, sıcak ışıklı "Office 100%" bölümü. Sağ üstte kutular, bisiklet ve eşyalarla dolu, loş "Garage 42%". Sol ortada yüksek metal raflar ve kutularla dolu "Aisle 8%". Sağ altta karanlık, kilit simgeli "Basement". Dağınık bölümlerde kutu taşıyan küçük yardımcı karakterler. Üstte "1,240 / 8,000 items" ilerleme çubuğu; köşelerde geri, ayarlar, ana sayfa ve "Items" butonları.

**`concept/02_section_garage.png` — Bölüm görünümü.** Garaj bölümünün içi, 3/4 üstten görünüm. Üstte "Comics", "Toys", "Tools" etiketli üç ahşap raf; kısmen dolu, boş yuvalar kesikli çizgili. Zeminin ortasında devrilmiş bir karton kutu ve etrafına saçılmış çizgi romanlar, oyuncaklar, aletler. Bir çizgi roman kesikli bir izle "Comics" rafına doğru sürükleniyor. Yığının içinde altın renkte parlayan küçük bir figür. Zeminde süpürülmüş temiz bir iz. Altta "Hand" (seçili), "Broom", "Magnet", "Magnifier" alet çubuğu; üstte "Garage 42%". (Büyüteç sonradan çıkarıldı, bkz. karar günlüğü.)

**`concept/03_rare_find_popup.png` — Nadir eşya bulma anı.** Bulanık, kararmış garaj arka planında, çizgi roman yığınının üstünde ışık huzmeleri ve parıltılar içinde yükselen süper kahraman kostümlü maskot. Altta "Rare find!" başlıklı kart, "Captain Chubby" adı, kısa açıklama. (Kartta görünen "Add to collection / Sell" seçimi geçersizdir, bkz. 9.3.)

### 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek]
- Çizgi roman kapaklarında gerçek markalar ve karakterler var ("Action Comics", "Detective Comics", Batman, Superman). Oyunda tamamen hayali yayınevleri, seriler ve karakterler kullanılacak.
- Captain Chubby'nin göğsündeki elmas içinde harf amblemi bilinen bir süper kahraman logosuna fazla yakın. Özgün bir amblem tasarlanacak.
- Yardımcı karakterler maskota çok benziyor. Ayrı karakter tasarımları yapılacak (bkz. 9.2).

---

## 13. Fikri mülkiyet ve özgünlük kuralları [KARAR]

- Oyunda hiçbir gerçek marka, logo, ürün adı, karakter, çizgi roman serisi, film veya oyun referansı yer almaz.
- Tüm eşyalar, markalar, kitap/çizgi roman kapakları ve kostümler özgün tasarlanır. "Benzeyen ama farklı adlı" taklitler de yapılmaz.
- Referans oyunların (bkz. bölüm 3) görselleri, isimleri, metinleri ve özgün öğeleri kopyalanmaz. Oyuncak ördek dükkânı teması kullanılmaz.
- AI ile üretilen görseller nihai varlık olarak kullanılmadan önce marka/karakter benzerliği açısından kontrol edilir.
- Mağaza görselleri ve reklam videoları için de aynı kurallar geçerlidir.

---

## 14. Ses ve haptik

- Her eşya türünün kendine özgü yerleştirme sesi (kâğıt hışırtısı, plastik tıkırtı, metal tıngırtı) **[KARAR]**.
- Süpürme sırasında sürekli, yumuşak bir fırçalama sesi.
- Raf dolduğunda, bölüm %100 olduğunda ve Chubby bulunduğunda katmanlı, ödüllendirici sesler. Nadir (mavi) eşya rafa oturduğunda küçük, parlak bir ek ses.
- Yardımcılara dokununca kısa, tatlı sesler.
- Dağınık bölümlerde sessiz/boğuk ortam sesi; temizlenince sıcak bir müzik katmanı devreye girer.
- Haptik: eşya rafa oturduğunda hafif tık, Chubby bulunduğunda belirgin titreşim. Ayarlardan kapatılabilir.

---

## 15. Teknik notlar (Unity)

Bu bölüm önerilerdir **[VARSAYILAN]**; uygulama sırasında daha iyi bir yol bulunursa Karar Günlüğü'ne yazılarak değiştirilebilir.

### 15.1 Genel
- Unity, URP (Universal Render Pipeline), mobil hedefli.
- 3D sahne + ortografik izometrik kamera (genel bakış) ve 3/4 açılı kamera (bölüm).
- Hedef: orta seviye Android cihazlarda kararlı 60 FPS.

### 15.2 Veri odaklı tasarım
İçerik koddan ayrı tutulur. ScriptableObject önerileri:
- `ItemDefinition` (id, kategori, nadirlik, görsel prefab/varyasyon, Coin değeri)
- `CategoryDefinition` (id, ad, ikon)
- `ContainerDefinition` (id, görsel, eşya havuzu/ağırlıkları)
- `SectionDefinition` (id, raflar ve kategorileri, konteynerler, kirlilik alanları, eşya sayısı)
- `VenueDefinition` (id, tema, bölümler, o mekânın Chubby'si)
- `CollectibleDefinition` (id, mekân, ad, açıklama; yalnızca Chubby'ler)
- `ToolDefinition` (seviyeler, fiyatlar, açılma koşulu)
- `HelperDefinition` (fiyat, açıldığı mekân, hız ve kapasite seviyeleri)
- Dengeleme tabloları (fiyatlar, eşikler, ödüller) tek bir yerde.

### 15.3 Performans
- Binlerce eşya aynı anda sahnede tutulmaz. Yalnızca aktif bölüm tam detaylı yüklenir.
- Genel bakışta bölümler önceden hazırlanmış görsel durumlarla (dağınık / kısmen / temiz) temsil edilir; tek tek eşyalar çizilmez.
- Eşya varyasyonları için GPU instancing ve ortak materyaller.
- Saçılma animasyonu için ağır fizik yerine hafif fizik veya önceden hesaplanmış/eğrisel animasyon tercih edilir.

### 15.4 Kayıt
- Yerel kayıt (JSON veya benzeri). Bulut kayıt **[AÇIK]**.
- Çevrimdışı ilerleme **yoktur** **[KARAR]** (prensip 11); zaman damgasına dayalı kazanç hesabı yapılmaz.
- Yardımcılar, yardımcı seviyeleri ve satın alınmış Oto Sort hakları kayıtta tutulur.
- Hangi eşyanın hangi rafta olduğu ve bölümlerin durumu kalıcı olarak saklanır (prensip 1: düzenlenen yer düzenli kalır).

### 15.5 Entegrasyonlar [AÇIK]
Reklam ağı, IAP (Unity IAP önerilir), analitik ve uzaktan yapılandırma (remote config) seçimleri henüz yapılmadı. Kod, bunları arayüzler (interface) arkasına alacak şekilde yazılmalı. MVP'de ödüllü reklam ve IAP sahte sağlayıcılarla çalışır; Oto Sort bu arayüzlerin ilk kullanıcısıdır.

### 15.6 Dil
Arayüz dili İngilizce **[VARSAYILAN]**. Türkçe sonradan eklenecek; tüm metinler baştan yerelleştirme sistemi üzerinden yazılır (koda gömülü metin yok).

---

## 16. MVP kapsamı

### 16.1 Dahil
- 3 mekân: Çizgi Roman Kutusu (öğretici), Garaj, Depo (4 bölüm).
- Genel bakış ve Bölüm görünümleri.
- Dokun-aç, sürükle-bırak, süpür hareketleri.
- 3 alet (El, Süpürge, Mıknatıs) ve temel yükseltmeleri.
- Oto Sort güçlendirmesi (ödüllü reklam / IAP ile, sahte sağlayıcılarla).
- Yardımcılar (en fazla 2), yükseltmeleri ve oda bitince sevme etkileşimi.
- Koleksiyon Kitabı (tek Chubby albümü) ve maskotun 3 kostümü.
- Chubby bulma anı; nadir (mavi) eşyalar.
- Bölüm %100 "önce / sonra" animasyonu; mekân tamamlanınca sıradakinin açılması.
- Ödüllü reklam ve IAP için altyapı (test/sahte sağlayıcılarla).
- Ses ve haptik geri bildirimi.

### 16.2 Dahil değil
- Sirk ve diğer büyük mekânlar.
- Premium para birimi.
- Çevrimdışı ilerleme (MVP sonrası için de planlanmıyor, bkz. prensip 11).
- Kalıcı Kategori Ustalığı (kaldırıldı, bkz. 10.2).
- Bulut kayıt, sosyal özellikler, sıralamalar.
- Etkinlikler (event) ve sezonluk içerik.
- Türkçe yerelleştirme (altyapı hazır olur, çeviri sonra).

### 16.3 MVP başarı kriteri [VARSAYILAN]
İlk 3 mekân, yeni bir oyuncu tarafından takılmadan bitirilebilmeli ve test oyuncuları özellikle şu üç anı "tatmin edici" bulmalı: rafın dolması, bölümün %100 olması, Chubby bulma anı. Ayrıca büyük odalar yardımcılarla birlikte "uzun ama keyifli" hissettirmeli; ne kendi kendine bitmeli ne de angaryaya dönmeli.

---

## 17. Sözlük

| Türkçe terim | Kod adı | Açıklama |
|---|---|---|
| Mekân | `Venue` | Satın alınan yer (kutu, garaj, depo, sirk...) |
| Bölüm | `Section` | Mekânın bir parçası (ofis, garaj, bodrum...) |
| Raf | `Shelf` | Bir kategoriye ait yerleştirme alanı |
| Yuva | `ShelfSlot` | Rafta bir eşyanın oturduğu yer |
| Konteyner | `Container` | Açılınca eşya saçan kutu, çanta, sandık |
| Kirlilik katmanı | `DirtLayer` | Süpürülerek temizlenen toz/kâğıt |
| Eşya | `Item` | Düzenlenebilir tekil nesne |
| Kategori | `Category` | Eşyanın ait olduğu grup |
| Nadir eşya | `ItemRarity.Rare` | Mavi parlayan, rafa konan, biraz daha fazla Coin veren özel eşya |
| Koleksiyon parçası | `Collectible` | Koleksiyon Kitabı'na giden parça; oyunda yalnızca Chubby'ler |
| Maskot (Chubby) | `Mascot` | Her mekânda kostümlü saklanan özgün figür |
| Koleksiyon Kitabı | `CollectionBook` | Chubby'lerin toplandığı albüm |
| Vitrin | `CollectionViewer` | Kitaptaki bir parçayı 3D döndürüp yakınlaştırarak inceleme ekranı |
| Set bonusu | `SetBonus` | Henüz karara bağlanmadı (bkz. 9.5) |
| Alet | `Tool` | El, Süpürge, Mıknatıs |
| Oto Sort | `AutoSortBoost` | Reklam ya da gerçek parayla alınan, bir odada bir rafın kategorisini oda bitene kadar kendiliğinden dizen güçlendirme (eski Kategori Ustalığı'nın yerine) |
| Yardımcı | `Helper` | Oyuncunun bulunduğu odada sıradan eşyaları yavaşça dizen, oda bitince sevilebilen küçük karakter |
| Genel bakış | `OverviewView` | İzometrik mekân görünümü |
| Bölüm görünümü | `SectionView` | Asıl oynanış ekranı |
| Temizlik yüzdesi | `SectionProgress` | Bölümün ne kadar düzenlendiği |

---

## 18. Açık sorular

1. Oyunun nihai adı.
2. Maskotun nihai tasarımı ve taban adı.
3. Yardımcı karakterlerin tasarımı.
4. Premium para birimi olacak mı, ne için kullanılacak?
5. Yardımcı dozu: toplam kaç yardımcı olacak, hangi mekânlarda açılacak, kaç yükseltme seviyesi olacak?
6. Gerçek "çöp" eşyalar ve çöp kutusu olacak mı?
7. Bölüm görünümünde pinch-zoom olacak mı?
8. Belirli sayıda Chubby bulununca kalıcı bir bonus verilecek mi (bkz. 9.5)?
9. Belli bir noktadan sonra mekân seçimi doğrusal olmaktan çıkacak mı?
10. Reklam ağı, analitik, bulut kayıt seçimleri.
11. Tüm dengeleme sayıları (alet ve yardımcı fiyatları, Mıknatıs bedeli, nadir eşya değeri ve sıklığı, yardımcı hızı).
12. Oto Sort dışında başka reklam ödülü ve IAP ürünü olacak mı? Oto Sort nadir (mavi) eşyaları da dizsin mi?

---

## 19. Karar günlüğü

| Tarih | Karar | Gerekçe |
|---|---|---|
| 2026-10-06 | Açık artırma yok, mekânlar sabit fiyatlı | Odak satın alma stratejisinde değil, düzenlemede olmalı. |
| 2026-10-06 | Tek tema yerine küçükten büyüğe çok temalı mekân merdiveni | Her mekân yeni bir kaos; içerik kendiliğinden tekrar oynanabilir. |
| 2026-10-06 | Koleksiyon Kitabı tek kuzey yıldızı | Çok tema kafa karıştırmasın; her mekân "yeni bir sayfa". |
| 2026-10-06 | Maskot (ördek değil) her temada kostümlü koleksiyon parçası | Sort Them Ducks kopyası görünmemek; oyuna özgün bir imza. |
| 2026-10-06 | Hafif renovasyon, tam iç dekorasyon yok | Odak dağılmasın, içerik maliyeti kontrol altında kalsın. |
| 2026-10-06 | Ayrı "çalışma masası" ekranı reddedildi; düzenleme bölümün içinde yapılır | Masa oyuncuyu mekândan koparıyordu; mekânın düzene girmesi görünmeli. |
| 2026-10-06 | Mühendislik gerektiren otomasyon yok; aletler + Kategori Ustalığı + yardımcılar | Basit, tek dokunuşluk ilerleme; mobil kitleye uygun. |
| 2026-10-06 | Yardımcılar nadir eşyalara dokunmaz | Otomasyon heyecanlı kısmı oyuncudan almamalı. |
| 2026-10-06 | Nadir eşyanın ilk kopyası otomatik kitaba, kopyalar otomatik satılır | Oyuncu "yanlış seçim yaptım" hissi yaşamamalı. |
| 2026-10-06 | Motor: Unity | Geliştirici tercihi. |
| 2026-10-06 | Gelir modeli: ödüllü reklam + IAP; koleksiyon parayla alınamaz | Cozy deneyimi bozmadan gelir. |
| 2026-10-06 | Yardımcılar maskottan ayrı karakterler | Konsept görsellerdeki karışıklığı gidermek. |
| 2026-10-06 | Koleksiyon Kitabı'na 3D Vitrin eklendi (döndür, yakınlaştır, kaydır) | Toplanan parçaları istendiği an sergilemek koleksiyon motivasyonunu güçlendirir (oyun sahibi isteği). |
| 2026-10-06 | Büyüteç (`Magnifier`) aletten çıkarıldı | Prototip testinde gereksiz bulundu (oyun sahibi kararı). |
| 2026-10-06 | El yükseltmesi = aynı anda taşıma kapasitesi (farklı türler birlikte); raf üzerinde bekleyince ait olanlar yerleşir | Birkaç eşyayı toplayıp raf raf dağıtmak; Mıknatıs'tan farklı bir rol. |
| 2026-10-06 | Mıknatıs sürekli çeker: taşıma sırasında küçük yarıçapa giren aynı kategori eşyalar ele gelir (1. seviyede 2) | Oyuncu parmağını gezdirerek toplar; pasif ve tatmin edici. |
| 2026-10-06 | Kategori Ustalığı: süpürülerek ortaya çıkan ve yeni oyunda yerde duran ustalaşılmış eşyalar da kendiliğinden rafa gider | Ustalık sonrası o kategoride elle iş kalmamalı. |
| 2026-10-06 | Prototipte mekân merdiveni küçük sayılarla kuruldu: Çizgi Roman Kutusu (12 eşya), Garaj (36), Depo (4 bölüm, 84). Her mekânda kostümlü bir Chubby (Captain / Mechanic / Night Guard). | Mekaniği test edilebilir tutmak; GDD 5.3 sayıları [VARSAYILAN]. |
| 2026-10-06 | Mekân satışı kaldırıldı: bir mekânın tüm bölümleri %100 olunca sonraki mekân ücretsiz açılır. Coin şimdilik alet ve oda kilidi için; ekonomi sonra yeniden düşünülecek. (5.6 ve 11 bu karara göre güncellenecek.) | Oyun sahibi kararı: ilerleme odaları temizleyerek olmalı. |
| 2026-10-06 | Bulunan bir koleksiyon parçası bir daha çıkmaz (kopya ve kopya satışı yok). Bir bölüm, içindeki tüm koleksiyon parçaları alınmadan bitmez. | Oyun sahibi kararı; 9.3 kopya kuralının yerini alır. |
| 2026-10-06 | Temizlenen odalar tekrar oynanmaz. Oda büyüklükleri: ~60 / ~200 / ~300 eşya. | Oyun sahibi kararı; odalar daha uzun sürmeli. |
| 2026-10-06 | Kalıcı Kategori Ustalığı kaldırıldı. Yerine Oto Sort güçlendirmesi: ödüllü reklam ya da gerçek para (IAP paketleri) ile alınır, Coin ile alınmaz; oda başına 1 raf; o rafın kategorisi oda bitene kadar kendiliğinden dizilir (sonradan dökülen ve süpürülerek çıkanlar dahil). 10.2 ve prensip 8 yeniden yazıldı. | Oyun sahibi kararı: kalıcı oto sort oyunu bir noktadan sonra fazla kolaylaştırıyordu. |
| 2026-10-06 | Yardımcılar ustalığın yerini alan kalıcı ilerleme katmanı oldu: Shop'tan Coin ile, yuvalar mekân ilerlemesine bağlı (her mekânda yeni yardımcı yok); oyuncunun bulunduğu odada belirir, sıradan eşyaları yavaşça tek tek dizer, sonradan çıkan eşyaları da toplar; kutuya, toza, nadir eşyaya ve Chubby'ye dokunmaz; yükseltmeleri (kapasite, hız) ucuz değildir. | Oyun sahibi kararı: ustalık kalkınca ilerleme çok uzamasın, ama oyun da uzun süre oynanabilir kalsın. |
| 2026-10-06 | Çevrimdışı (AFK) ilerleme ve kazanç yok; yardımcılar yalnızca oyuncu odadayken çalışır. Prensip 11 eklendi; 10.3, 11.2, 15.4 güncellendi. | Oyun sahibi kararı: oyuncu mutlaka oynamaya teşvik edilmeli. |
| 2026-10-06 | Oda bitince yardımcılar odada dolaşır; dokununca sevinirler (kalpler, tatlı sesler). Yardımcılar küçük, yuvarlak, yumuşak (bubbly / squishy) karakterlerdir. | Oyun sahibi isteği: yardımcılar evcil hayvan gibi sevilebilsin. |
| 2026-10-06 | Mıknatıs kilidi: El son seviyede olmadan alınamaz ve fiyatı belirgin şekilde yükseltildi. | Oyun sahibi kararı: mevcut halde El'i atlayıp doğrudan Mıknatıs almak en iyi hamleydi. |
| 2026-10-06 | Koleksiyon yalnızca Chubby'ler: mekân başına tek Chubby, kitap tek albüm. Eski altın nadir eşyalar mavi parlayan "nadir eşya"ya dönüştü: rafa konur, biraz daha fazla Coin verir, kitaba girmez, her odada bulunmaz. 8.1, 9 yeniden yazıldı; set bonusları [AÇIK] oldu. | Oyun sahibi kararı: koleksiyon parçası bulmak nadirleşsin ve değerlensin. |
| 2026-10-06 | Kilitler mekân ilerlemesine bağlı; ayrı bir oyuncu seviyesi / XP sistemi yok. | Oyun sahibi kararı; yeni bir sistem kurmadan dozu mekân merdiveniyle ayarlamak. |
| 2026-10-06 | 5.1, 5.2, 5.6, 4 ve 11, daha önce alınan "mekân satışı yok" kararına göre güncellendi. | Belge ile oyunun güncel hali arasındaki fark kapatıldı. |
