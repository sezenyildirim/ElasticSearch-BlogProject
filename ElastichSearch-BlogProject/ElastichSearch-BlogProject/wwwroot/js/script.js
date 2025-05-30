// Modal açma ve kapama işlemleri
document.addEventListener('DOMContentLoaded', function () {
    const searchButton = document.getElementById('search-button');
    const searchModal = document.getElementById('search-modal');
    const searchModalClose = document.getElementById('search-modal-close');
    const modalSearchInput = document.getElementById('modal-search-input');
    const modalSearchButton = document.getElementById('modal-search-button');
    const modalSearchResults = document.getElementById('modal-search-results');

    if (!searchButton) {
        console.error("Search button not found!");
    }
    if (!searchModal) {
        console.error("Search modal not found!");
    }

    // Modal'ı aç
    searchButton.addEventListener('click', function(e) {
        e.preventDefault();
        searchModal.style.display = 'flex';
        modalSearchInput.focus();
    });

    // Modal'ı kapat
    searchModalClose.addEventListener('click', function() {
        console.log("Close button clicked");
        searchModal.style.display = 'none';
        modalSearchInput.value = '';
        modalSearchResults.innerHTML = '';
    });

    // ESC tuşu ile modal'ı kapat
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape' && searchModal.style.display === 'flex') {
            searchModal.style.display = 'none';
            modalSearchInput.value = '';
            modalSearchResults.innerHTML = '';
        }
    });

    // Modal dışına tıklandığında kapat
    searchModal.addEventListener('click', function(e) {
        if (e.target === searchModal) {
            searchModal.style.display = 'none';
            modalSearchInput.value = '';
            modalSearchResults.innerHTML = '';
        }
    });

    // Arama butonuna tıklandığında
    modalSearchButton.addEventListener('click', handleModalSearch);

    // Enter tuşuna basıldığında
    modalSearchInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            handleModalSearch();
        }
    });

    // Input değiştiğinde kontrol et
    modalSearchInput.addEventListener('input', function() {
        const searchTerm = modalSearchInput.value.trim();
        if (searchTerm === '') {
            modalSearchResults.innerHTML = '';
        }
    });

    async function handleModalSearch() {
        const searchTerm = modalSearchInput.value.trim();
        if (searchTerm === '') {
            modalSearchResults.innerHTML = '';
            return;
        }

        // Yükleme göstergesi
        modalSearchResults.innerHTML = '<div class="loading-indicator">Aranıyor...</div>';

        try {
            // API'den verileri çek
            const response = await fetch(`/Blog/SearchModal?searchText=${encodeURIComponent(searchTerm)}`);
            
            if (!response.ok) {
                throw new Error('API yanıt vermedi');
            }
            
            const data = await response.json();
            displayModalFlowers(data);
        } catch (error) {
            console.error('Arama sırasında hata oluştu:', error);
            modalSearchResults.innerHTML = '<p class="error-message">Arama sırasında bir hata oluştu. Lütfen tekrar deneyin.</p>';
        }
    }

    function displayModalFlowers(items) {
        modalSearchResults.innerHTML = '';
        
        if (!items || items.length === 0) {
            modalSearchResults.innerHTML = '<p class="no-results">Sonuç bulunamadı.</p>';
            return;
        }

        items.forEach(item => {
            const card = document.createElement('div');
            card.className = 'modal-flower-card';
            card.innerHTML = `
                <img src="${item.image || 'https://via.placeholder.com/100x100?text=No+Image'}" alt="${item.name}" onerror="this.src='https://via.placeholder.com/100x100?text=No+Image'">
                <h3>${item.name || 'İsimsiz'}</h3>
                <p>${item.latinName || ''}</p>
                <a href="/Blog/Detail/${item._id}" class="view-details">Detayları Gör</a>
            `;
            modalSearchResults.appendChild(card);
        });
    }
});