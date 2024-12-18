document.addEventListener('DOMContentLoaded', function() {
    // Get current page path
    const currentPath = window.location.pathname;
    const pageName = currentPath.split('/').pop() || 'index.html';
    
    // Find the corresponding nav link
    const navLinks = document.querySelectorAll('.nav-links a');
    navLinks.forEach(link => {
        // Remove any existing active classes
        link.classList.remove('active');
        
        // If this link matches current page, add active class
        if (link.getAttribute('href') === pageName) {
            link.classList.add('active');
        }
    });

    // Keep the active state when scrolling
    let ticking = false;
    window.addEventListener('scroll', function() {
        if (!ticking) {
            window.requestAnimationFrame(function() {
                // Find the active link again and ensure it stays active
                const activeLink = document.querySelector('.nav-links a[href="' + pageName + '"]');
                if (activeLink && !activeLink.classList.contains('active')) {
                    navLinks.forEach(link => link.classList.remove('active'));
                    activeLink.classList.add('active');
                }
                ticking = false;
            });
            ticking = true;
        }
    });
});