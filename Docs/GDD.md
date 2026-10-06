# GDD — Chubby's Clutter (çalışma adı)

| Alan | Değer |
|---|---|
| Sürüm | 0.1 (ilk taslak) |
| Tarih | 2026-10-06 |
| Durum | Konsept netleşti, MVP geliştirmesine hazır |
| Platform | iOS ve Android (mobil), dikey ekran |
| Motor | Unity |
| Tür | Cozy / düzenleme / koleksiyon / hafif idle |
| Gelir modeli | Ödüllü reklam + uygulama içi satın alma |

## 0. Bu belge nasıl okunmalı

Bu belge oyunun tek doğruluk kaynağıdır. Geliştirme sırasında (insan ya da Claude Code) bir karar gerektiğinde önce buraya bakılır.

Belgedeki işaretler:

- **[KARAR]** Netleşmiş, tartışılmadan değiştirilmemesi gereken karar.
- **[VARSAYILAN]** Mantıklı bir başlangıç değeri. Test sonrası değişebilir, değişirse Karar Günlüğü'ne (bölüm 18) yazılır.
- **[AÇIK]** Henüz karara bağlanmamış konu. Kod bu konular için esnek yazılmalı, kesin varsayımda bulunmamalı.

Kod içinde kullanılacak isimler `kod_biçiminde` verilmiştir. Türkçe terimlerin kod karşılıkları için bölüm 17'deki sözlüğe bak.

Konsept görseller `concept/` klasöründedir. Görselleri göremeyen okuyucular için her birinin açıklaması bölüm 12'de yazılıdır.

---

## 1. Özet

Oyuncu dağınık, terk edilmiş yerler satın alır: önce küçük bir kutu, sonra bir garaj, bir depo, ileride kapanmış bir oyuncakçı veya terk edilmiş bir sirk. Bu yerlerdeki binlerce eşyayı tek tek ayıklayıp doğru raflara yerleştirir, tozu süpürür ve kaosu düzene çevirir. Eşyaların neredeyse tamamı para getirir; bu para işi hızlandıran aletlere ve yeni mekânlara harcanır. Asıl hedef ise yığınların içinde saklı nadir koleksiyon parçalarını, özellikle her mekânda temaya uygun kostümüyle saklanan maskot figürü bulmak ve Koleksiyon Kitabı'nı tamamlamaktır. Temizlenen mekân restore edilmiş haliyle satılır ve bir sonraki, daha büyük mekâna geçilir.

**Tek cümlelik konsept:** Dağınık yerleri satın al, eşya eşya düzenle, içinden çıkan hazineleri topla.

**Hedef kitle:** Cozy, ASMR, "oddly satisfying" içerikleri seven geniş mobil kitle. Kısa oturumlarla oynayan, ama uzun vadeli koleksiyon hedeflerinden keyif alan oyuncular.

---

## 2. Değişmez prensipler

Bu prensipler oyunun kimliğidir. Bir özellik bunlardan biriyle çelişiyorsa, özellik değişir; prensip değil.

1. **Kaostan düzene, gözle görülür şekilde.** Her eylem mekânı görünür biçimde daha düzenli hale getirmelidir. Oyuncunun düzenlediği yer düzenli kalır; çıkıp geri döndüğünde emeğini görür.
2. **Baskı yok.** Zaman sınırı yok, yanlış yerleştirmeye ceza yok, enerji sistemi yok, oyuncuyu bekleten duvarlar yok. Oyun her zaman rahatlatıcıdır.
3. **Basit eylem, büyük ölçek.** Temel eylemler tek parmakla yapılır (dokun, sürükle, kaydır). Derinlik eylemlerin karmaşıklığından değil, eşyaların miktarından ve çeşitliliğinden gelir.
4. **Tek kuzey yıldızı: Koleksiyon Kitabı.** Oyuncu her an neyin peşinde olduğunu bilir. Para bir araçtır, hedef koleksiyondur.
5. **%99 yakıt, %1 hazine.** Sıradan eşyalar para ve ilerleme getirir; nadir eşyalar oyunun asıl ödülüdür. Bu oran her mekânda korunur.
6. **Otomasyon sıkıcı kısmı alır, heyecanlı kısmı asla.** Hiçbir otomasyon sistemi (yardımcılar dahil) nadir eşyaları oyuncunun yerine bulmaz veya toplamaz.
7. **Mühendislik yok.** Konveyör, üretim hattı, yerleşim planı, kaynak zinciri gibi kurulum gerektiren sistemler oyuna girmez. Otomasyon tek dokunuşla satın alınır ve çalışır.
8. **Bilgi = hız.** Oyuncu kategorileri öğrendikçe hızlanır; Kategori Ustalığı bu öğrenmeyi doğrudan mekaniğe çevirir.
9. **Para ile koleksiyon satın alınmaz.** Gerçek parayla belirli bir nadir eşya, koleksiyon parçası veya maskot satın alınamaz. Gerçek para ile satın alınan rastgele ödül (loot box) yoktur.
10. **Tamamen özgün içerik.** Gerçek markalar, logolar, karakterler veya telifli eserler oyunda yer almaz (bkz. bölüm 13).

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
Kutuya dokun → kutu yerinde devrilir, eşyalar zemine saçılır → eşyayı doğru rafa sürükle → para kazan, raf dolar → tozu süpür, altından yeni eşyalar çıkar → nadir eşya parlarsa al, Koleksiyon Kitabı'na gider.

### 4.2 Oturum döngüsü (1–5 dakika)
Genel bakıştan bir bölüm seç → birkaç kutu/yığın düzenle → bölüm yüzdesi artar → yeni alet veya yükseltme al → yardımcıları bölümlere ata → oyundan çık (yardımcılar çalışmaya devam eder).

### 4.3 Meta döngü (günler/haftalar)
Mekân satın al (sabit fiyat) → bölümleri temizle ve düzenle → mekânın koleksiyon sayfasını doldur → mekân %100 olunca restore edilmiş halini sat → büyük ödeme ile bir sonraki mekânı aç.

### 4.4 Kaynak akışı
- Sıradan eşya doğru rafa → **Coin** (anında).
- Nadir eşyanın ilk kopyası → **Koleksiyon Kitabı**.
- Nadir eşyanın tekrar kopyaları → yüksek değerli **Coin**.
- Coin → aletler, yükseltmeler, yardımcılar, yeni mekânlar.
- Tamamlanan koleksiyon sayfası → **kalıcı set bonusu**.
- Satılan mekân → büyük **Coin** ödemesi.

---

## 5. Mekânlar ve bölümler

### 5.1 Yapı [KARAR]
- Her mekân (`Venue`) bir veya daha fazla bölümden (`Section`) oluşur.
- Mekânlar **sabit fiyatla** satın alınır. Açık artırma veya teklif sistemi yoktur.
- Mekânlar küçükten büyüğe bir merdiven halinde ilerler. Büyüdükçe eşya sayısı, kategori sayısı ve nadir eşya sayısı artar.
- Her mekânın bir teması vardır (çizgi romanlar, garaj, sirk...). Tema; eşya havuzunu, kategorileri, koleksiyon sayfasını ve maskotun kostümünü belirler.

### 5.2 İlerleme sırası [VARSAYILAN]
Mekânlar doğrusal sırayla açılır: bir mekân satılınca bir sonraki satın alınabilir. İleride belli bir noktadan sonra haritada birden fazla seçenek sunmak **[AÇIK]**.

### 5.3 Mekân merdiveni (taslak)

| # | Mekân | Bölümler | Eşya (yaklaşık) | Not |
|---|---|---|---|---|
| 1 | Çizgi Roman Kutusu | 1 | 50 | Öğretici. Tek ekran, 2–3 dakika. |
| 2 | Garaj | 1–2 | 500 | İlk alet yükseltmeleri. |
| 3 | Depo | 4 (Ofis, Garaj, Raf Koridoru, Bodrum) | 8.000 | İlk yardımcılar, ilk Kategori Ustalığı. |
| 4 | Kapanmış Oyuncakçı | 3–4 | — | MVP sonrası |
| 5 | Tavan Arası / Eski Ev | 3–4 | — | MVP sonrası |
| 6 | Terk Edilmiş Sirk | 4–5 (Kostüm Odası, Bilet Gişesi, Palyaço Karavanı, Ana Çadır...) | — | MVP sonrası |
| 7+ | Eski Tiyatro, Lunapark... | — | — | MVP sonrası |

Sayılar **[VARSAYILAN]**, dengeleme testlerinde değişecek.

### 5.4 Bölüm kilitleri [VARSAYILAN]
Bir mekân içindeki bazı bölümler kilitli başlar (ör. Depo'daki Bodrum). Kilit, mekân içindeki diğer bölümlerin belli bir yüzdeye ulaşmasıyla veya Coin ile açılır.

### 5.5 Hafif renovasyon [KARAR]
- Oyunda duvar boyama, mobilya seçme gibi iç dekorasyon **yoktur**. Bu, prensip 3 ve 7 ile çelişir ve içerik maliyetini katlar.
- Renovasyon kendiliğinden olur: bir bölüm %100 olduğunda kısa bir "önce / sonra" animasyonu oynar. Toz kaybolur, ışıklar yanar, renkler canlanır, ortam sesi değişir.
- Temizlenen bölümler genel bakışta temiz ve renkli kalır.

### 5.6 Mekânın satışı [KARAR]
- Mekânın tüm bölümleri %100 olduğunda "Mekânı Sat" seçeneği açılır.
- Satış büyük bir Coin ödemesi verir ve bir sonraki mekânın kilidini açar.
- Satılan mekânlar haritada "Satıldı" tabelasıyla bir kupa olarak görünmeye devam eder.
- Satıştan önce mekândaki tüm nadir eşyaların bulunması zorunlu **değildir** **[VARSAYILAN]**. Bulunmayan parçalar için eski mekânlara dönüş mekaniği **[AÇIK]**.

---

## 6. Görünümler ve kamera

Oyunda iki ana görünüm vardır. Ayrı bir "çalışma masası" ekranı **yoktur** **[KARAR]**; bu fikir, oyuncuyu mekândan kopardığı için reddedildi.

### 6.1 Genel bakış (`OverviewView`) [KARAR]
- İzometrik, üstten kesit (cutaway) görünüm. Mekânın tüm bölümleri aynı anda görünür.
- Amacı ölçeği hissettirmek ve ilerlemeyi göstermektir. Burada eşya düzenlenmez.
- Dağınık bölümler loş, tozlu ve soluk; temizlenen bölümler aydınlık, sıcak ve renklidir.
- Her bölümün üstünde bir etiket: bölüm adı + temizlik yüzdesi. Kilitli bölümler karanlık ve kilit simgelidir.
- Üstte toplam ilerleme sayacı (ör. `1,240 / 8,000 items`).
- Yardımcılar bölümlerde dolaşıp çalışırken görünür.
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
| Parlayan nadir eşyaya dokunma | Nadir eşya bulma anı tetiklenir (bkz. 9.3). |

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
| Nadir eşya | `ItemRarity.Rare` | Altın ışıkla parlar. İlk kopya Koleksiyon Kitabı'na, sonrakiler satılır. |
| Maskot figürü | `ItemRarity.Mascot` | Her mekânda temaya uygun kostümlü maskot. Nadir eşyanın en değerli alt türü. |
| Kaba nesne (konteyner) | `ContainerDefinition` | Kutu, çanta, sandık. Açılınca içindeki eşyaları saçar. |
| Kirlilik | `DirtLayer` | Süpürülerek temizlenir. Eşya değildir. |

### 8.2 Kategoriler [KARAR]
- Her sıradan eşya tek bir kategoriye aittir (ör. Çizgi Roman, Oyuncak, Alet).
- Kategoriler **mekânlar arası ortaktır**: "Oyuncak" kategorisi birden fazla mekânda çıkabilir. Bu, Kategori Ustalığı'nın (10.2) mekânlar arası taşınmasını sağlar.
- Her yeni mekân en az birkaç **yeni** kategori getirir; böylece her mekânda elle düzenleme tekrar başlar.
- Kategori, oyuncunun eşyanın görünüşünden tanıyabileceği kadar net olmalıdır. Belirsiz eşya tasarlanmaz.

### 8.3 Eşya çeşitliliği
Aynı kategorideki eşyalar görsel olarak farklı varyasyonlara sahiptir (farklı kapak, renk, şekil). Sort Them Ducks'taki "her ördek farklı" hissi hedeflenir. Varyasyonlar maliyet için modüler üretilmelidir (taban model + doku/renk/aksesuar kombinasyonları).

---

## 9. Koleksiyon sistemi

### 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR]
- Oyunun kuzey yıldızı.
- Her mekânın kitapta kendi sayfası vardır. Sayfada o mekânın nadir eşyaları için yuvalar bulunur; bulunmamış olanlar gri silüet olarak görünür.
- Sayfada ilerleme sayacı (ör. `7 / 12`).
- Scrapbook / albüm estetiği: kâğıt dokusu, çıkartmalar, damgalar.

### 9.1.1 Vitrin (`CollectionViewer`) [KARAR]
- Kitapta bulunmuş bir parçaya dokununca parça, kararmış ekranın ortasında 3D olarak açılır.
- Oyuncu parçayı tek parmakla döndürür, iki parmakla yakınlaştırır ve kaydırır (editörde: fare tekerleği ve sağ tık sürükleme). Çift dokunuş görünümü sıfırlar; dokunulmadığında parça yavaşça kendi etrafında döner.
- Altta parçanın adı ve kısa açıklaması, tek bir "Kapat" butonu. Kapatınca kitaba dönülür.
- Amaç: toplanan parçaları istendiği an sergileyebilmek; koleksiyonun kendisi ödül hissi verir (prensip 4).

### 9.2 Maskot [KARAR]
- Oyunun özgün bir maskotu vardır: yuvarlak gövdeli, küçük kollu-bacaklı, iri ve sevimli gözlü bir koleksiyon figürü. Taban adı **Chubby** **[VARSAYILAN]**.
- Her mekânda maskot temaya uygun bir kostümle saklıdır (ör. Çizgi Roman Kutusu'nda süper kahraman kostümlü "Captain Chubby", Sirk'te sunucu kostümlü Chubby).
- Bir mekânda maskotun kendisi, ya da maskot + ona ait küçük aksesuar parçaları bulunabilir **[AÇIK: mekân başına tek figür mü, figür + eşyaları mı]**.
- Maskot tasarımı hiçbir mevcut karakteri veya logoyu çağrıştırmamalıdır (bkz. bölüm 13).
- **Yardımcılar maskottan ayrı karakterlerdir** **[KARAR]**. Konsept görsellerde ikisi birbirine çok benziyor; nihai tasarımda net şekilde ayrışmalılar.

### 9.3 Nadir eşya bulma anı [KARAR]
- Nadir eşya zeminde altın ışıkla parlar. Oyuncu dokununca eşya yükselir, ışık huzmeleri ve parıltılar çıkar, arka plan kararır ve bulanıklaşır.
- **İlk kopya otomatik olarak Koleksiyon Kitabı'na eklenir.** Oyuncuya "sat mı, sakla mı" seçimi sunulmaz.
- Kartta: "Rare find!" başlığı, eşyanın adı, kısa ve sevimli bir açıklama, tek bir "Devam" / "Kitaba git" butonu.
- **Tekrar kopyalar** otomatik olarak yüksek değerle satılır; kısa bir "Kopya satıldı +X Coin" bildirimi gösterilir.
- Not: `concept/03_rare_find_popup.png` görselinde "Add to collection / Sell" seçimi var. Bu görsel **eski** davranışı gösterir; uygulanacak davranış yukarıdaki gibidir.

### 9.4 Set bonusları [KARAR]
Bir koleksiyon sayfası tamamlandığında **kalıcı** bir bonus verilir (ör. "Tüm mekânlarda nadir eşya parıltısı daha belirgin", "Coin kazancı +%5"). Bonus değerleri **[VARSAYILAN]**, dengelemede belirlenecek.

---

## 10. Otomasyon ve ilerleme

Üç katman vardır. Hiçbiri kurulum, yerleşim veya mühendislik gerektirmez (prensip 7).

### 10.1 Aletler (`Tool`) [KARAR]
Oyuncunun kendi elini güçlendirir. Alet çubuğunda seçilir, Coin ile seviye atlatılır.

| Alet | Temel işlev | Yükseltme örnekleri |
|---|---|---|
| El (`Hand`) | Eşya taşıma. Taşırken üzerinden geçilen eşyalar da ele alınır (her türden, kapasite kadar). Bir rafın üzerinde kısa süre beklenince eldekilerden o rafa ait olanlar yerleşir, diğerleri elde kalır. | Aynı anda taşınabilen eşya sayısı (1 → 2 → 3) |
| Süpürge (`Broom`) | Kirlilik katmanını temizleme | Daha geniş süpürme alanı |
| Mıknatıs (`Magnet`) | Bir eşya taşınırken, parmak hareket ettikçe küçük çekim yarıçapına giren aynı kategoriden eşyalar ele çekilir | Daha çok eşya (1. seviyede 2), biraz daha geniş yarıçap |

### 10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR]
Oyunun imza mekaniği.
- Oyuncu bir kategoriden belirli sayıda eşyayı doğru rafa yerleştirdiğinde o kategoride ustalaşır (ör. 100 çizgi roman).
- Ustalaşılan kategorinin eşyaları, bir konteyner açıldığında **kendiliğinden uçarak** doğru rafa yerleşir.
- Ustalık mekânlar arası **kalıcıdır**.
- Ustalık seviyeli olabilir (ör. Bronz: rafı gösterir, Gümüş: yakındakiler kendiliğinden gider, Altın: tümü kendiliğinden gider) **[AÇIK]**.
- Ustalık anında kutlama: banner ("Category mastered: Comics") + eşyaların raflara uçtuğu tatmin edici animasyon.
- **Nadir eşyalar ustalıktan asla etkilenmez**; her zaman oyuncunun kendisi tarafından alınır (prensip 6).

### 10.3 Yardımcılar (`Helper`) [KARAR]
- Coin ile işe alınan, maskottan ayrı sevimli karakterler.
- Tek dokunuşla bir bölüme atanırlar ve oradaki sıradan eşyaları yavaşça düzenlerler.
- Oyuncu oyunda yokken de çalışırlar (çevrimdışı ilerleme, bkz. 15.4). Oyuna dönüşte "Siz yokken..." özeti gösterilir.
- Yükseltme yalnızca hız/kapasite artırır. Yerleşim, rota veya planlama yoktur.
- **Nadir eşyalara dokunmazlar.** Bir yardımcı bölümü temizlediğinde nadir eşyalar zeminde parlayarak oyuncuyu bekler.
- Eski, tamamen temizlenmiş mekânlar satıldığı için yardımcıların çalıştığı yer her zaman aktif mekândır **[VARSAYILAN]**.

### 10.4 Ölçek büyüdükçe oyuncunun rolü
Küçük mekânlarda oyuncu her eşyayı eliyle düzenler. Büyük mekânlarda ustalık ve yardımcılar sıradan işi üstlenir; oyuncu yeni kategorileri öğrenmeye, nadir eşya avına ve tatmin edici anlara odaklanır. Elle yapılan iş her zaman kısa ve keyifli kalır.

---

## 11. Ekonomi ve gelir modeli

### 11.1 Para birimleri
- **Coin** (yumuşak para) [KARAR]: Oyun içinde kazanılır. Aletler, yükseltmeler, yardımcılar, mekânlar.
- **Premium para** (ör. Gem) **[AÇIK]**: Gerekip gerekmediği ve ne için kullanılacağı netleşmedi. Eklenirse prensip 9 geçerlidir.

### 11.2 Ödüllü reklamlar [KARAR]
Yalnızca oyuncu isteyerek izler. Örnek ödüller:
- Belirli süre 2x Coin
- Yardımcılara geçici hız artışı
- Mekân satışında bonus ödeme
- Çevrimdışı kazancı katlama

### 11.3 Uygulama içi satın almalar [KARAR]
Örnekler: başlangıç paketi, Coin paketleri, ek yardımcı yuvası, kozmetik raf/albüm temaları, reklamsız ödül paketi (reklamı izlemeden ödülleri almak).

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
- Raf dolduğunda, bölüm %100 olduğunda ve nadir eşya bulunduğunda katmanlı, ödüllendirici sesler.
- Dağınık bölümlerde sessiz/boğuk ortam sesi; temizlenince sıcak bir müzik katmanı devreye girer.
- Haptik: eşya rafa oturduğunda hafif tık, nadir eşya bulunduğunda belirgin titreşim. Ayarlardan kapatılabilir.

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
- `CategoryDefinition` (id, ad, ikon, ustalık eşiği)
- `ContainerDefinition` (id, görsel, eşya havuzu/ağırlıkları)
- `SectionDefinition` (id, raflar ve kategorileri, konteynerler, kirlilik alanları, eşya sayısı)
- `VenueDefinition` (id, tema, fiyat, bölümler, satış değeri, koleksiyon sayfası)
- `CollectibleDefinition` (id, mekân, ad, açıklama, maskot mu)
- `ToolDefinition`, `HelperDefinition`, `SetBonusDefinition`
- Dengeleme tabloları (fiyatlar, eşikler, ödüller) tek bir yerde.

### 15.3 Performans
- Binlerce eşya aynı anda sahnede tutulmaz. Yalnızca aktif bölüm tam detaylı yüklenir.
- Genel bakışta bölümler önceden hazırlanmış görsel durumlarla (dağınık / kısmen / temiz) temsil edilir; tek tek eşyalar çizilmez.
- Eşya varyasyonları için GPU instancing ve ortak materyaller.
- Saçılma animasyonu için ağır fizik yerine hafif fizik veya önceden hesaplanmış/eğrisel animasyon tercih edilir.

### 15.4 Kayıt ve çevrimdışı ilerleme
- Yerel kayıt (JSON veya benzeri). Bulut kayıt **[AÇIK]**.
- Çevrimdışı ilerleme: son çıkış zaman damgası üzerinden yardımcı çalışmasının hesaplanması. Üst sınır (ör. en fazla 8 saat) **[VARSAYILAN]**.
- Hangi eşyanın hangi rafta olduğu ve bölümlerin durumu kalıcı olarak saklanır (prensip 1: düzenlenen yer düzenli kalır).

### 15.5 Entegrasyonlar [AÇIK]
Reklam ağı, IAP (Unity IAP önerilir), analitik ve uzaktan yapılandırma (remote config) seçimleri henüz yapılmadı. Kod, bunları arayüzler (interface) arkasına alacak şekilde yazılmalı.

### 15.6 Dil
Arayüz dili İngilizce **[VARSAYILAN]**. Türkçe sonradan eklenecek; tüm metinler baştan yerelleştirme sistemi üzerinden yazılır (koda gömülü metin yok).

---

## 16. MVP kapsamı

### 16.1 Dahil
- 3 mekân: Çizgi Roman Kutusu (öğretici), Garaj, Depo (4 bölüm).
- Genel bakış ve Bölüm görünümleri.
- Dokun-aç, sürükle-bırak, süpür hareketleri.
- 3 alet (El, Süpürge, Mıknatıs) ve temel yükseltmeleri.
- Kategori Ustalığı.
- Yardımcılar ve çevrimdışı ilerleme.
- Koleksiyon Kitabı (3 sayfa) ve maskotun 3 kostümü.
- Nadir eşya bulma anı.
- Bölüm %100 "önce / sonra" animasyonu ve mekân satışı.
- Ödüllü reklam ve IAP için altyapı (test/sahte sağlayıcılarla da olabilir).
- Ses ve haptik geri bildirimi.

### 16.2 Dahil değil
- Sirk ve diğer büyük mekânlar.
- Premium para birimi.
- Bulut kayıt, sosyal özellikler, sıralamalar.
- Etkinlikler (event) ve sezonluk içerik.
- Türkçe yerelleştirme (altyapı hazır olur, çeviri sonra).

### 16.3 MVP başarı kriteri [VARSAYILAN]
İlk 3 mekân, yeni bir oyuncu tarafından takılmadan bitirilebilmeli ve test oyuncuları özellikle şu üç anı "tatmin edici" bulmalı: rafın dolması, bölümün %100 olması, nadir eşya bulma anı.

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
| Nadir eşya | `Collectible` | Koleksiyon Kitabı'na giden eşya |
| Maskot | `Mascot` | Her mekânda kostümlü saklanan özgün figür |
| Koleksiyon Kitabı | `CollectionBook` | Tüm nadir eşyaların toplandığı albüm |
| Vitrin | `CollectionViewer` | Kitaptaki bir parçayı 3D döndürüp yakınlaştırarak inceleme ekranı |
| Set bonusu | `SetBonus` | Tamamlanan sayfanın kalıcı ödülü |
| Alet | `Tool` | El, Süpürge, Mıknatıs |
| Kategori Ustalığı | `CategoryMastery` | Kategoriyi öğrenince gelen otomatik yerleştirme |
| Yardımcı | `Helper` | Bölüme atanıp kendi başına düzenleyen karakter |
| Genel bakış | `OverviewView` | İzometrik mekân görünümü |
| Bölüm görünümü | `SectionView` | Asıl oynanış ekranı |
| Temizlik yüzdesi | `SectionProgress` | Bölümün ne kadar düzenlendiği |

---

## 18. Açık sorular

1. Oyunun nihai adı.
2. Maskotun nihai tasarımı ve taban adı; mekân başına tek figür mü, figür + aksesuar parçaları mı?
3. Yardımcı karakterlerin tasarımı.
4. Premium para birimi olacak mı, ne için kullanılacak?
5. Kategori Ustalığı tek seviyeli mi, çok seviyeli mi?
6. Gerçek "çöp" eşyalar ve çöp kutusu olacak mı?
7. Bölüm görünümünde pinch-zoom olacak mı?
8. Mekân satışından sonra bulunamamış nadir eşyalar için geri dönüş mekaniği.
9. Belli bir noktadan sonra mekân seçimi doğrusal olmaktan çıkacak mı?
10. Reklam ağı, analitik, bulut kayıt seçimleri.
11. Tüm dengeleme sayıları (fiyatlar, eşikler, ödüller, çevrimdışı limit).

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
