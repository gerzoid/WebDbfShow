    window.addEventListener('load', function() {
    // Скрываем preloader
    const preloader = document.querySelector('.preloader');
    preloader.style.display = 'none';

    // Показываем основное содержимое
    const content = document.querySelector('.content');
    content.style.display = 'block';
  });
