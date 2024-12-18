document.addEventListener('DOMContentLoaded', () => {
    // Mobile menu toggle
    const menuButton = document.createElement('button');
    menuButton.classList.add('menu-toggle');
    menuButton.innerHTML = '<i class="fas fa-bars"></i>';
    document.body.appendChild(menuButton);

    menuButton.addEventListener('click', () => {
        document.querySelector('.sidebar').classList.toggle('active');
    });

    // Close sidebar when clicking outside
    document.addEventListener('click', (e) => {
        if (!e.target.closest('.sidebar') && !e.target.closest('.menu-toggle')) {
            document.querySelector('.sidebar').classList.remove('active');
        }
    });

    // Add copy button to code blocks
    document.querySelectorAll('pre code').forEach((block) => {
        const button = document.createElement('button');
        button.className = 'copy-button';
        button.textContent = 'Copy';
        
        button.addEventListener('click', async () => {
            try {
                await navigator.clipboard.writeText(block.textContent);
                button.textContent = 'Copied!';
                setTimeout(() => {
                    button.textContent = 'Copy';
                }, 2000);
            } catch (err) {
                console.error('Failed to copy text:', err);
            }
        });

        const wrapper = document.createElement('div');
        wrapper.className = 'code-wrapper';
        block.parentNode.insertBefore(wrapper, block);
        wrapper.appendChild(block);
        wrapper.appendChild(button);
    });

    // Highlight active nav item based on scroll position
    const sections = document.querySelectorAll('section[id]');
    const navLinks = document.querySelectorAll('.nav-links a');

    function highlightNavItem() {
        const scrollY = window.scrollY;
        
        sections.forEach(section => {
            const sectionTop = section.offsetTop - 100;
            const sectionBottom = sectionTop + section.offsetHeight;
            const sectionId = section.getAttribute('id');
            
            if (scrollY >= sectionTop && scrollY < sectionBottom) {
                navLinks.forEach(link => {
                    link.classList.remove('active');
                    if (link.getAttribute('href').includes(sectionId)) {
                        link.classList.add('active');
                    }
                });
            }
        });
    }

    window.addEventListener('scroll', highlightNavItem);
});