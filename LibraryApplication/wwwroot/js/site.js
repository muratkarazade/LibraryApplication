    document.addEventListener("DOMContentLoaded", function() {
        document.querySelector("form").addEventListener("submit", function (e) {
            const fileInput = document.querySelector('input[type="file"]');
            const file = fileInput.files[0];

            // 5MB kontrolü
            if (file.size > 5 * 1024 * 1024) {
                alert("Dosya boyutu 5MB'den büyük olamaz!");
                e.preventDefault();
                return;
            }

            // Dosya uzantısı kontrolü (Örneğin: .jpg veya .png)
            const validExtensions = ['.jpg', '.png'];
            const fileExtension = '.' + file.name.split('.').pop().toLowerCase();
            if (!validExtensions.includes(fileExtension)) {
                alert("Sadece .jpg ve .png uzantılı dosyalar kabul edilir.");
                e.preventDefault();
                return;
            }
        });
    // Dosya input elementini al
    const actualFile = document.getElementById('actual-file');
    // Seçilen dosyanın adını gösterecek elementi al
    const fileChosen = document.getElementById('file-chosen');

    // Dosya input elementinde değişiklik olduğunda tetiklenecek fonksiyon
    actualFile.addEventListener('change', function(){
    // Eğer dosya seçildiyse
    if (actualFile.value) {
        // Dosya yolu içerisinden dosya adını çekip göster
        fileChosen.textContent = actualFile.value.match(/[\/\\]([\w\d\s\.\-\(\)]+)$/)[1];
    } else {
        // Eğer dosya seçilmediyse varsayılan mesajı göster
        fileChosen.textContent = 'Dosya seçilmedi';
    }
    });   
});


